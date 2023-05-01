

namespace Emulator._6502.Instructions
{
    public abstract class INC : Instruction6502
    {
        protected INC(byte bytesUsed, AddrMode6502 mode) : base("INC", bytesUsed, mode, Status6502.Zero | Status6502.Negative)
        {
        }
        protected static void SetFlags(ref Cpu6502 cpu, ushort data)
        {
            // The Zero flag is set if the result is 0
            cpu.SetFlag(Status6502.Zero, (data & 0x00FF) == 0);

            // The negative flag is set to the most significant bit of the result
            cpu.SetFlag(Status6502.Negative, (data & 0x80) > 0);
        }
    }

    public sealed class INC_ZeroPage : INC
    {
        public INC_ZeroPage() : base(2, AddrMode6502.ZeroPage)
        {
        }

        public override byte Execute(Cpu6502 cpu)
        {
            var addr = ZeroPage(ref cpu);
            var t = (ushort)(cpu.ReadByte(addr) + 1);
            cpu.WriteByte(addr, (byte)(t & 0x00FF));
            SetFlags(ref cpu, t);
            return 5;
        }
    }
    public sealed class INC_Absolute : INC
    {
        public INC_Absolute() : base(3, AddrMode6502.Absolute)
        {
        }

        public override byte Execute(Cpu6502 cpu)
        {
            var addr = Absolute(ref cpu);
            var t = (ushort)(cpu.ReadByte(addr) + 1);
            cpu.WriteByte(addr, (byte)(t & 0x00FF));
            SetFlags(ref cpu, t);
            return 6;
        }
    }

    public sealed class INC_ZeroPageX : INC
    {
        public INC_ZeroPageX() : base(2, AddrMode6502.ZeroPageX)
        {
        }

        public override byte Execute(Cpu6502 cpu)
        {
            var addr = ZeroPageX(ref cpu);
            var t = (ushort)(cpu.ReadByte(addr) + 1);
            cpu.WriteByte(addr, (byte)(t & 0x00FF));
            SetFlags(ref cpu, t);
            return 6;
        }
    }
    public sealed class INC_AbsoluteX : INC
    {
        public INC_AbsoluteX() : base(3, AddrMode6502.AbsoluteX)
        {
        }

        public override byte Execute(Cpu6502 cpu)
        {
            var addr = AbsoluteX(ref cpu).addr;
            var t = (ushort)(cpu.ReadByte(addr) + 1);
            cpu.WriteByte(addr, (byte)(t & 0x00FF));
            SetFlags(ref cpu, t);
            return 7;
        }
    }
}

