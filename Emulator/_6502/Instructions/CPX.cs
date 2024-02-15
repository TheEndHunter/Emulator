

namespace Emulator._6502.Instructions
{
    public abstract class CPX(byte bytesUsed, AddrMode6502 mode) : Instruction6502("CPX", bytesUsed, mode, Status6502.Carry | Status6502.Zero | Status6502.Negative)
    {
        protected static void SetFlags(ref Cpu6502 cpu, byte fetched)
        {
            var temp = cpu.X - fetched;
            // The carry flag out exists in the high byte bit 0
            cpu.SetFlag(Status6502.Carry, cpu.X >= fetched);

            // The Zero flag is set if the result is 0
            cpu.SetFlag(Status6502.Zero, (temp & 0x00FF) == 0x0000);

            // The negative flag is set to the most significant bit of low byte the result
            cpu.SetFlag(Status6502.Negative, (temp & 0x0080) > 0);
        }
    }

    public sealed class CPX_Immediate : CPX
    {
        public CPX_Immediate() : base(2, AddrMode6502.Immediate)
        {

        }

        public override byte Execute(Cpu6502 cpu)
        {
            SetFlags(ref cpu, cpu.ReadByte(cpu.PC++));
            return 2;
        }
    }
    public sealed class CPX_Absolute : CPX
    {
        public CPX_Absolute() : base(3, AddrMode6502.Absolute)
        {

        }

        public override byte Execute(Cpu6502 cpu)
        {
            SetFlags(ref cpu, cpu.ReadByte(Absolute(ref cpu)));
            return 4;
        }
    }

    public sealed class CPX_ZeroPage : CPX
    {
        public CPX_ZeroPage() : base(2, AddrMode6502.ZeroPage)
        {

        }

        public override byte Execute(Cpu6502 cpu)
        {
            SetFlags(ref cpu, cpu.ReadByte(ZeroPage(ref cpu)));
            return 3;
        }
    }
}
