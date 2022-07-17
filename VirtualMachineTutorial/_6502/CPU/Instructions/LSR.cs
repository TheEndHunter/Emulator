using Emulator._6502.Devices;

namespace Emulator._6502.CPU.Instructions
{
    public abstract class LSR : Instruction6502
    {
        protected LSR(AddrMode6502 mode) : base("LSR", mode, Status6502.Zero | Status6502.Negative)
        {
        }
    }

    public sealed class LSR_Accumulator : LSR
    {
        public LSR_Accumulator() : base(AddrMode6502.Accumulator)
        {
        }

        public override byte Execute(Registers6502 registers, Bus6502 bus)
        {
            return 0;
        }
    }

    public sealed class LSR_ZeroPage : LSR
    {
        public LSR_ZeroPage() : base(AddrMode6502.ZeroPage)
        {
        }

        public override byte Execute(Registers6502 registers, Bus6502 bus)
        {
            return 0;
        }
    }
    public sealed class LSR_Absolute : LSR
    {
        public LSR_Absolute() : base(AddrMode6502.Absolute)
        {
        }

        public override byte Execute(Registers6502 registers, Bus6502 bus)
        {
            return 0;
        }
    }

    public sealed class LSR_ZeroPageX : LSR
    {
        public LSR_ZeroPageX() : base(AddrMode6502.ZeroPageX)
        {
        }

        public override byte Execute(Registers6502 registers, Bus6502 bus)
        {
            return 0;
        }
    }
    public sealed class LSR_AbsoluteX : LSR
    {
        public LSR_AbsoluteX() : base(AddrMode6502.AbsoluteX)
        {
        }

        public override byte Execute(Registers6502 registers, Bus6502 bus)
        {
            return 0;
        }
    }
}

