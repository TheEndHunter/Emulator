using Emulator._6502.Devices;

namespace Emulator._6502.CPU.Instructions
{
    public abstract class BIT : Instruction6502
    {
        protected BIT(AddrMode6502 mode) : base("BIT", mode, Status6502.Zero | Status6502.OverFlow | Status6502.Negative)
        {
        }

        protected static void SetFlags(Registers6502 registers, ushort data, byte fetch)
        {
            registers.SetFlag(Status6502.Zero, (data & 0x00FF) == 0x00);

            registers.SetFlag(Status6502.Negative, (fetch & (1 << 7)) > 0);

            registers.SetFlag(Status6502.OverFlow, (fetch & (1 << 6)) > 0);
        }
    }

    public sealed class BIT_ZeroPage : BIT
    {
        public BIT_ZeroPage() : base(AddrMode6502.ZeroPage)
        {
        }

        public override byte Execute(Registers6502 registers, Bus6502 bus)
        {
            byte f = bus.ReadByte(ZeroPage(registers, bus));
            ushort t = (ushort)(registers.A & f);
            SetFlags(registers, t, f);
            return 3;
        }
    }
    public sealed class BIT_Absolute : BIT
    {
        public BIT_Absolute() : base(AddrMode6502.Absolute)
        {
        }

        public override byte Execute(Registers6502 registers, Bus6502 bus)
        {
            byte f = bus.ReadByte(Absolute(registers, bus));
            ushort t = (ushort)(registers.A & f);
            SetFlags(registers, t, f);
            return 4;
        }
    }


}

