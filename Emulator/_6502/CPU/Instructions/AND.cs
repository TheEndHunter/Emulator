using Emulator._6502.Devices;

namespace Emulator._6502.CPU.Instructions
{
    public abstract class AND : Instruction6502
    {
        protected AND(AddrMode6502 mode) : base("AND", mode, Status6502.Zero | Status6502.Negative)
        {
        }
    }

    public sealed class AND_Immediate : AND
    {
        public AND_Immediate() : base(AddrMode6502.Immediate)
        {
        }

        public override byte Execute(Registers6502 registers, Bus6502 bus)
        {
            return 0;
        }
    }

    public sealed class AND_ZeroPage : AND
    {
        public AND_ZeroPage() : base(AddrMode6502.ZeroPage)
        {
        }

        public override byte Execute(Registers6502 registers, Bus6502 bus)
        {
            return 0;
        }
    }

    public sealed class AND_ZeroPageX : AND
    {
        public AND_ZeroPageX() : base(AddrMode6502.ZeroPageX)
        {
        }

        public override byte Execute(Registers6502 registers, Bus6502 bus)
        {
            return 0;
        }
    }
    public sealed class AND_Absolute : AND
    {
        public AND_Absolute() : base(AddrMode6502.Absolute)
        {
        }

        public override byte Execute(Registers6502 registers, Bus6502 bus)
        {
            return 0;
        }
    }

    public sealed class AND_IndirectIndexed : AND
    {
        public AND_IndirectIndexed() : base(AddrMode6502.IndirectIndexed)
        {
        }

        public override byte Execute(Registers6502 registers, Bus6502 bus)
        {
            return 0;
        }
    }

    public sealed class AND_IndexedIndirect : AND
    {
        public AND_IndexedIndirect() : base(AddrMode6502.IndexedIndirect)
        {
        }

        public override byte Execute(Registers6502 registers, Bus6502 bus)
        {
            return 0;
        }
    }

    public sealed class AND_AbsoluteX : AND
    {
        public AND_AbsoluteX() : base(AddrMode6502.AbsoluteX)
        {
        }

        public override byte Execute(Registers6502 registers, Bus6502 bus)
        {
            return 0;
        }
    }
    public sealed class AND_AbsoluteY : AND
    {
        public AND_AbsoluteY() : base(AddrMode6502.AbsoluteY)
        {
        }

        public override byte Execute(Registers6502 registers, Bus6502 bus)
        {
            return 0;
        }
    }
}
