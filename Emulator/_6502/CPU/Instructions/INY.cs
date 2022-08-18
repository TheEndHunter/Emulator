using Emulator._6502.Devices;

namespace Emulator._6502.CPU.Instructions
{
    public sealed class INY : Instruction6502
    {
        public INY() : base("INY", 1, AddrMode6502.Implied, Status6502.Zero | Status6502.Negative)
        {
        }

        public override byte Execute(ref Registers6502 registers, Bus6502 bus)
        {
            registers.Y++;
            registers.SetFlag(Status6502.Zero, registers.Y == 0x00);
            registers.SetFlag(Status6502.Negative, (registers.Y & 0x80) > 0);
            return 2;
        }
    }
}

