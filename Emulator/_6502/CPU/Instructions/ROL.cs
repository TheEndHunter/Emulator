using Emulator._6502.Devices;

namespace Emulator._6502.CPU.Instructions
{
    public abstract class ROL : Instruction6502
    {
        protected ROL(AddrMode6502 mode) : base("ROL", mode, Status6502.Carry | Status6502.Zero | Status6502.Negative)
        {
        }
    }

    public sealed class ROL_Accumulator : ROL
    {
        public ROL_Accumulator() : base(AddrMode6502.Accumulator)
        {
        }

        public override byte Execute(Registers6502 registers, Bus6502 bus)
        {
            return 0;
        }
    }

    public sealed class ROL_ZeroPage : ROL
    {
        public ROL_ZeroPage() : base(AddrMode6502.ZeroPage)
        {
        }

        public override byte Execute(Registers6502 registers, Bus6502 bus)
        {
            return 0;
        }
    }
    public sealed class ROL_Absolute : ROL
    {
        public ROL_Absolute() : base(AddrMode6502.Absolute)
        {
        }

        public override byte Execute(Registers6502 registers, Bus6502 bus)
        {
            return 0;
        }
    }

    public sealed class ROL_ZeroPageX : ROL
    {
        public ROL_ZeroPageX() : base(AddrMode6502.ZeroPageX)
        {
        }

        public override byte Execute(Registers6502 registers, Bus6502 bus)
        {
            return 0;
        }
    }
    public sealed class ROL_AbsoluteX : ROL
    {
        public ROL_AbsoluteX() : base(AddrMode6502.AbsoluteX)
        {
        }

        public override byte Execute(Registers6502 registers, Bus6502 bus)
        {
            return 0;
        }
    }
}

