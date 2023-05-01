

namespace Emulator._6502.Instructions
{
    public sealed class TXS : Instruction6502
    {
        public TXS() : base("TXS", 1, AddrMode6502.Implied, 0)
        {

        }

        public override byte Execute(Cpu6502 cpu)
        {
            cpu.STKP = cpu.X;
            return 2;
        }
    }
}
