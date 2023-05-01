namespace Emulator._6502.Instructions
{
    public abstract class BIT : Instruction6502
    {
        protected BIT(byte bytesUsed, AddrMode6502 mode) : base("BIT", bytesUsed, mode, Status6502.Zero | Status6502.OverFlow | Status6502.Negative)
        {
        }

        protected static void SetFlags(ref Cpu6502 cpu, ushort data, byte fetch)
        {
            cpu.SetFlag(Status6502.Zero, (data & 0x00FF) == 0x00);

            cpu.SetFlag(Status6502.Negative, (fetch & 1 << 7) > 0);

            cpu.SetFlag(Status6502.OverFlow, (fetch & 1 << 6) > 0);
        }
    }

    public sealed class BIT_ZeroPage : BIT
    {
        public BIT_ZeroPage() : base(2, AddrMode6502.ZeroPage)
        {
        }

        public override byte Execute(Cpu6502 cpu)
        {
            byte f = cpu.ReadByte(ZeroPage(ref cpu));
            ushort t = (ushort)(cpu.A & f);
            SetFlags(ref cpu, t, f);
            return 3;
        }
    }
    public sealed class BIT_Absolute : BIT
    {
        public BIT_Absolute() : base(3, AddrMode6502.Absolute)
        {
        }

        public override byte Execute(Cpu6502 cpu)
        {
            byte f = cpu.ReadByte(Absolute(ref cpu));
            ushort t = (ushort)(cpu.A & f);
            SetFlags(ref cpu, t, f);
            return 4;
        }
    }


}

