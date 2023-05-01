

namespace Emulator._6502.Instructions
{
    public sealed class INX : Instruction6502
    {
        public INX() : base("INX", 1, AddrMode6502.Implied, Status6502.Zero | Status6502.Negative)
        {
        }

        public override byte Execute(Cpu6502 cpu)
        {
            cpu.X++;
            cpu.SetFlag(Status6502.Zero, cpu.X == 0x00);
            cpu.SetFlag(Status6502.Negative, (cpu.X & 0x80) > 0);
            return 2;
        }
    }
}

