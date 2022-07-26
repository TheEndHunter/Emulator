using Emulator._6502.Devices;

namespace Emulator._6502.CPU.Instructions
{
    public sealed class DEX : Instruction6502
    {
        public DEX() : base("DEX", AddrMode6502.Implied, Status6502.Zero | Status6502.Negative)
        {
        }
        public override byte Execute(Registers6502 registers, Bus6502 bus)
        {
            registers.X--;
            registers.SetFlag(Status6502.Zero, registers.X == 0x00);
            registers.SetFlag(Status6502.Negative, (registers.X & 0x80) > 0);
            return 2;
        }
    }
}

