

namespace Emulator._6502.Instructions
{
    public sealed class CLI : Instruction6502
    {
        public CLI() : base("CLI", 1, AddrMode6502.Implied, Status6502.InterruptDisable)
        {
        }

        public override byte Execute(Cpu6502 cpu)
        {
            cpu.SetFlag(Status6502.InterruptDisable, false);
            return 2;
        }
    }
}
