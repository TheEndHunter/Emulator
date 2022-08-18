using Emulator._6502.Devices;

using System.Diagnostics;
using System.Text;

namespace Emulator._6502.CPU
{
    [DebuggerDisplay($"{{{nameof(GetDebuggerDisplay)}(),nq}}")]
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
            Reset();
        }

        private ulong cycles = 0;
        /// <summary>
        /// executes until specified number of "BRK" command are hit in a row
        /// </summary>
        /// <param name="breaksBeforeStop">how many breaks in a row before stopping</param>
        public void Execute(byte breaksBeforeStop = 2)
        {
            if (breaksBeforeStop < 2)
                breaksBeforeStop = 2;

            byte brk = 0;
            while (brk < breaksBeforeStop)
            {
                if (cycles == 0)
                {
                    var op = m_Instructions[m_Bus.ReadByte(m_Registers.PC++)];
                    if (op.Name == "BRK")
                    {
                        brk++;
                    }
                    else if (brk > 0)
                    {
                        brk = 0;
                    }
                    cycles += op.Execute(ref m_Registers, m_Bus);
                }
                cycles--;
            }

        }

        public void Step(byte steps)
        {
            byte instruct = 0;
            while (instruct < steps)
            {
                if (cycles == 0)
                {
                    cycles += m_Instructions[m_Bus.ReadByte(m_Registers.PC++)].Execute(ref m_Registers, m_Bus);
                    instruct++;
                }
                cycles--;
            }
        }
        public List<string> DecompileOpcodes(ushort StartAddress, ushort opCount, bool ShowFlags = false)
        {
            if (StartAddress + opCount >= ushort.MaxValue)
            {
                throw new ArgumentOutOfRangeException(nameof(opCount), "Arguments will go out of range");
            }
            List<string> disasembly = new();
            ushort address = StartAddress;
            ushort ops = 0;
            while (ops != opCount)
            {
                var addr = address;
                StringBuilder sb = new();
                var opcode = m_Instructions[m_Bus.ReadByte(addr)];
                addr++;
                sb.Append($"0x{address.ToString("X4")}");
                sb.Append(": ");
                sb.Append(opcode.Dissassemble(m_Bus, ref addr));
                if (ShowFlags && opcode.Flags != Status6502.None)
                    sb.Append($" [Flags]{opcode.Flags}");
                sb.AppendLine();
                disasembly.Add(sb.ToString());
                address = addr;
                ops++;
            }
            return disasembly;
        }
        public List<string> DecompileAddrRange(ushort StartAddress, ushort EndAddress, bool ShowFlags = false)
        {
            List<string> disasembly = new();
            for (ushort address = StartAddress; address < EndAddress;)
            {
                var addr = address;
                StringBuilder sb = new();
                var opcode = m_Instructions[m_Bus.ReadByte(addr)];
                addr++;
                sb.Append($"0x{address.ToString("X4")}");
                sb.Append(": ");
                sb.Append(opcode.Dissassemble(m_Bus, ref addr));
                if (ShowFlags && opcode.Flags != Status6502.None)
                    sb.Append($" [Flags]{opcode.Flags}");
                sb.AppendLine();
                disasembly.Add(sb.ToString());
                address = addr;
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
            m_Registers.A = 0;
            m_Registers.X = 0;
            m_Registers.Y = 0;
            m_Registers.PC = m_Bus.ReadWord(m_Bus.ReadWord(0xFFFC));
            m_Registers.STKP = 0xFD;
            m_Registers.Status = Status6502.None | Status6502.Unused;
        }

        public string GetDebuggerDisplay()
        {
            StringBuilder sb = new();
            sb.AppendLine("CPU Status:");
            sb.AppendLine($"Registers:");
            sb.AppendLine($"{m_Registers.GetDebuggerDisplay()}");
            sb.AppendLine("Devices:");
            sb.AppendLine($"{m_Bus.GetDebuggerDisplay()}");
            return sb.ToString();
        }
    }
}
