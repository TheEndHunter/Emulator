using Emulator._6502.Devices;

namespace Emulator._6502.CPU.Instructions
{
    public sealed class CLI : Instruction6502
    {
        public CLI() : base("CLI", 1, AddrMode6502.Implied, Status6502.InterruptDisable)
        {
        }

        public override byte Execute(ref Registers6502 registers, Bus6502 bus)
        {
            registers.SetFlag(Status6502.InterruptDisable, false);
            return 2;
        }
    }
}
