using Emulator._6502.Devices;

namespace Emulator._6502.CPU.Instructions
{
    public sealed class TAY : Instruction6502
    {
        public TAY() : base("TAY", 1, AddrMode6502.Implied, Status6502.Zero | Status6502.Negative)
        {

        }

        public override byte Execute(Registers6502 registers, Bus6502 bus)
        {
            registers.Y = registers.A;
            registers.SetFlag(Status6502.Zero, registers.Y == 0);
            registers.SetFlag(Status6502.Negative, (registers.Y & 0x80) > 0);
            return 2;
        }
    }
}
