using Emulator._6502.Devices;

namespace Emulator._6502.CPU.Instructions
{
    public sealed class BRK : Instruction6502
    {
        public BRK() : base("BRK", 1, AddrMode6502.Implied, Status6502.None)
        {
        }

        public override byte Execute(Registers6502 registers, Bus6502 bus)
        {
            registers.PC++;
            registers.SetFlag(Status6502.InterruptDisable, true);
            bus.Write((ushort)(0x0100 + registers.STKP), registers.PC);
            registers.STKP -= 2;
            registers.SetFlag(Status6502.Break, true);
            bus.Write((ushort)(0x0100 + registers.STKP), (byte)registers.Status);
            registers.STKP--;
            registers.SetFlag(Status6502.Break, false);
            registers.PC = bus.ReadWord(0xFFFE);
            return 7;
        }
    }
}
