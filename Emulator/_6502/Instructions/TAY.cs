

namespace Emulator._6502.Instructions
{
    public sealed class TAY : Instruction6502
    {
        public TAY() : base("TAY", 1, AddrMode6502.Implied, Status6502.Zero | Status6502.Negative)
        {

        }

        public override byte Execute(Cpu6502 cpu)
        {

            cpu.Y = cpu.A;
            cpu.SetFlag(Status6502.Zero, cpu.Y == 0);
            cpu.SetFlag(Status6502.Negative, (cpu.Y & 0x80) > 0);
            return 2;
        }
    }
}
