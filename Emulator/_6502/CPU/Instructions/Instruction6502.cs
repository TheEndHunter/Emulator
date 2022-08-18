using Emulator._6502.Devices;

using System.Diagnostics;
using System.Text;

namespace Emulator._6502.CPU.Instructions
{
    /// <summary>
    /// 6502 instruction
    /// </summary>
    [DebuggerDisplay($"{{{nameof(GetDebuggerDisplay)}(),nq}}")]
    public abstract class Instruction6502
    {
        /// <summary>
        /// Default Constructor
        /// </summary>
        /// <param name="name"></param>
        /// <param name="addressMode"></param>
        /// <param name="flags"></param>
        protected Instruction6502(string name, byte bytesUsed, AddrMode6502 addressMode, Status6502 flags)
        {
            Name = name;
            AddressMode = addressMode;
            Flags = flags;
            BytesUsed = bytesUsed;
        }
        public string Name { get; }
        public AddrMode6502 AddressMode { get; }
        public Status6502 Flags { get; }
        public byte BytesUsed { get; }
        public abstract byte Execute(ref Registers6502 registers, Bus6502 bus);

        public string GetDebuggerDisplay()
        {
            return $"{Name}[A:{AddressMode}][F:{Flags}]";
        }

        public string Dissassemble(Bus6502 bus, ref ushort addr)
        {
            StringBuilder sb = new();
            sb.Append(Name);
            sb.Append(' ');
            if (BytesUsed == 2)
            {
                sb.Append(AddSymbols(AddressMode, bus.ReadByte(addr)));
                addr++;
            }
            else if (BytesUsed == 3)
            {
                sb.Append(AddSymbols(AddressMode, bus.ReadWord(addr)));
                addr += 2;
            }
            else if (BytesUsed > 3)
                throw new InvalidCastException("The specified number of bytes used by this instruction is invalid");
            return sb.ToString();
        }

        protected static string AddSymbols(AddrMode6502 mode, ushort data)
        {
            return mode switch
            {
                AddrMode6502.Immediate => $"#${data:X2}",
                AddrMode6502.Indirect => $"(${data:X2})",
                AddrMode6502.IndexedIndirect => $"(${data:X2},X)",
                AddrMode6502.IndirectIndexed => $"(${data:X2}),Y",
                AddrMode6502.ZeroPage => $"${data:X2}",
                AddrMode6502.ZeroPageX => $"${data:X2},X",
                AddrMode6502.ZeroPageY => $"${data:X2},Y",
                AddrMode6502.Relative => $"${data:X2}",
                AddrMode6502.Accumulator => "A",
                AddrMode6502.Absolute => $"${data:X4}",
                AddrMode6502.AbsoluteX => $"${data:X4},X",
                AddrMode6502.AbsoluteY => $"${data:X4},Y",
                _ => string.Empty,
            };
        }

        protected static string AddSymbols(AddrMode6502 mode, byte data)
        {
            return mode switch
            {
                AddrMode6502.Immediate => $"#${data:X2}",
                AddrMode6502.Indirect => $"(${data:X2})",
                AddrMode6502.IndexedIndirect => $"(${data:X2},X)",
                AddrMode6502.IndirectIndexed => $"(${data:X2}),Y",
                AddrMode6502.ZeroPage => $"${data:X2}",
                AddrMode6502.ZeroPageX => $"${data:X2},X",
                AddrMode6502.ZeroPageY => $"${data:X2},Y",
                AddrMode6502.Relative => $"${data:X2}",
                AddrMode6502.Accumulator => "A",
                AddrMode6502.Absolute => $"${data:X4}",
                AddrMode6502.AbsoluteX => $"${data:X4},X",
                AddrMode6502.AbsoluteY => $"${data:X4},Y",
                _ => string.Empty,
            };
        }

