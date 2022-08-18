using Emulator._6502.Devices;

namespace Emulator._6502.CPU.Instructions
{
    public sealed class RTS : Instruction6502
    {
        public RTS() : base("RTS", 1, AddrMode6502.Implied, Status6502.None)
        {

        }

        public override byte Execute(ref Registers6502 registers, Bus6502 bus)
        {
            registers.STKP++;
            registers.PC = bus.ReadByte((ushort)(0x0100 + registers.STKP));
            registers.STKP++;
            registers.PC |= (ushort)(bus.ReadByte((ushort)(0x0100 + registers.STKP)) << 8);
            return 6;
        }
    }
}
