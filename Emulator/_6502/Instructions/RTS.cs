

namespace Emulator._6502.Instructions
{
    public sealed class RTS : Instruction6502
    {
        public RTS() : base("RTS", 1, AddrMode6502.Implied, 0)
        {

        }

        public override byte Execute(Cpu6502 cpu)
        {
            cpu.PC = (ushort)(cpu.ReadWord((ushort)(0x0100 + cpu.STKP)) + 1);
            cpu.STKP += 2;
            return 6;
        }
    }
}
