

namespace Emulator._6502.Instructions
{
    public abstract class LDX(byte bytesUsed, AddrMode6502 mode) : Instruction6502("LDX", bytesUsed, mode, Status6502.Zero | Status6502.Negative)
    {
        protected static void SetFlags(ref Cpu6502 cpu, byte data)
        {
            // The Zero flag is set if the result is 0
            cpu.SetFlag(Status6502.Zero, data == 0x00);

            // The negative flag is set to the most significant bit of the result
            cpu.SetFlag(Status6502.Negative, (data & 0x80) > 0);
        }
    }

    public sealed class LDX_Immediate : LDX
    {
        public LDX_Immediate() : base(2, AddrMode6502.Immediate)
        {
        }

        public override byte Execute(Cpu6502 cpu)
        {
            cpu.X = cpu.ReadByte(cpu.PC++);
            SetFlags(ref cpu, cpu.X);
            return 2;
        }
    }

    public sealed class LDX_ZeroPage : LDX
    {
        public LDX_ZeroPage() : base(2, AddrMode6502.ZeroPage)
        {
        }

        public override byte Execute(Cpu6502 cpu)
        {
            cpu.X = cpu.ReadByte(ZeroPage(ref cpu));
            SetFlags(ref cpu, cpu.X);
            return 3;
        }
    }
    public sealed class LDX_Absolute : LDX
    {
        public LDX_Absolute() : base(3, AddrMode6502.Absolute)
        {
        }

        public override byte Execute(Cpu6502 cpu)
        {
            cpu.X = cpu.ReadByte(Absolute(ref cpu));
            SetFlags(ref cpu, cpu.X);
            return 4;
        }
    }

    public sealed class LDX_ZeroPageY : LDX
    {
        public LDX_ZeroPageY() : base(2, AddrMode6502.ZeroPageY)
        {
        }

        public override byte Execute(Cpu6502 cpu)
        {
            cpu.X = cpu.ReadByte(ZeroPageY(ref cpu));
            SetFlags(ref cpu, cpu.X);
            return 4;
        }
    }
    public sealed class LDX_AbsoluteY : LDX
    {
        public LDX_AbsoluteY() : base(3, AddrMode6502.AbsoluteY)
        {
        }

        public override byte Execute(Cpu6502 cpu)
        {
            var (addr, clocks) = AbsoluteY(ref cpu);
            cpu.X = cpu.ReadByte(addr);
            SetFlags(ref cpu, cpu.X);
            return (byte)(4 + clocks);
        }
    }
}

