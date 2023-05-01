

namespace Emulator._6502.Instructions
{
    public sealed class SEI : Instruction6502
    {
        public SEI() : base("SEI", 1, AddrMode6502.Implied, Status6502.InterruptDisable)
        {

        }

        public override byte Execute(Cpu6502 cpu)
        {
            cpu.SetFlag(Status6502.InterruptDisable, true);
            return 2;
        }
    }
}
