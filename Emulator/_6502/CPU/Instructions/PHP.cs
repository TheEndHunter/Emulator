using Emulator._6502.Devices;

namespace Emulator._6502.CPU.Instructions
{
    public sealed class PHP : Instruction6502
    {
        public PHP() : base("PHP", 1, AddrMode6502.Implied, Status6502.None)
        {

        }

        public override byte Execute(Registers6502 registers, Bus6502 bus)
        {
            bus.Write((ushort)(0x0100 + registers.STKP), (byte)(registers.Status | Status6502.Break | Status6502.Unused));
            registers.STKP--;
            registers.SetFlag(Status6502.Break, false);
            registers.SetFlag(Status6502.Unused, false);
            return 3;
        }
    }
}
