using Emulator._6502.Devices;

namespace Emulator._6502.CPU.Instructions
{
    public sealed class BVS : Instruction6502
    {
        public BVS() : base("BVS", AddrMode6502.Relative, Status6502.None)
        {
        }

        public override byte Execute(Registers6502 registers, Bus6502 bus)
        {
            return 0;
        }
    }
}
