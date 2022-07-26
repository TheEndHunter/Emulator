using Emulator._6502.Devices;

namespace Emulator._6502.CPU
{
    public static class CPU6502_Addressing
    {


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
