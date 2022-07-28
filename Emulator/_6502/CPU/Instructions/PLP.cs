using Emulator._6502.Devices;

namespace Emulator._6502.CPU.Instructions
{
    public sealed class PLP : Instruction6502
    {
        public PLP() : base("PLP", 1, AddrMode6502.Implied, Status6502.Carry | Status6502.Zero | Status6502.InterruptDisable | Status6502.Decimal | Status6502.Break | Status6502.OverFlow | Status6502.Negative)
        {

        }

        public override byte Execute(Registers6502 registers, Bus6502 bus)
        {
            registers.STKP++;
            registers.A = bus.ReadByte((ushort)(0x0100 + registers.STKP));
            registers.SetFlag(Status6502.Unused, true);
            return 4;
        }
    }
}
