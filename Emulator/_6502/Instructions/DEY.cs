

namespace Emulator._6502.Instructions
{
    public sealed class DEY : Instruction6502
    {
        public DEY() : base("DEY", 1, AddrMode6502.Implied, Status6502.Zero | Status6502.Negative)
        {
        }

        public override byte Execute(Cpu6502 cpu)
        {
            cpu.Y--;
            cpu.SetFlag(Status6502.Zero, cpu.Y == 0x00);
            cpu.SetFlag(Status6502.Negative, (cpu.Y & 0x80) > 0);
            return 2;
        }
    }
}

