using Emulator._6502.Devices;

namespace Emulator._6502.CPU.Instructions
{
    public sealed class SEC : Instruction6502
    {
        public SEC() : base("SEC", AddrMode6502.Implied, Status6502.Carry)
        {

        }

        public override byte Execute(Registers6502 registers, Bus6502 bus)
        {
            return 0;
        }
    }
}
