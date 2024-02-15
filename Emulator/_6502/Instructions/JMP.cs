

namespace Emulator._6502.Instructions
{
    public abstract class JMP(byte bytesUsed, AddrMode6502 mode) : Instruction6502("JMP", bytesUsed, mode, 0)
    {
    }

    public sealed class JMP_Absolute : JMP
    {
        public JMP_Absolute() : base(3, AddrMode6502.Absolute)
        {
        }

        public override byte Execute(Cpu6502 cpu)
        {
            cpu.PC = Absolute(ref cpu);
            return 3;
        }
    }

    public sealed class JMP_Indirect : JMP
    {
        public JMP_Indirect() : base(3, AddrMode6502.Indirect)
        {
        }

        public override byte Execute(Cpu6502 cpu)
        {
            cpu.PC = Indirect(ref cpu);
            return 5;
        }
    }
}

