using Emulator._6502.Devices;

namespace Emulator._6502.CPU.Instructions
{
    public sealed class TSX : Instruction6502
    {
        public TSX() : base("TSX", 1, AddrMode6502.Implied, Status6502.Zero | Status6502.Negative)
        {

        }

        public override byte Execute(Registers6502 registers, Bus6502 bus)
        {
            registers.X = registers.STKP;
            registers.SetFlag(Status6502.Zero, registers.X == 0);
            registers.SetFlag(Status6502.Negative, (registers.X & 0x80) > 0);
            return 2;
        }
    }
}
