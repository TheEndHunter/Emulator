using Emulator._6502.Devices;

namespace Emulator._6502.CPU.Instructions
{
    public abstract class LDA : Instruction6502
    {
        protected LDA(AddrMode6502 mode) : base("LDA", mode, Status6502.Zero | Status6502.Negative)
        {
        }
    }

    public sealed class LDA_Immediate : LDA
    {
        public LDA_Immediate() : base(AddrMode6502.Immediate)
        {
        }

        public override byte Execute(Registers6502 registers, Bus6502 bus)
        {
            return 0;
        }
    }

    public sealed class LDA_IndexedIndirect : LDA
    {
        public LDA_IndexedIndirect() : base(AddrMode6502.IndexedIndirect)
        {
        }

        public override byte Execute(Registers6502 registers, Bus6502 bus)
        {
            return 0;
        }
    }
    public sealed class LDA_IndirectIndexed : LDA
    {
        public LDA_IndirectIndexed() : base(AddrMode6502.IndirectIndexed)
        {
        }

        public override byte Execute(Registers6502 registers, Bus6502 bus)
        {
            return 0;
        }
    }

    public sealed class LDA_ZeroPage : LDA
    {
        public LDA_ZeroPage() : base(AddrMode6502.ZeroPage)
        {
        }

        public override byte Execute(Registers6502 registers, Bus6502 bus)
        {
            return 0;
        }
    }
    public sealed class LDA_Absolute : LDA
    {
        public LDA_Absolute() : base(AddrMode6502.Absolute)
        {
        }

        public override byte Execute(Registers6502 registers, Bus6502 bus)
        {
            return 0;
        }
    }

    public sealed class LDA_ZeroPageX : LDA
    {
        public LDA_ZeroPageX() : base(AddrMode6502.ZeroPageX)
        {
        }

        public override byte Execute(Registers6502 registers, Bus6502 bus)
        {
            return 0;
        }
    }
    public sealed class LDA_AbsoluteX : LDA
    {
        public LDA_AbsoluteX() : base(AddrMode6502.AbsoluteX)
        {
        }

        public override byte Execute(Registers6502 registers, Bus6502 bus)
        {
            return 0;
        }
    }
    public sealed class LDA_AbsoluteY : LDA
    {
        public LDA_AbsoluteY() : base(AddrMode6502.AbsoluteY)
        {
        }

        public override byte Execute(Registers6502 registers, Bus6502 bus)
        {
            return 0;
        }
    }
}

