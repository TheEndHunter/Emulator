

namespace Emulator._6502.Instructions
{
    public sealed class SEC : Instruction6502
    {
        public SEC() : base("SEC", 1, AddrMode6502.Implied, Status6502.Carry)
        {

        }

        public override byte Execute(Cpu6502 cpu)
        {
            cpu.SetFlag(Status6502.Carry, true);
            return 2;
        }
    }
}
