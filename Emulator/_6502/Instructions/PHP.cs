

namespace Emulator._6502.Instructions
{
    public sealed class PHP : Instruction6502
    {
        public PHP() : base("PHP", 1, AddrMode6502.Implied, 0)
        {

        }

        public override byte Execute(Cpu6502 cpu)
        {
            cpu.WriteByte((ushort)(0x0100 + cpu.STKP), (byte)(cpu.Status | Status6502.Break | Status6502.Unused));
            cpu.STKP--;
            cpu.SetFlag(Status6502.Break, false);
            cpu.SetFlag(Status6502.Unused, false);
            return 3;
        }
    }
}
