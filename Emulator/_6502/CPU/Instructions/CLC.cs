using Emulator._6502.Devices;

namespace Emulator._6502.CPU.Instructions
{
    public sealed class CLC : Instruction6502
    {
        public CLC() : base("CLC", 1, AddrMode6502.Implied, Status6502.Carry)
        {
        }

        public override byte Execute(Registers6502 registers, Bus6502 bus)
        {
            registers.SetFlag(Status6502.Carry, false);
            return 2;
        }
    }
}
