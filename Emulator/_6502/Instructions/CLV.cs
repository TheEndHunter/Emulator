

namespace Emulator._6502.Instructions
{
    public sealed class CLV : Instruction6502
    {
        public CLV() : base("CLV", 1, AddrMode6502.Implied, Status6502.OverFlow)
        {
        }

        public override byte Execute(Cpu6502 cpu)
        {
            cpu.SetFlag(Status6502.OverFlow, false);
            return 2;
        }
    }
}
