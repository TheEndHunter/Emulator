

namespace Emulator._6502.Instructions
{
    public sealed class BPL : Instruction6502
    {
        public BPL() : base("BPL", 2, AddrMode6502.Relative, 0)
        {
        }

        public override byte Execute(Cpu6502 cpu)
        {
            byte clocks = 2;
            if (!cpu.GetFlag(Status6502.Negative))
            {
                clocks++;
                cpu.PC = cpu.ReadWord(Relative(ref cpu));
                if ((Relative(ref cpu) & 0xFF00) != (cpu.ReadWord(Relative(ref cpu)) & 0xFF00))
                {
                    clocks++;
                }
            }
            return clocks;
        }
    }
}
