using Emulator._6502.Devices;

using System.Text;

namespace Emulator._6502.CPU
{
    public static class CPU6502_Addressing
    {


    }
    public class Cpu6502
    {
        private readonly Bus6502 m_Bus;
        private Registers6502 m_Registers;
        private readonly InstructionSet6502 m_Instructions;

        public Cpu6502(ref Bus6502 bus)
        {
            m_Bus = bus;
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
        public List<string> Decompile(ushort StartAddress, ushort EndAddress, bool ShowFlags = false)
        {
            List<string> disasembly = new();
            for (ushort address = StartAddress; address < EndAddress; address++)
            {
                StringBuilder sb = new();
                var opcode = m_Instructions[m_Bus.ReadByte(address)];
                address++;
                sb.Append(address);
                sb.Append(": ");
                sb.Append(opcode.Dissassemble(m_Bus, ref address));
                if (ShowFlags && opcode.Flags != Status6502.None)
                    sb.Append($" [Flags]{opcode.Flags}");
                sb.AppendLine();
                disasembly.Add(sb.ToString());
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
