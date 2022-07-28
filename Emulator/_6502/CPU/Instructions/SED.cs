using Emulator._6502.Devices;

namespace Emulator._6502.CPU.Instructions
{
    public sealed class SED : Instruction6502
    {
        public SED() : base("SED", 1, AddrMode6502.Implied, Status6502.Decimal)
        {

        }

        public override byte Execute(Registers6502 registers, Bus6502 bus)
        {
            registers.SetFlag(Status6502.Decimal, true);
            return 2;
        }
    }
}
