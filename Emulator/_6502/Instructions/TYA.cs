

namespace Emulator._6502.Instructions
{
    public sealed class TYA : Instruction6502
    {
        public TYA() : base("TYA", 1, AddrMode6502.Implied, Status6502.Zero | Status6502.Negative)
        {

        }

        public override byte Execute(Cpu6502 cpu)
        {
            cpu.A = cpu.Y;
            cpu.SetFlag(Status6502.Zero, cpu.A == 0);
            cpu.SetFlag(Status6502.Negative, (cpu.A & 0x80) > 0);
            return 2;
        }
    }
}
