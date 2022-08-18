using Emulator._6502.Devices;

namespace Emulator._6502.CPU.Instructions
{
    public sealed class SEC : Instruction6502
    {
        public SEC() : base("SEC", 1, AddrMode6502.Implied, Status6502.Carry)
        {

        }

        public override byte Execute(ref Registers6502 registers, Bus6502 bus)
        {
            registers.SetFlag(Status6502.Carry, true);
            return 2;
        }
    }
}
