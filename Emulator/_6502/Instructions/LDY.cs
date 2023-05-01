

namespace Emulator._6502.Instructions
{
    public abstract class LDY : Instruction6502
    {
        protected LDY(byte bytesUsed, AddrMode6502 mode) : base("LDY", bytesUsed, mode, Status6502.Zero | Status6502.Negative)
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

    public sealed class LDY_Immediate : LDY
    {
        public LDY_Immediate() : base(2, AddrMode6502.Immediate)
        {
        }

        public override byte Execute(Cpu6502 cpu)
        {
            cpu.Y = cpu.ReadByte(cpu.PC++);
            SetFlags(ref cpu, cpu.Y);
            return 2;
        }
    }

    public sealed class LDY_ZeroPage : LDY
    {
        public LDY_ZeroPage() : base(2, AddrMode6502.ZeroPage)
        {
        }

        public override byte Execute(Cpu6502 cpu)
        {
            cpu.Y = cpu.ReadByte(ZeroPage(ref cpu));
            SetFlags(ref cpu, cpu.Y);
            return 3;
        }
    }
    public sealed class LDY_Absolute : LDY
    {
        public LDY_Absolute() : base(3, AddrMode6502.Absolute)
        {
        }

        public override byte Execute(Cpu6502 cpu)
        {
            cpu.Y = cpu.ReadByte(Absolute(ref cpu));
            SetFlags(ref cpu, cpu.Y);
            return 4;
        }
    }

    public sealed class LDY_ZeroPageX : LDY
    {
        public LDY_ZeroPageX() : base(2, AddrMode6502.ZeroPageX)
        {
        }

        public override byte Execute(Cpu6502 cpu)
        {
            cpu.Y = cpu.ReadByte(ZeroPageX(ref cpu));
            SetFlags(ref cpu, cpu.Y);
            return 4;
        }
    }
    public sealed class LDY_AbsoluteX : LDY
    {
        public LDY_AbsoluteX() : base(3, AddrMode6502.AbsoluteX)
        {
        }

        public override byte Execute(Cpu6502 cpu)
        {
            var (addr, clocks) = AbsoluteX(ref cpu);
            cpu.Y = cpu.ReadByte(addr);
            SetFlags(ref cpu, cpu.Y);
            return (byte)(4 + clocks);
        }
    }
}

