using Emulator._6502.Devices;

namespace Emulator._6502.CPU.Instructions
{
    public sealed class BCC : Instruction6502
    {
        public BCC() : base("BCC", AddrMode6502.Relative, Status6502.None)
        {
        }

        public override byte Execute(Registers6502 registers, Bus6502 bus)
        {
            return 0;
        }
    }
}
