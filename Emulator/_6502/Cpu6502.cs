using System.Buffers.Binary;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Xml.Linq;

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
        public bool GetFlag(Status6502 f) => (Status & f) != 0;

        private readonly InstructionSet6502 m_Instructions;

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
#if DEBUG
                var sdev = s.Value.dev;
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
            byte _0 = OnCPUReadByte((ushort)(0x0100 | ++STKP));
            byte _1 = OnCPUReadByte((ushort)(0x0100 | ++STKP));

            if (BitConverter.IsLittleEndian)
            {
                return BitConverter.ToUInt16(new byte[] { _0, _1 });
            }
            else
            {
                return BitConverter.ToUInt16(new byte[] { _1, _0 });
            }
        }
        /// <summary>
        /// Push a word to the Stack
        /// </summary>
        /// <param name="data"></param>
        public void PushWord(ushort data)
        {
            byte[] b = BitConverter.GetBytes(data);
            if (BitConverter.IsLittleEndian)
            {
                OnCPUWriteByte((ushort)(0x0100 | STKP--), b[1]);
                OnCPUWriteByte((ushort)(0x0100 | STKP--), b[0]);
            }
            else
            {
                OnCPUWriteByte((ushort)(0x0100 | STKP--), b[0]);
                OnCPUWriteByte((ushort)(0x0100 | STKP--), b[1]);
            }
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
            byte lo = OnCPUReadByte(address);
            byte hi = OnCPUReadByte((ushort)(address + 1));

            if (BitConverter.IsLittleEndian)
            {
                return BitConverter.ToUInt16(new byte[] { lo, hi }, 0);
            }
            else
            {
                return BitConverter.ToUInt16(new byte[] { hi, lo }, 0);
            }

        }

        public void WriteWord(ushort address, ushort data)
        {
            byte[] b = BitConverter.GetBytes(data);
            if (BitConverter.IsLittleEndian)
            {
                OnCPUWriteByte(address, b[0]);
                OnCPUWriteByte((ushort)(address + 1), b[1]);
            }
            else
            {
                OnCPUWriteByte(address, b[1]);
                OnCPUWriteByte((ushort)(address + 1), b[0]);
            }
        }

        public Cpu6502()
        {
            m_Instructions = new InstructionSet6502();
            Devices = new List<IDevice6502>();

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
                var inst = m_Instructions[ReadByte(PC++)];
                ulong cycles = inst.Execute(this);

                if (inst.Name == "BRK" && tokenNull)
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

            return ValueTask.FromResult(totalCycles);
        }

        private (IDevice6502 dev, ushort address)? CheckDeviceOverlap(IDevice6502 n)
        {
            for (ushort addr = 0; addr < ushort.MaxValue; addr++)
            {
                var s = n.CheckDeviceInRange(addr, n.DeviceFlags);
                foreach (var device in Devices)
                {
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
                var b = ReadByte(PC++);
                cycles += m_Instructions[b].Execute(this);

                if ((_interrupted && !Status.HasFlag(Status6502.InterruptDisable) || _NonMaskableInterrupt) && !_interruptHandler)
                {
                    _interruptHandler = true;
                    cycles += 7;
                    PushPC();
                    PushStatus();
                    SetFlag(Status6502.InterruptDisable, true);
                    PC = ReadWord(_NonMaskableInterrupt ? (ushort)0xFFFA : (ushort)0xFFFE);
                    instruct++;
                }
                totalCycles += cycles;

                while (cycles > 0)
                {
                    cycles--;
                }
            }
            return totalCycles;
        }

        public List<string> DecompileOpcodes(ushort startAddress, ushort opCount, bool showFlags = false)
        {
            if (startAddress + opCount >= ushort.MaxValue)
            {
                throw new ArgumentOutOfRangeException(nameof(opCount), "Arguments will go out of range");
            }

            List<string> disassembly = new();
            for (ushort ops = 0, address = startAddress; ops < opCount; ops++)
            {
                var sb = new StringBuilder();
                m_Instructions[ReadByte(address)].Disassemble(sb, this, ref address, showFlags);
                disassembly.Add(sb.ToString());
            }

            return disassembly;
        }

        public List<string> DecompileAddrRange(ushort startAddress, ushort endAddress = ushort.MaxValue, bool showFlags = false)
        {
            List<string> disassembly = new();

            for (var address = startAddress; address < endAddress;)
            {
                var sb = new StringBuilder();
                m_Instructions[ReadByte(address)].Disassemble(sb, this, ref address, showFlags);
                disassembly.Add(sb.ToString());
            }
            return disassembly;
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
            sb.Append($"Registers:");
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