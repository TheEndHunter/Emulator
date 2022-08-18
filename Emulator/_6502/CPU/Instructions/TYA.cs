using Emulator._6502.Devices;

namespace Emulator._6502.CPU.Instructions
{
    public sealed class TYA : Instruction6502
    {
        public TYA() : base("TYA", 1, AddrMode6502.Implied, Status6502.Zero | Status6502.Negative)
        {

        }

        public override byte Execute(ref Registers6502 registers, Bus6502 bus)
        {
            registers.A = registers.Y;
            registers.SetFlag(Status6502.Zero, registers.A == 0);
            registers.SetFlag(Status6502.Negative, (registers.A & 0x80) > 0);
            return 2;
        }
    }
}
