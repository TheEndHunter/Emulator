

namespace Emulator._6502.Instructions
{
    public sealed class SED : Instruction6502
    {
        public SED() : base("SED", 1, AddrMode6502.Implied, Status6502.Decimal)
        {

        }

        public override byte Execute(Cpu6502 cpu)
        {
            cpu.SetFlag(Status6502.Decimal, true);
            return 2;
        }
    }
}
