

namespace Emulator._6502.Instructions
{
    public sealed class CLD : Instruction6502
    {
        public CLD() : base("CLD", 1, AddrMode6502.Implied, Status6502.Decimal)
        {
        }

        public override byte Execute(Cpu6502 cpu)
        {
            cpu.SetFlag(Status6502.Decimal, false);
            return 2;
        }
    }
}
