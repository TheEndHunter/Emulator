

namespace Emulator._6502.Instructions
{
    public sealed class PHA : Instruction6502
    {
        public PHA() : base("PHA", 1, AddrMode6502.Implied, 0)
        {

        }

        public override byte Execute(Cpu6502 cpu)
        {
            cpu.WriteByte((ushort)(0x0100 + cpu.STKP), cpu.A);
            cpu.STKP--;
            return 3;
        }
    }
}
