

namespace Emulator._6502.Instructions
{
    public sealed class BEQ : Instruction6502
    {
        public BEQ() : base("BEQ", 2, AddrMode6502.Relative, 0)
        {
        }

        public override byte Execute(Cpu6502 cpu)
        {
            var addr = Relative(ref cpu);
            var jaddr = cpu.ReadWord(addr);
            byte clocks = 2;
            if (cpu.GetFlag(Status6502.Zero))
            {
                clocks++;
                cpu.PC = jaddr;
                if ((addr & 0xFF00) != (jaddr & 0xFF00))
                {
                    clocks++;
                }
            }
            return clocks;
        }
    }
}
