

namespace Emulator._6502.Instructions
{
    public abstract class LDA : Instruction6502
    {
        protected LDA(byte bytesUsed, AddrMode6502 mode) : base("LDA", bytesUsed, mode, Status6502.Zero | Status6502.Negative)
        {
        }
        protected static void SetFlags(ref Cpu6502 cpu, byte data)
        {
            // The Zero flag is set if the result is 0
            cpu.SetFlag(Status6502.Zero, data == 0x00);

            // The negative flag is set to the most significant bit of the result
            cpu.SetFlag(Status6502.Negative, (data & 0x80) > 0);
        }
    }

    public sealed class LDA_Immediate : LDA
    {
        public LDA_Immediate() : base(2, AddrMode6502.Immediate)
        {
        }

        public override byte Execute(Cpu6502 cpu)
        {
            cpu.A = cpu.ReadByte(cpu.PC++);
            SetFlags(ref cpu, cpu.A);
            return 2;
        }
    }

    public sealed class LDA_IndexedIndirect : LDA
    {
        public LDA_IndexedIndirect() : base(2, AddrMode6502.IndexedIndirect)
        {
        }

        public override byte Execute(Cpu6502 cpu)
        {
            cpu.A = cpu.ReadByte(IndexIndirect(ref cpu));
            SetFlags(ref cpu, cpu.A);
            return 6;
        }
    }
    public sealed class LDA_IndirectIndexed : LDA
    {
        public LDA_IndirectIndexed() : base(2, AddrMode6502.IndirectIndexed)
        {
        }

        public override byte Execute(Cpu6502 cpu)
        {
            var (addr, clocks) = IndirectIndex(ref cpu);
            cpu.A = cpu.ReadByte(addr);
            SetFlags(ref cpu, cpu.A);
            return (byte)(5 + clocks);
        }
    }

    public sealed class LDA_ZeroPage : LDA
    {
        public LDA_ZeroPage() : base(2, AddrMode6502.ZeroPage)
        {
        }

        public override byte Execute(Cpu6502 cpu)
        {
            cpu.A = cpu.ReadByte(ZeroPage(ref cpu));
            SetFlags(ref cpu, cpu.A);
            return 3;
        }
    }
    public sealed class LDA_Absolute : LDA
    {
        public LDA_Absolute() : base(3, AddrMode6502.Absolute)
        {
        }

        public override byte Execute(Cpu6502 cpu)
        {
            cpu.A = cpu.ReadByte(Absolute(ref cpu));
            SetFlags(ref cpu, cpu.A);
            return 4;
        }
    }

    public sealed class LDA_ZeroPageX : LDA
    {
        public LDA_ZeroPageX() : base(2, AddrMode6502.ZeroPageX)
        {
        }

        public override byte Execute(Cpu6502 cpu)
        {
            cpu.A = cpu.ReadByte(ZeroPageX(ref cpu));
            SetFlags(ref cpu, cpu.A);
            return 4;
        }
    }
    public sealed class LDA_AbsoluteX : LDA
    {
        public LDA_AbsoluteX() : base(3, AddrMode6502.AbsoluteX)
        {
        }

        public override byte Execute(Cpu6502 cpu)
        {
            var (addr, clocks) = AbsoluteX(ref cpu);
            cpu.A = cpu.ReadByte(addr);
            SetFlags(ref cpu, cpu.A);
            return (byte)(4 + clocks);
        }
    }
    public sealed class LDA_AbsoluteY : LDA
    {
        public LDA_AbsoluteY() : base(3, AddrMode6502.AbsoluteY)
        {
        }

        public override byte Execute(Cpu6502 cpu)
        {
            var (addr, clocks) = AbsoluteY(ref cpu);
            cpu.A = cpu.ReadByte(addr);
            SetFlags(ref cpu, cpu.A);
            return (byte)(4 + clocks);
        }
    }
}

