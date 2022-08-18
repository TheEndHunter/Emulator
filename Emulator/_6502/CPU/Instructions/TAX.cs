using Emulator._6502.Devices;

namespace Emulator._6502.CPU.Instructions
{
    public sealed class TAX : Instruction6502
    {
        public TAX() : base("TAX", 1, AddrMode6502.Implied, Status6502.Zero | Status6502.Negative)
        {

        }

        public override byte Execute(ref Registers6502 registers, Bus6502 bus)
        {
            registers.X = registers.A;
            registers.SetFlag(Status6502.Zero, registers.X == 0);
            registers.SetFlag(Status6502.Negative, (registers.X & 0x80) > 0);
            return 2;
        }
    }
}
