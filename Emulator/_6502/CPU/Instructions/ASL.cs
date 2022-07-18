using Emulator._6502.Devices;

namespace Emulator._6502.CPU.Instructions
{
    public abstract class ASL : Instruction6502
    {
        protected ASL(AddrMode6502 mode) : base("ASL", mode, Status6502.Carry | Status6502.Zero | Status6502.Negative)
        {
        }
    }

    public sealed class ASL_Acumulator : ASL
    {
        public ASL_Acumulator() : base(AddrMode6502.Accumulator)
        {
        }

        public override byte Execute(Registers6502 registers, Bus6502 bus)
        {
            return 0;
        }
    }

    public sealed class ASL_ZeroPage : ASL
    {
        public ASL_ZeroPage() : base(AddrMode6502.ZeroPage)
        {
        }

        public override byte Execute(Registers6502 registers, Bus6502 bus)
        {
            return 0;
        }
    }

    public sealed class ASL_ZeroPageX : ASL
    {
        public ASL_ZeroPageX() : base(AddrMode6502.ZeroPageX)
        {
        }

        public override byte Execute(Registers6502 registers, Bus6502 bus)
        {
            return 0;
        }
    }
    public sealed class ASL_Absolute : ASL
    {
        public ASL_Absolute() : base(AddrMode6502.Absolute)
        {
        }

        public override byte Execute(Registers6502 registers, Bus6502 bus)
        {
            return 0;
        }
    }

    public sealed class ASL_AbsoluteX : ASL
    {
        public ASL_AbsoluteX() : base(AddrMode6502.AbsoluteX)
        {
        }

        public override byte Execute(Registers6502 registers, Bus6502 bus)
        {
            return 0;
        }
    }
}
