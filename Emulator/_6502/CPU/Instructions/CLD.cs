using Emulator._6502.Devices;

namespace Emulator._6502.CPU.Instructions
{
    public sealed class CLD : Instruction6502
    {
        public CLD() : base("CLD", 1, AddrMode6502.Implied, Status6502.Decimal)
        {
        }

        public override byte Execute(Registers6502 registers, Bus6502 bus)
        {
            registers.SetFlag(Status6502.Decimal, false);
            return 2;
        }
    }
}
