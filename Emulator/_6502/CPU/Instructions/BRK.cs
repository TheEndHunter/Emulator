using Emulator._6502.Devices;

namespace Emulator._6502.CPU.Instructions
{
    public sealed class BRK : Instruction6502
    {
        public BRK() : base("BRK", 1, AddrMode6502.Implied, Status6502.None)
        {
        }

        public override byte Execute(ref Registers6502 registers, Bus6502 bus)
        {
            registers.PC--;
            registers.SetFlag(Status6502.InterruptDisable, true);
            bus.Write((ushort)(0x0100 + registers.STKP), (byte)(((registers.PC) >> 8) & 0x00FF));
            registers.STKP--;
            bus.Write((ushort)(0x0100 + registers.STKP), (byte)(registers.PC & 0x00FF));
            registers.STKP--;

            registers.SetFlag(Status6502.Break, true);
            bus.Write((ushort)(0x0100 + registers.STKP), (byte)registers.Status);
            registers.STKP--;

            registers.SetFlag(Status6502.Break, false);
            registers.PC = bus.ReadWord(0xFFFE);
            return 7;
        }
    }
}
