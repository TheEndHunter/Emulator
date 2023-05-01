

namespace Emulator._6502.Instructions
{
    public sealed class TSX : Instruction6502
    {
        public TSX() : base("TSX", 1, AddrMode6502.Implied, Status6502.Zero | Status6502.Negative)
        {

        }

        public override byte Execute(Cpu6502 cpu)
        {

            cpu.X = cpu.STKP;
            cpu.SetFlag(Status6502.Zero, cpu.X == 0);
            cpu.SetFlag(Status6502.Negative, (cpu.X & 0x80) > 0);
            return 2;
        }
    }
}
