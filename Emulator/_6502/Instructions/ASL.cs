

namespace Emulator._6502.Instructions
{
    public abstract class ASL : Instruction6502
    {
        protected ASL(byte bytesUsed, AddrMode6502 mode) : base("ASL", bytesUsed, mode, Status6502.Carry | Status6502.Zero | Status6502.Negative)
        {
        }
        protected static void SetFlags(ref Cpu6502 cpu, ushort data)
        {

            // The carry flag out exists in the high byte bit 0
            cpu.SetFlag(Status6502.Carry, (data & 0xFF00) > 0);

            // The Zero flag is set if the result is 0
            cpu.SetFlag(Status6502.Zero, (data & 0x00FF) == 0x00);


            // The negative flag is set to the most significant bit of the result
            cpu.SetFlag(Status6502.Negative, (data & 0x80) > 0);
        }
    }

    public sealed class ASL_Acumulator : ASL
    {
        public ASL_Acumulator() : base(1, AddrMode6502.Accumulator)
        {
        }

        public override byte Execute(Cpu6502 cpu)
        {
            var temp = (ushort)(cpu.A << 1);
            cpu.A = (byte)(temp & 0x00FF);
            SetFlags(ref cpu, temp);
            return 2;
        }
    }

    public sealed class ASL_ZeroPage : ASL
    {
        public ASL_ZeroPage() : base(2, AddrMode6502.ZeroPage)
        {
        }

        public override byte Execute(Cpu6502 cpu)
        {
            var raddr = cpu.PC;
            var temp = (ushort)(cpu.ReadByte(ZeroPage(ref cpu)) << 1);
            cpu.WriteByte(raddr, (byte)(temp & 0x00FF));
            SetFlags(ref cpu, temp);
            return 5;
        }
    }

    public sealed class ASL_ZeroPageX : ASL
    {
        public ASL_ZeroPageX() : base(2, AddrMode6502.ZeroPageX)
        {
        }

        public override byte Execute(Cpu6502 cpu)
        {
            var raddr = cpu.PC;
            var temp = (ushort)(cpu.ReadByte(ZeroPageX(ref cpu)) << 1);
            cpu.WriteByte(raddr, (byte)(temp & 0x00FF));
            SetFlags(ref cpu, temp);
            return 6;
        }
    }
    public sealed class ASL_Absolute : ASL
    {
        public ASL_Absolute() : base(3, AddrMode6502.Absolute)
        {
        }

        public override byte Execute(Cpu6502 cpu)
        {
            var raddr = cpu.PC;
            var temp = (ushort)(cpu.ReadByte(Absolute(ref cpu)) << 1);
            cpu.WriteByte(raddr, (byte)(temp & 0x00FF));
            SetFlags(ref cpu, temp);
            return 6;
        }
    }

    public sealed class ASL_AbsoluteX : ASL
    {
        public ASL_AbsoluteX() : base(3, AddrMode6502.AbsoluteX)
        {
        }

        public override byte Execute(Cpu6502 cpu)
        {
            var raddr = cpu.PC;
            var temp = (ushort)(cpu.ReadByte(AbsoluteX(ref cpu).addr) << 1);
            cpu.WriteByte(raddr, (byte)(temp & 0x00FF));
            SetFlags(ref cpu, temp);
            return 7;
        }
    }
}
