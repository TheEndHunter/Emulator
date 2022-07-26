using Emulator._6502.Devices;

using System.Diagnostics;

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
        protected Instruction6502(string name, AddrMode6502 addressMode, Status6502 flags)
        {
            Name = name;
            AddressMode = addressMode;
            Flags = flags;
        }
        public string Name { get; }
        public AddrMode6502 AddressMode { get; }
        public Status6502 Flags { get; }
        public abstract byte Execute(Registers6502 registers, Bus6502 bus);

        public string GetDebuggerDisplay()
        {
            return $"{Name}[A:{AddressMode}][F:{Flags}]";
        }

        /// <summary>
        /// hi byte for some addressing modes
        /// </summary>
        private const byte hi = 0;
        protected static ushort ZeroPage(Registers6502 registers, Bus6502 bus)
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
        protected static ushort Immediate(Registers6502 registers, Bus6502 bus)
        {
            return registers.PC++;
        }
        protected static ushort ZeroPageX(Registers6502 registers, Bus6502 bus)
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
        protected static ushort ZeroPageY(Registers6502 registers, Bus6502 bus)
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
        protected static ushort Absolute(Registers6502 registers, Bus6502 bus)
        {
            var ret = bus.ReadWord(registers.PC);
            registers.PC += 2;
            return ret;
        }
        public static (ushort addr, byte clocks) AbsoluteX(Registers6502 registers, Bus6502 bus)
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
        public static (ushort addr, byte clocks) AbsoluteY(Registers6502 registers, Bus6502 bus)
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
        public static ushort Indirect(Registers6502 registers, Bus6502 bus, bool useFixedBehavior = false)
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
        public static ushort IndirectIndex(Registers6502 registers, Bus6502 bus)
        {
            return bus.ReadWord((byte)(bus.ReadByte(registers.PC++) + registers.X));
        }
        /// <summary>
        /// Y indexed indirect 
        /// </summary>
        /// <param name="registers">The registers.</param>
        /// <param name="bus">The bus.</param>
        /// <returns></returns>
        public static (ushort addr, byte clocks) IndexIndirect(Registers6502 registers, Bus6502 bus)
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
        public static ushort Relative(Registers6502 registers, Bus6502 bus)
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
