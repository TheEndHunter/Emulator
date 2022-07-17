using Emulator._6502.Devices;

namespace Emulator._6502.CPU.Instructions
{
    public sealed class JSR : Instruction6502
    {
        public JSR() : base("JSR", AddrMode6502.Absolute, Status6502.None)
        {
        }

        public override byte Execute(Registers6502 registers, Bus6502 bus)
        {
            return 0;
        }
    }
}

