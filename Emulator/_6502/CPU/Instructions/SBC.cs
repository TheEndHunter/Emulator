using Emulator._6502.Devices;

namespace Emulator._6502.CPU.Instructions
{
    public abstract class SBC : Instruction6502
    {
        protected SBC(AddrMode6502 mode) : base("SBC", mode, Status6502.Carry | Status6502.Zero | Status6502.OverFlow | Status6502.Negative)
        {
        }
    }

    public sealed class SBC_Immediate : SBC
    {
        public SBC_Immediate() : base(AddrMode6502.Immediate)
        {
        }

        public override byte Execute(Registers6502 registers, Bus6502 bus)
        {
            return 0;
        }
    }

    public sealed class SBC_IndexedIndirect : SBC
    {
        public SBC_IndexedIndirect() : base(AddrMode6502.IndexedIndirect)
        {
        }

        public override byte Execute(Registers6502 registers, Bus6502 bus)
        {
            return 0;
        }
    }

    public sealed class SBC_IndirectIndexed : SBC
    {
        public SBC_IndirectIndexed() : base(AddrMode6502.IndirectIndexed)
        {
        }

        public override byte Execute(Registers6502 registers, Bus6502 bus)
        {
            return 0;
        }
    }

    public sealed class SBC_ZeroPage : SBC
    {
        public SBC_ZeroPage() : base(AddrMode6502.ZeroPage)
        {
        }

        public override byte Execute(Registers6502 registers, Bus6502 bus)
        {
            return 0;
        }
    }
    public sealed class SBC_Absolute : SBC
    {
        public SBC_Absolute() : base(AddrMode6502.Absolute)
        {
        }

        public override byte Execute(Registers6502 registers, Bus6502 bus)
        {
            return 0;
        }
    }

    public sealed class SBC_ZeroPageX : SBC
    {
        public SBC_ZeroPageX() : base(AddrMode6502.ZeroPageX)
        {
        }

        public override byte Execute(Registers6502 registers, Bus6502 bus)
        {
            return 0;
        }
    }
    public sealed class SBC_AbsoluteX : SBC
    {
        public SBC_AbsoluteX() : base(AddrMode6502.AbsoluteX)
        {
        }

        public override byte Execute(Registers6502 registers, Bus6502 bus)
        {
            return 0;
        }
    }
    public sealed class SBC_AbsoluteY : SBC
    {
        public SBC_AbsoluteY() : base(AddrMode6502.AbsoluteY)
        {
        }

        public override byte Execute(Registers6502 registers, Bus6502 bus)
        {
            return 0;
        }
    }
}

