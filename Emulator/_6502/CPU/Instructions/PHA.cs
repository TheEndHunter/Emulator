using Emulator._6502.Devices;

namespace Emulator._6502.CPU.Instructions
{
    public sealed class PHA : Instruction6502
    {
        public PHA() : base("PHA", 1, AddrMode6502.Implied, Status6502.None)
        {

        }

        public override byte Execute(Registers6502 registers, Bus6502 bus)
        {
            bus.Write((ushort)(0x0100 + registers.STKP), registers.A);
            registers.STKP--;
            return 3;
        }
    }
}
