using Emulator._6502.Devices;

namespace Emulator._6502.CPU.Instructions
{
    public abstract class ROR : Instruction6502
    {
        protected ROR(AddrMode6502 mode) : base("ROR", mode, Status6502.Carry | Status6502.Zero | Status6502.Negative)
        {
        }
    }

    public sealed class ROR_Accumulator : ROR
    {
        public ROR_Accumulator() : base(AddrMode6502.Accumulator)
        {
        }

        public override byte Execute(Registers6502 registers, Bus6502 bus)
        {
            return 0;
        }
    }

    public sealed class ROR_ZeroPage : ROR
    {
        public ROR_ZeroPage() : base(AddrMode6502.ZeroPage)
        {
        }

        public override byte Execute(Registers6502 registers, Bus6502 bus)
        {
            return 0;
        }
    }
    public sealed class ROR_Absolute : ROR
    {
        public ROR_Absolute() : base(AddrMode6502.Absolute)
        {
        }

        public override byte Execute(Registers6502 registers, Bus6502 bus)
        {
            return 0;
        }
    }

    public sealed class ROR_ZeroPageX : ROR
    {
        public ROR_ZeroPageX() : base(AddrMode6502.ZeroPageX)
        {
        }

        public override byte Execute(Registers6502 registers, Bus6502 bus)
        {
            return 0;
        }
    }
    public sealed class ROR_AbsoluteX : ROR
    {
        public ROR_AbsoluteX() : base(AddrMode6502.AbsoluteX)
        {
        }

        public override byte Execute(Registers6502 registers, Bus6502 bus)
        {
            return 0;
        }
    }
}

