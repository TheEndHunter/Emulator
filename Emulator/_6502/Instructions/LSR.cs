

namespace Emulator._6502.Instructions
{
    public abstract class LSR(byte bytesUsed, AddrMode6502 mode) : Instruction6502("LSR", bytesUsed, mode, Status6502.Zero | Status6502.Negative)
    {
        protected static void SetCarry(ref Cpu6502 cpu, byte data)
        {
            // The Carry flag is set to the value of the lowest data bit(bit 0)
            cpu.SetFlag(Status6502.Carry, (byte)(data & 0x01) == 0x01);
        }

        protected static void SetZN(ref Cpu6502 cpu, byte data)
        {
            // The Zero flag is set if the result is 0
            cpu.SetFlag(Status6502.Zero, data == 0x00);

            // The negative flag is set to the most significant bit of the result
            cpu.SetFlag(Status6502.Negative, false);
        }
    }

    public sealed class LSR_Accumulator : LSR
    {
        public LSR_Accumulator() : base(1, AddrMode6502.Accumulator)
        {
        }

        public override byte Execute(Cpu6502 cpu)
        {
            SetCarry(ref cpu, cpu.A);
            cpu.A >>= 1;
            SetZN(ref cpu, cpu.A);
            return 2;
        }
    }

    public sealed class LSR_ZeroPage : LSR
    {
        public LSR_ZeroPage() : base(2, AddrMode6502.ZeroPage)
        {
        }

        public override byte Execute(Cpu6502 cpu)
        {
            var addr = ZeroPage(ref cpu);
            var d = cpu.ReadByte(addr);
            SetCarry(ref cpu, d);
            d = (byte)(d >> 1);
            SetZN(ref cpu, d);
            cpu.WriteByte(addr, d);
            return 5;
        }
    }
    public sealed class LSR_Absolute : LSR
    {
        public LSR_Absolute() : base(3, AddrMode6502.Absolute)
        {
        }

        public override byte Execute(Cpu6502 cpu)
        {
            var addr = Absolute(ref cpu);
            var d = cpu.ReadByte(addr);
            SetCarry(ref cpu, d);
            d = (byte)(d >> 1);
            SetZN(ref cpu, d);
            cpu.WriteByte(addr, d);
            return 6;
        }
    }

    public sealed class LSR_ZeroPageX : LSR
    {
        public LSR_ZeroPageX() : base(2, AddrMode6502.ZeroPageX)
        {
        }

        public override byte Execute(Cpu6502 cpu)
        {
            var addr = ZeroPageX(ref cpu);
            var d = cpu.ReadByte(addr);
            SetCarry(ref cpu, d);
            d = (byte)(d >> 1);
            SetZN(ref cpu, d);
            cpu.WriteByte(addr, d);
            return 6;
        }
    }
    public sealed class LSR_AbsoluteX : LSR
    {
        public LSR_AbsoluteX() : base(3, AddrMode6502.AbsoluteX)
        {
        }

        public override byte Execute(Cpu6502 cpu)
        {
            var addr = AbsoluteX(ref cpu).addr;
            var d = cpu.ReadByte(addr);
            SetCarry(ref cpu, d);
            d = (byte)(d >> 1);
            SetZN(ref cpu, d);
            cpu.WriteByte(addr, d);
            return 7;
        }
    }
}