        /// <summary>
        /// hi byte for some addressing modes
        /// </summary>
        private const byte hi = 0;
        protected static ushort ZeroPage(ref Registers6502 registers, Bus6502 bus)
        {
            var lo = bus.ReadByte(registers.PC++);
            if (BitConverter.IsLittleEndian)
            {
                return BitConverter.ToUInt16(new[] { lo, hi }, 0);
            }
            else
            {
                return BitConverter.ToUInt16(new[] { hi, lo }, 0);
            }
        }
        protected static ushort Immediate(ref Registers6502 registers, Bus6502 bus)
        {
            return registers.PC++;
        }
        protected static ushort ZeroPageX(ref Registers6502 registers, Bus6502 bus)
        {
            byte lo = (byte)(bus.ReadByte(registers.PC++) + registers.X);
            if (BitConverter.IsLittleEndian)
            {
                return BitConverter.ToUInt16(new[] { lo, hi }, 0);
            }
            else
            {
                return BitConverter.ToUInt16(new[] { hi, lo }, 0);
            }
        }
        protected static ushort ZeroPageY(ref Registers6502 registers, Bus6502 bus)
        {
            byte lo = (byte)(bus.ReadByte(registers.PC++) + registers.Y);
            if (BitConverter.IsLittleEndian)
            {
                return BitConverter.ToUInt16(new[] { lo, hi }, 0);
            }
            else
            {
                return BitConverter.ToUInt16(new[] { hi, lo }, 0);
            }
        }
        protected static ushort Absolute(ref Registers6502 registers, Bus6502 bus)
        {
            var ret = bus.ReadWord(registers.PC);
            registers.PC += 2;
            return ret;
        }
        public static (ushort addr, byte clocks) AbsoluteX(ref Registers6502 registers, Bus6502 bus)
        {
            ushort addr = registers.PC;
            registers.PC += 2;
            byte hiAddrbyte = BitConverter.IsLittleEndian ? bus.ReadByte((ushort)(addr + 1)) : bus.ReadByte(addr);
            ushort val = (ushort)(bus.ReadWord(addr) + registers.X);
            if ((val & 0xFF00) != hiAddrbyte << 8)
            {
                return (val, 1);
            }
            else
            {
                return (val, 0);
            }
        }
        public static (ushort addr, byte clocks) AbsoluteY(ref Registers6502 registers, Bus6502 bus)
        {
            ushort addr = registers.PC;
            registers.PC += 2;
            byte hiAddrbyte = BitConverter.IsLittleEndian ? bus.ReadByte((ushort)(addr + 1)) : bus.ReadByte(addr);
            ushort val = (ushort)(bus.ReadWord(addr) + registers.Y);
            if ((val & 0xFF00) != hiAddrbyte << 8)
            {
                return (val, 1);
            }
            else
            {
                return (val, 0);
            }
        }
        public static ushort Indirect(ref Registers6502 registers, Bus6502 bus, bool useFixedBehavior = false)
        {
            ushort addr = bus.ReadWord(registers.PC);
            registers.PC += 2;

            if (addr == 0x00FF && !useFixedBehavior)
            {
                byte tHi = bus.ReadByte((ushort)(addr & 0xFF00));
                byte tLo = bus.ReadByte(addr);

                if (BitConverter.IsLittleEndian)
                {
                    return BitConverter.ToUInt16(new[] { tLo, tHi }, 0);
                }
                else
                {
                    return BitConverter.ToUInt16(new[] { tHi, tLo }, 0);
                }

            }
            else
            {
                return bus.ReadWord(addr);
            }
        }

        /// <summary>
        /// X  indirect indexed
        /// </summary>
        /// <param name="registers">The registers.</param>
        /// <param name="bus">The bus.</param>
        /// <returns></returns>
        public static ushort IndirectIndex(ref Registers6502 registers, Bus6502 bus)
        {
            return bus.ReadWord((byte)(bus.ReadByte(registers.PC++) + registers.X));
        }
        /// <summary>
        /// Y indexed indirect 
        /// </summary>
        /// <param name="registers">The registers.</param>
        /// <param name="bus">The bus.</param>
        /// <returns></returns>
        public static (ushort addr, byte clocks) IndexIndirect(ref Registers6502 registers, Bus6502 bus)
        {
            ushort addr = registers.PC;
            registers.PC += 2;
            byte hiAddrbyte = BitConverter.IsLittleEndian ? bus.ReadByte((ushort)(addr + 1 & 0x00FF)) : bus.ReadByte((ushort)(addr & 0x00FF));
            ushort val = (ushort)(bus.ReadWord(addr) + registers.Y);
            if ((val & 0xFF00) != hiAddrbyte << 8)
            {
                return (val, 1);
            }
            else
            {
                return (val, 0);
            }
        }
        public static ushort Relative(ref Registers6502 registers, Bus6502 bus)
        {
            ushort loc = bus.ReadByte(registers.PC++);

            if ((loc & 0x80) == 1)
            {
                loc |= 0xFF00;
            }

            return loc;
        }
    }
}
