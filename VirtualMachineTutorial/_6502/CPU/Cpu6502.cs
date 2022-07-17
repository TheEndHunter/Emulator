using Emulator._6502.Devices;

namespace Emulator._6502.CPU
{
    public static class CPU6502_Addressing
    {

        /// <summary>
        /// Immediate mode addressing expects the next byte to be a value, so we pass back the value
        /// </summary>
        /// <param name="registers">The Current CPU's Registers</param>
        /// <param name="bus">The Current Bus</param>
        /// <returns>byte directly after instruction in memory</returns>
        public static byte ImmediateMode(Registers6502 registers, Bus6502 bus)
        {
            return bus.ReadByte(registers.PC++);
        }

        /// <summary>
        /// Absolutes mode addressing expects the next word to be an address , so we pass back the address
        /// </summary>
        /// <param name="registers">The Current CPU's Registers</param>
        /// <param name="bus">The Current Bus</param>
        /// <returns>word pointed to by address after instruction in memory</returns>
        public static ushort AbsoluteMode(Registers6502 registers, Bus6502 bus)
        {
            return bus.ReadWord(registers.PC++);
        }

        /// <summary>
        /// Absolutes X mode addressing expects the next word to be an address so we take that and add the content of the X register to it and use that as the new address.
        /// /// if the new value crosses the page boundary then it returns a 1 in clocks as an extra clock cycle is required
        /// </summary>
        /// <param name="registers">The Current CPU's Registers</param>
        /// <param name="bus">The Current Bus</param>
        public static (ushort addr, byte clocks) AbsoluteXMode(Registers6502 registers, Bus6502 bus)
        {
            ushort addr = registers.PC++;
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

        /// <summary>
        /// Absolutes Y mode addressing expects the next word to be an address so we take that and add the content of the Y register to it and use that as the new address.
        /// if the new value crosses the page boundary then it returns a 1 in clocks as an extra clock cycle is required
        /// </summary>
        /// <param name="registers">The Current CPU's Registers</param>
        /// <param name="bus">The Current Bus</param>
        public static (ushort addr, byte clocks) AbsoluteYMode(Registers6502 registers, Bus6502 bus)
        {
            ushort addr = registers.PC++;
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

        /// <summary>
        /// Indirect mode addressing expects the word after the instruction to be an address to the address of the value needed, so we return the real actual value address.
        /// this particular mode suffers from a bug in the original hardware that causes a page wrap around as the hi byte is not properly used in this mode.
        /// for compatibility, by default this function will behave as the original CPU does, but has a flag to allow "corrected" behavior
        /// </summary>
        /// <param name="registers">The Current CPU's Registers</param>
        /// <param name="bus">The Current Bus</param>
        /// <param name="useFixedBehavior">[Optional] allows function to used fixed, non-original instruction mode.(Default = false)</param>
        public static ushort IndirectMode(Registers6502 registers, Bus6502 bus, bool useFixedBehavior = false)
        {
            ushort addr = bus.ReadWord(registers.PC++);

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
        /// Indirect X mode addressing expects the byte after the instruction to be an index into zero page, which is then offset by X and used to get an address
        /// </summary>
        /// <param name="registers">The Current CPU's Registers</param>
        /// <param name="bus">The Current Bus</param>
        public static ushort IndirectXMode(Registers6502 registers, Bus6502 bus)
        {
            return bus.ReadWord((byte)(bus.ReadByte(registers.PC++) + registers.X));
        }
        /// <summary>
        /// Indirect Y mode addressing expects the byte after the instruction to be an index into zero page, which is then used to get an address
        /// and then the content of Y is added as an offset, if this causes a Page Change then 1 extra clock cycle is needed
        /// </summary>
        /// <param name="registers">The Current CPU's Registers</param>
        /// <param name="bus">The Current Bus</param>
        public static (ushort addr, byte clocks) IndirectYMode(Registers6502 registers, Bus6502 bus)
        {
            ushort addr = registers.PC++;
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

        /// <summary>
        /// hi byte constant for zero-page mode addressing.
        /// </summary>
        public const byte hi = 0x00;

        /// <summary>
        /// Addressing mode which expects the next byte to be an offset into the zero page memory so we return it as an address within the zeropage
        /// </summary>
        /// <param name="registers">The Current CPU's Registers</param>
        /// <param name="bus">The Current Bus</param>
        /// <returns>address pointed to in zero page memory</returns>
        public static ushort ZeroPageMode(Registers6502 registers, Bus6502 bus)
        {
            var lo = bus.ReadByte(registers.PC++);

            ushort address;
            if (BitConverter.IsLittleEndian)
            {
                address = BitConverter.ToUInt16(new[] { lo, hi }, 0);
            }
            else
            {
                address = BitConverter.ToUInt16(new[] { hi, lo }, 0);
            }
            return address;
        }

        /// <summary>
        /// Addressing mode which expects the next byte to be an offset into the zero page memory then we add the content of the X register as an offset and return the new address
        /// </summary>
        /// <param name="registers">The Current CPU's Registers</param>
        /// <param name="bus">The Current Bus</param>
        /// <returns>address pointed to in zero page memory</returns>
        public static ushort ZeroPageXMode(Registers6502 registers, Bus6502 bus)
        {
            var lo = (byte)(bus.ReadByte(registers.PC++) + registers.X);

            ushort address;
            if (BitConverter.IsLittleEndian)
            {
                address = BitConverter.ToUInt16(new[] { lo, hi }, 0);
            }
            else
            {
                address = BitConverter.ToUInt16(new[] { hi, lo }, 0);
            }
            return address;
        }

        /// <summary>
        /// Addressing mode which expects the next byte to be an offset into the zero page memory then we add the content of the Y register as an offset and return the new address
        /// </summary>
        /// <param name="registers">The Current CPU's Registers</param>
        /// <param name="bus">The Current Bus</param>
        /// <returns>address pointed to in zero page memory</returns>
        public static ushort ZeroPageYMode(Registers6502 registers, Bus6502 bus)
        {
            var lo = (byte)(bus.ReadByte(registers.PC++) + registers.Y);

            ushort address;
            if (BitConverter.IsLittleEndian)
            {
                address = BitConverter.ToUInt16(new[] { lo, hi }, 0);
            }
            else
            {
                address = BitConverter.ToUInt16(new[] { hi, lo }, 0);
            }
            return address;
        }

        /// <summary>
        /// Addressing mode which is used exclusively in branch instructions, it expect the next byte to be an offset to from the current PC to a new address to jump to. so we return an address representing the new address 
        /// </summary>
        /// <param name="registers">The Current CPU's Registers</param>
        /// <param name="bus">The Current Bus</param>
        /// <returns>address pointed to in zero page memory</returns>
        public static ushort RelativeMode(Registers6502 registers, Bus6502 bus)
        {
            ushort loc = bus.ReadByte(registers.PC++);

            if ((loc & 0x80) == 1)
            {
                loc |= 0xFF00;
            }

            return loc;
        }
    }
    public class Cpu6502
    {
        private readonly Bus6502 m_Bus;
        private readonly bool m_UseFixedIndirectModeBehavior;
        private Registers6502 m_Registers;
        private readonly InstructionSet6502 m_Instructions;

        public Cpu6502(ref Bus6502 bus, bool useFixedIndirectModeBehavior = false)
        {
            m_Bus = bus;
            m_UseFixedIndirectModeBehavior = useFixedIndirectModeBehavior;
            m_Registers = new Registers6502();
            m_Instructions = new InstructionSet6502();
        }

        private ulong cycles = 0;
        public void Clock()
        {
            if (cycles == 0)
            {
                cycles += m_Instructions[m_Bus.ReadByte(m_Registers.PC++)].Execute(m_Registers, m_Bus);
            }
            cycles--;
        }

        public void Step(byte steps)
        {
            byte instruct = 0;
            while (instruct < steps)
            {
                if (cycles == 0)
                {
                    cycles += m_Instructions[m_Bus.ReadByte(m_Registers.PC++)].Execute(m_Registers, m_Bus);
                    instruct++;
                }
                cycles--;
            }
        }

        private static string ToHex(ushort address)
        {
            return address.ToString("0xX4");
        }
        private static string ToHex(byte data)
        {
            return data.ToString("0xX2");
        }
        public List<string> Dissasemble(ushort StartAddress, ushort EndAddress, bool ShowFlags = false)
        {
            List<string> disasembly = new();
            for (ushort address = StartAddress; address < EndAddress; address++)
            {
                var opcode = m_Bus.ReadByte(address);
                var instruction = m_Instructions[opcode];
                string code = $"{ToHex(address)}: {instruction.Name}({ToHex(opcode)})";

                bool hasData = true;
                ushort data = 0x0000;
                string reg = "";
                switch (instruction.AddressMode)
                {
                    case AddrMode6502.Absolute:
                    case AddrMode6502.IndexedIndirect:
                    case AddrMode6502.AbsoluteX:
                        {
                            data = m_Bus.ReadWord(++address);
                            reg = "X";
                            address++;
                            break;
                        }
                    case AddrMode6502.AbsoluteY:
                    case AddrMode6502.IndirectIndexed:
                        {
                            data = m_Bus.ReadWord(++address);
                            reg = "Y";
                            address++;
                            break;
                        }
                    case AddrMode6502.Immediate:
                    case AddrMode6502.Indirect:
                        {
                            data = m_Bus.ReadWord(++address);
                            address++;
                            break;
                        }
                    case AddrMode6502.Relative:
                    case AddrMode6502.ZeroPage:
                        {
                            data = m_Bus.ReadByte(++address);
                            break;
                        }
                    case AddrMode6502.ZeroPageX:
                        {
                            data = m_Bus.ReadByte(++address);
                            reg = "X";
                            break;
                        }
                    case AddrMode6502.ZeroPageY:
                        {
                            data = m_Bus.ReadByte(++address);
                            reg = "Y";
                            break;
                        }
                    default:
                        {
                            hasData = false;
                            break;
                        }
                }
                code += $"{(hasData ? $",{ToHex(data)} [{instruction.AddressMode}]" : "")}{(reg.Length > 0 ? $",{reg}" : "")}{(ShowFlags ? "" : $", [{instruction.Flags}]")}";
                disasembly.Add(code);
            }
            return disasembly;
        }

        public void IRQ()
        {
            // WIP
        }
        public void NMI()
        {
            // WIP
        }
        public void Reset()
        {
            // WIP
        }
    }
}
