

namespace Emulator._6502.Instructions
{
    public abstract class STA : Instruction6502
    {
        protected STA(byte bytesUsed, AddrMode6502 mode) : base("STA", bytesUsed, mode, 0)
        {
        }
    }

    public sealed class STA_IndexedIndirect : STA
    {
        public STA_IndexedIndirect() : base(2, AddrMode6502.IndexedIndirect)
        {
        }

        public override byte Execute(Cpu6502 cpu)
        {
            cpu.WriteByte(cpu.ReadWord(IndexIndirect(ref cpu)), cpu.A);
            return 6;
        }
    }

    public sealed class STA_IndirectIndexed : STA
    {
        public STA_IndirectIndexed() : base(2, AddrMode6502.IndirectIndexed)
        {
        }

        public override byte Execute(Cpu6502 cpu)
        {
            cpu.WriteByte(cpu.ReadWord(IndirectIndex(ref cpu).addr), cpu.A);
            return 6;
        }
    }

    public sealed class STA_ZeroPage : STA
    {
        public STA_ZeroPage() : base(2, AddrMode6502.ZeroPage)
        {
        }

        public override byte Execute(Cpu6502 cpu)
        {
            cpu.WriteByte(cpu.ReadWord(ZeroPage(ref cpu)), cpu.A);
            return 3;
        }
    }
    public sealed class STA_Absolute : STA
    {
        public STA_Absolute() : base(3, AddrMode6502.Absolute)
        {
        }

        public override byte Execute(Cpu6502 cpu)
        {
            cpu.WriteByte(cpu.ReadWord(Absolute(ref cpu)), cpu.A);
            return 4;
        }
    }

    public sealed class STA_ZeroPageX : STA
    {
        public STA_ZeroPageX() : base(2, AddrMode6502.ZeroPageX)
        {
        }

        public override byte Execute(Cpu6502 cpu)
        {
            cpu.WriteByte(cpu.ReadWord(ZeroPageX(ref cpu)), cpu.A);
            return 4;
        }
    }
    public sealed class STA_AbsoluteX : STA
    {
        public STA_AbsoluteX() : base(3, AddrMode6502.AbsoluteX)
        {
        }

        public override byte Execute(Cpu6502 cpu)
        {
            cpu.WriteByte(cpu.ReadWord(AbsoluteX(ref cpu).addr), cpu.A);
            return 5;
        }
    }
    public sealed class STA_AbsoluteY : STA
    {
        public STA_AbsoluteY() : base(3, AddrMode6502.AbsoluteY)
        {
        }

        public override byte Execute(Cpu6502 cpu)
        {
            cpu.WriteByte(cpu.ReadWord(AbsoluteY(ref cpu).addr), cpu.A);
            return 5;
        }
    }
}

