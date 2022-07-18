using Emulator._6502.Devices;

namespace Emulator._6502.CPU.Instructions
{
    public sealed class TAX : Instruction6502
    {
        public TAX() : base("TAX", AddrMode6502.Implied, Status6502.Zero | Status6502.Negative)
        {

        }

        public override byte Execute(Registers6502 registers, Bus6502 bus)
        {
            return 0;
        }
    }
}
