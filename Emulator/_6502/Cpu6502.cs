using Emulator._6502.Instructions;

using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;

namespace Emulator._6502
{
    public delegate byte OnCPUReadByte(ushort address);
    public delegate void OnCPUWriteByte(ushort address, byte data);
    public delegate string DebugGetDeviceName(ushort address, DeviceConfig config);

    [DebuggerDisplay($"{{{nameof(GetDebuggerDisplay)}(),nq}}")]
    public class Cpu6502
    {
        public byte A { get; set; }
        public byte X { get; set; }
        public byte Y { get; set; }
        public byte STKP { get; set; }
        public ushort PC { get; set; }
        public Status6502 Status { get; set; }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public void SetFlag(Status6502 f, bool condition)
        {
            if (condition)
                Status |= f;
            else
                Status &= ~f;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public bool GetFlag(Status6502 f) => (Status & f) != 0;

        private readonly UnknownInstruction _unknownInstruction;
        private readonly InstructionSetMOS6502 _instructions;

        public event OnCPUReadByte OnCPUReadByte;
        public event OnCPUWriteByte OnCPUWriteByte;
        public event DebugGetDeviceName OnDebugGetDeviceName;

        public List<IDevice6502> Devices { get; private set; }


        public void RegisterDevice<T>(T instance) where T : IDevice6502
        {
            if (instance == null) return;

            if (instance.DeviceFlags == DeviceConfig.Invalid) return;
            var s = CheckDeviceOverlap(instance);
            if (s != null)
            {
                var sadd = s.Value.address;
                var sdev = s.Value.dev;
#if DEBUG
                throw new InvalidOperationException($"device: {instance.DebugName} overlaps with device: {sdev} at address 0x{sadd:X}");
#else
                throw new InvalidOperationException($"a device attempted to register a handler that is overlapping with another device at address 0x{sadd:X}");
#endif
            }

            if (instance.DeviceFlags.HasFlag(DeviceConfig.ReadOnly))
            {
                OnCPUReadByte += instance.ReadByte;
            }

            if (instance.DeviceFlags.HasFlag(DeviceConfig.WriteOnly))
            {
                OnCPUWriteByte += instance.WriteByte;
            }
#if DEBUG
            if (instance.DeviceFlags.HasFlag(DeviceConfig.Debug))
            {
                OnDebugGetDeviceName += instance.GetDebugDeviceName;
            }
#endif
            Devices.Add(instance);
        }


        public void RegisterDevice<T>() where T : IDevice6502, new()
        {
            RegisterDevice(new T());
        }


        public void UnregisterDevices()
        {
            foreach (var instance in Devices)
            {
                if (instance.DeviceFlags.HasFlag(DeviceConfig.ReadOnly))
                {
                    OnCPUReadByte -= instance.ReadByte;
                }

                if (instance.DeviceFlags.HasFlag(DeviceConfig.WriteOnly))
                {
                    OnCPUWriteByte -= instance.WriteByte;
                }

#if DEBUG
                if (instance.DeviceFlags.HasFlag(DeviceConfig.Debug))
                {
                    OnDebugGetDeviceName -= instance.GetDebugDeviceName;
                }
#endif
            }

            Devices.Clear();
        }

        public byte ReadByte(ushort address)
        {
            return OnCPUReadByte(address);
        }

        public void WriteByte(ushort address, byte data)
        {
            OnCPUWriteByte(address, data);
        }
        /// <summary>
        /// Pop a byte off the Stack
        /// </summary>
        /// <returns></returns>
        public byte PopByte()
        {
            return OnCPUReadByte((ushort)(0x0100 | ++STKP));
        }

        /// <summary>
        /// Push a byte to the Stack
        /// </summary>
        /// <param name="data"></param>
        public void PushByte(byte data)
        {
            OnCPUWriteByte((ushort)(0x0100 | STKP--), data);
        }

        /// <summary>
        /// Pop a word off the Stack
        /// </summary>
        /// <returns></returns>
        public ushort PopWord()
        {
            Span<byte> bytes = [OnCPUReadByte((ushort)(0x0100 | ++STKP)), OnCPUReadByte((ushort)(0x0100 | ++STKP))];
            if (!BitConverter.IsLittleEndian)
            {
                bytes.Reverse();
            }
            return BitConverter.ToUInt16(bytes);
        }
        /// <summary>
        /// Push a word to the Stack
        /// </summary>
        /// <param name="data"></param>
        public void PushWord(ushort data)
        {
            Span<byte> bytes = BitConverter.GetBytes(data);

            if (!BitConverter.IsLittleEndian)
            {
                bytes.Reverse();
            }

            OnCPUWriteByte((ushort)(0x0100 | STKP--), bytes[0]);
            OnCPUWriteByte((ushort)(0x0100 | STKP--), bytes[1]);
        }

        public void PopStatus()
        {
            Status = (Status6502)PopByte();
        }

        public void PushStatus()
        {
            PushByte((byte)Status);
        }

        public void PopPC()
        {
            PC = PopWord();
        }

        public void PushPC()
        {
            PushWord(PC);
        }

        public ushort ReadWord(ushort address)
        {
            Span<byte> bytes = [OnCPUReadByte(address), OnCPUReadByte((ushort)(address + 1))];
            if (!BitConverter.IsLittleEndian)
            {
                bytes.Reverse();
            }
            return BitConverter.ToUInt16(bytes);
        }

        public void WriteWord(ushort address, ushort data)
        {
            Span<byte> bytes = BitConverter.GetBytes(data);
            if (!BitConverter.IsLittleEndian)
            {
                bytes.Reverse();
            }

            OnCPUWriteByte(address, bytes[0]);
            OnCPUWriteByte((ushort)(address + 1), bytes[1]);
        }

        public Cpu6502()
        {
            _unknownInstruction = new UnknownInstruction();
            _instructions = new InstructionSetMOS6502();
            Devices = [];

            A = 0;
            X = 0;
            Y = 0;
            PC = 0xFFFC;
            STKP = 0xFD;
            Status = Status6502.Unused;
        }

        ~Cpu6502()
        {
            UnregisterDevices();
        }

        private bool _interrupted;
        private bool _interruptHandler;
        private bool _NonMaskableInterrupt;

        /// <summary>
        /// executes until specified number of "BRK" command are hit in a row
        /// </summary>
        /// <param name="token">Is used to stop the emulator when required(Used for Asynchronous Tasks).<br/>
        /// If null it when a BRK instruction is hit, it will check the PC - 1 against the address at 0xFFFE.<br/>
        /// If they match then it brakes before the infinite loop can occur.<br/>
        /// passing null IS NOT recommended for running asynchronously unless awaiting</param>

        public ValueTask<ulong> Execute(CancellationToken? token = null)
        {
            ulong totalCycles = 0;
            _interruptHandler = false;
            bool tokenNull = token is null;

            while (!(token?.IsCancellationRequested ?? false))
            {
                byte opcode = ReadByte(PC++);
                var inst = _instructions[opcode];

                if (inst != null)
                {
                    ulong cycles = inst.Execute(this);

                    if (inst!.Name == "BRK" && tokenNull)
                    {
                        if (PC == ReadWord(0xFFFE))
                        {
                            break;
                        }
                    }

                    if ((_interrupted && !Status.HasFlag(Status6502.InterruptDisable) || _NonMaskableInterrupt) && !_interruptHandler)
                    {
                        _interruptHandler = true;
                        cycles += 7;
                        PushPC();
                        PushStatus();
                        SetFlag(Status6502.InterruptDisable, true);
                        PC = ReadWord(_NonMaskableInterrupt ? (ushort)0xFFFA : (ushort)0xFFFE);
                    }
                    totalCycles += cycles;
                }
                else
                {
                    _unknownInstruction.Execute(this);
                    return ValueTask.FromException<ulong>(new InvalidOperationException($"This Instruction({opcode}) is Not Implemented!"));
                }
            }
            return ValueTask.FromResult(totalCycles);
        }

        private ulong _cycles = 0;
        public void Tick(double delta)
        {
            if (_cycles > 0)
            {
                _cycles--;
                return;
            }

            if ((_interrupted && !Status.HasFlag(Status6502.InterruptDisable) || _NonMaskableInterrupt) && !_interruptHandler)
            {
                _interruptHandler = true;
                _cycles += 7;
                PushPC();
                PushStatus();
                SetFlag(Status6502.InterruptDisable, true);
                PC = ReadWord(_NonMaskableInterrupt ? (ushort)0xFFFA : (ushort)0xFFFE);
            }
            else
            {
                var b = ReadByte(PC++);
                var i = _instructions[b] ?? throw new InvalidOperationException($"This Instruction({b}) is Not Implemented!");
                _cycles += i.Execute(this);
            }
        }

        /// <summary>
        /// Check if a device overlaps with another device
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        private (IDevice6502 dev, ushort address)? CheckDeviceOverlap(IDevice6502 n)
        {
            for (ushort addr = 0; addr < ushort.MaxValue; addr++)
            {
                var s = n.CheckDeviceInRange(addr, n.DeviceFlags);
                int devCount = Devices.Count;
                for (int i = 0; i < devCount; i++)
                {
                    var device = Devices[i];

                    if (s && device.CheckDeviceInRange(addr, n.DeviceFlags))
                    {
                        return (device, addr);
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Execute Specific number of instructions (allows and adjusts cycles for interrupts)
        /// </summary>
        /// <param name="steps">how many steps to take</param>
        /// <returns></returns>
        public ulong Step(ushort steps)
        {
            ulong cycles = 0;
            ulong totalCycles = 0;

            for (ushort instruct = 0; instruct < steps; instruct++)
            {
                if ((_interrupted && !Status.HasFlag(Status6502.InterruptDisable) || _NonMaskableInterrupt) && !_interruptHandler)
                {
                    _interruptHandler = true;
                    _cycles += 7;
                    PushPC();
                    PushStatus();
                    SetFlag(Status6502.InterruptDisable, true);
                    PC = ReadWord(_NonMaskableInterrupt ? (ushort)0xFFFA : (ushort)0xFFFE);
                }
                else
                {
                    var b = ReadByte(PC++);
                    var i = _instructions[b] ?? throw new InvalidOperationException($"This Instruction({b}) is Not Implemented!");
                    cycles += i.Execute(this);
                }
                totalCycles += cycles;

                while (cycles > 0)
                {
                    cycles--;
                }
            }
            return totalCycles;
        }

        public ReadOnlySpan<string> DecompileOpcodes(ushort startAddress, ushort opCount, bool showFlags = false)
        {
            if (startAddress + opCount >= ushort.MaxValue)
            {
                throw new ArgumentOutOfRangeException(nameof(opCount), "Arguments will go out of range");
            }

            Span<string> span = new string[(opCount)];
            var sb = new StringBuilder();
            for (ushort ops = 0, address = startAddress; ops < opCount; ops++)
            {
                sb.Clear();
                var i = _instructions[ReadByte(address)];
                if (i is null)
                {
                    _unknownInstruction.Disassemble(sb, this, ref address, showFlags);
                }
                else
                {
                    i.Disassemble(sb, this, ref address, showFlags);
                }
                span[ops] = sb.ToString();
            }

            return span;
        }

        public ReadOnlySpan<string> DecompileAddrRange(ushort startAddress, ushort endAddress = ushort.MaxValue, bool showFlags = false)
        {
            Span<string> span = new string[(endAddress - startAddress) + 1];

            var sb = new StringBuilder();
            for (var address = startAddress; address < endAddress;)
            {
                sb.Clear();
                var i = _instructions[ReadByte(address)];
                if (i is null)
                {
                    _unknownInstruction.Disassemble(sb, this, ref address, showFlags);
                }
                else
                {
                    i.Disassemble(sb, this, ref address, showFlags);
                }

                span[address] = sb.ToString();
            }
            return span;
        }

        public void SetIRQ()
        {
            _interrupted = true;
        }

        public void SetNMI()
        {
            _NonMaskableInterrupt = true;
        }

        public void ClearIRQ()
        {
            _interrupted = false;
            _NonMaskableInterrupt = false;
            _NonMaskableInterrupt = false;
        }

        public void Reset(ushort resetVector = 0xFFFC)
        {
            A = 0;
            X = 0;
            Y = 0;
            PC = ReadWord(resetVector);
            STKP = 0xFD;
            Status = 0 | Status6502.Unused;
        }

        public string GetDebuggerDisplay()
        {
            StringBuilder sb = new();
            sb.Append("CPU Status:");
            sb.Append($"_registers:");
            sb.Append($"A: 0x{A:X2}({A}) ");
            sb.Append($"X: 0x{X:X2}({X}) ");
            sb.Append($"Y: 0x{Y:X2}({Y}) ");
            sb.Append($"P: {Status}({((byte)Status)}) ");
            sb.Append($"STKP: 0x{STKP:X2}({STKP}) ");
            sb.AppendLine($"PC: 0x{PC:X4}({PC}) ");
            return sb.ToString();
        }
    }
}