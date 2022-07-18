using Emulator._6502.Devices;

namespace Emulator._6502.CPU.Instructions
{
    public sealed class SEI : Instruction6502
    {
        public SEI() : base("SEI", AddrMode6502.Implied, Status6502.InterruptDisable)
        {

        }

        public override byte Execute(Registers6502 registers, Bus6502 bus)
        {
            return 0;
        }
    }
}
