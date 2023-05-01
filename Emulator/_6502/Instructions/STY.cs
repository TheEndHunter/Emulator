

namespace Emulator._6502.Instructions
{
    public abstract class STY : Instruction6502
    {
        protected STY(byte bytesUsed, AddrMode6502 mode) : base("STY", bytesUsed, mode, 0)
        {
        }
    }

    public sealed class STY_ZeroPage : STY
    {
        public STY_ZeroPage() : base(2, AddrMode6502.ZeroPage)
        {
        }

        public override byte Execute(Cpu6502 cpu)
        {
            cpu.WriteByte(cpu.ReadWord(ZeroPage(ref cpu)), cpu.Y);
            return 4;
        }
    }
    public sealed class STY_Absolute : STY
    {
        public STY_Absolute() : base(3, AddrMode6502.Absolute)
        {
        }

        public override byte Execute(Cpu6502 cpu)
        {
            cpu.WriteByte(cpu.ReadWord(Absolute(ref cpu)), cpu.Y);
            return 3;
        }
    }

    public sealed class STY_ZeroPageX : STY
    {
        public STY_ZeroPageX() : base(2, AddrMode6502.ZeroPageX)
        {
        }

        public override byte Execute(Cpu6502 cpu)
        {
            cpu.WriteByte(cpu.ReadWord(ZeroPageX(ref cpu)), cpu.Y);
            return 4;
        }
    }
}

