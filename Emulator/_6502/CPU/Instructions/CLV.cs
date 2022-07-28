using Emulator._6502.Devices;

namespace Emulator._6502.CPU.Instructions
{
    public sealed class CLV : Instruction6502
    {
        public CLV() : base("CLV", 1, AddrMode6502.Implied, Status6502.OverFlow)
        {
        }

        public override byte Execute(Registers6502 registers, Bus6502 bus)
        {
            registers.SetFlag(Status6502.OverFlow, false);
            return 2;
        }
    }
}
