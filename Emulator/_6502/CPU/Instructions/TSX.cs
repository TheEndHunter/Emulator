using Emulator._6502.Devices;

namespace Emulator._6502.CPU.Instructions
{
    public sealed class TSX : Instruction6502
    {
        public TSX() : base("TSX", AddrMode6502.Implied, Status6502.Zero | Status6502.Negative)
        {

        }

        public override byte Execute(Registers6502 registers, Bus6502 bus)
        {
            return 0;
        }
    }
}
