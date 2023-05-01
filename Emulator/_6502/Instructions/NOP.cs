

namespace Emulator._6502.Instructions
{
    public sealed class NOP : Instruction6502
    {
        public NOP() : base("NOP", 1, AddrMode6502.Implied, 0)
        {
        }

        public override byte Execute(Cpu6502 cpu)
        {
            return 2;
        }
    }
}
