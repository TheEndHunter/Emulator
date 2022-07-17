using Emulator._6502.Devices;

namespace Emulator._6502.CPU.Instructions
{
    public abstract class DEC : Instruction6502
    {
        protected DEC(AddrMode6502 mode) : base("DEC", mode, Status6502.Zero | Status6502.Negative)
        {
        }
    }

    public sealed class DEC_ZeroPage : DEC
    {
        public DEC_ZeroPage() : base(AddrMode6502.ZeroPage)
        {
        }

        public override byte Execute(Registers6502 registers, Bus6502 bus)
        {
            return 0;
        }
    }
    public sealed class DEC_Absolute : DEC
    {
        public DEC_Absolute() : base(AddrMode6502.Absolute)
        {
        }

        public override byte Execute(Registers6502 registers, Bus6502 bus)
        {
            return 0;
        }
    }

    public sealed class DEC_ZeroPageX : DEC
    {
        public DEC_ZeroPageX() : base(AddrMode6502.ZeroPageX)
        {
        }

        public override byte Execute(Registers6502 registers, Bus6502 bus)
        {
            return 0;
        }
    }
    public sealed class DEC_AbsoluteX : DEC
    {
        public DEC_AbsoluteX() : base(AddrMode6502.AbsoluteX)
        {
        }

        public override byte Execute(Registers6502 registers, Bus6502 bus)
        {
            return 0;
        }
    }
}

