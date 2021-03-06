using Emulator._6502.Devices;

namespace Emulator._6502.CPU.Instructions
{
    public sealed class CLI : Instruction6502
    {
        public CLI() : base("CLI", AddrMode6502.Implied, Status6502.InterruptDisable)
        {
        }

        public override byte Execute(Registers6502 registers, Bus6502 bus)
        {
            return 0;
        }
    }
}
