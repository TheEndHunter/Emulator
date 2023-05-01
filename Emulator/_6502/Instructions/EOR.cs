

namespace Emulator._6502.Instructions
{
    public abstract class EOR : Instruction6502
    {
        protected EOR(byte bytesUsed, AddrMode6502 mode) : base("EOR", bytesUsed, mode, Status6502.Zero | Status6502.Negative)
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
    public sealed class EOR_Immediate : EOR
    {
        public EOR_Immediate() : base(2, AddrMode6502.Immediate)
        {
        }

        public override byte Execute(Cpu6502 cpu)
        {
            cpu.A = (byte)(cpu.A ^ cpu.ReadByte(ZeroPage(ref cpu)));
            SetFlags(ref cpu, cpu.A);
            return 2;
        }
    }
    public sealed class EOR_IndirectIndexed : EOR
    {
        public EOR_IndirectIndexed() : base(2, AddrMode6502.IndirectIndexed)
        {
        }

        public override byte Execute(Cpu6502 cpu)
        {
            var (addr, clocks) = IndirectIndex(ref cpu);
            cpu.A = (byte)(cpu.A ^ cpu.ReadByte(addr));
            SetFlags(ref cpu, cpu.A);
            return (byte)(5 + clocks);

        }
    }

    public sealed class EOR_IndexedIndirect : EOR
    {
        public EOR_IndexedIndirect() : base(2, AddrMode6502.IndexedIndirect)
        {
        }

        public override byte Execute(Cpu6502 cpu)
        {
            cpu.A = (byte)(cpu.A ^ cpu.ReadByte(IndexIndirect(ref cpu)));
            SetFlags(ref cpu, cpu.A);
            return 6;
        }
    }

    public sealed class EOR_ZeroPage : EOR
    {
        public EOR_ZeroPage() : base(2, AddrMode6502.ZeroPage)
        {
        }

        public override byte Execute(Cpu6502 cpu)
        {
            cpu.A = (byte)(cpu.A ^ cpu.ReadByte(ZeroPage(ref cpu)));
            SetFlags(ref cpu, cpu.A);
            return 3;
        }
    }
    public sealed class EOR_Absolute : EOR
    {
        public EOR_Absolute() : base(3, AddrMode6502.Absolute)
        {
        }

        public override byte Execute(Cpu6502 cpu)
        {
            cpu.A = (byte)(cpu.A ^ cpu.ReadByte(Absolute(ref cpu)));
            SetFlags(ref cpu, cpu.A);
            return 4;
        }
    }

    public sealed class EOR_ZeroPageX : EOR
    {
        public EOR_ZeroPageX() : base(2, AddrMode6502.ZeroPageX)
        {
        }

        public override byte Execute(Cpu6502 cpu)
        {
            cpu.A = (byte)(cpu.A ^ cpu.ReadByte(ZeroPageX(ref cpu)));
            SetFlags(ref cpu, cpu.A);
            return 4;
        }
    }
    public sealed class EOR_AbsoluteX : EOR
    {
        public EOR_AbsoluteX() : base(3, AddrMode6502.AbsoluteX)
        {
        }

        public override byte Execute(Cpu6502 cpu)
        {
            var (addr, clocks) = AbsoluteX(ref cpu);
            cpu.A = (byte)(cpu.A ^ cpu.ReadByte(addr));
            SetFlags(ref cpu, cpu.A);
            return (byte)(4 + clocks);
        }
    }
    public sealed class EOR_AbsoluteY : EOR
    {
        public EOR_AbsoluteY() : base(3, AddrMode6502.AbsoluteY)
        {
        }

        public override byte Execute(Cpu6502 cpu)
        {
            var (addr, clocks) = AbsoluteY(ref cpu);
            cpu.A = (byte)(cpu.A ^ cpu.ReadByte(addr));
            SetFlags(ref cpu, cpu.A);
            return (byte)(4 + clocks);
        }
    }
}

