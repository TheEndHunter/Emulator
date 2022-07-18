using Emulator._6502.Devices;

namespace Emulator._6502.CPU.Instructions
{
    public abstract class EOR : Instruction6502
    {
        protected EOR(AddrMode6502 mode) : base("EOR", mode, Status6502.Zero | Status6502.Negative)
        {
        }
    }
    public sealed class EOR_Immediate : EOR
    {
        public EOR_Immediate() : base(AddrMode6502.Immediate)
        {
        }

        public override byte Execute(Registers6502 registers, Bus6502 bus)
        {
            return 0;
        }
    }
    public sealed class EOR_IndirectIndexed : EOR
    {
        public EOR_IndirectIndexed() : base(AddrMode6502.IndirectIndexed)
        {
        }

        public override byte Execute(Registers6502 registers, Bus6502 bus)
        {
            return 0;
        }
    }

    public sealed class EOR_IndexedIndirect : EOR
    {
        public EOR_IndexedIndirect() : base(AddrMode6502.IndexedIndirect)
        {
        }

        public override byte Execute(Registers6502 registers, Bus6502 bus)
        {
            return 0;
        }
    }

    public sealed class EOR_ZeroPage : EOR
    {
        public EOR_ZeroPage() : base(AddrMode6502.ZeroPage)
        {
        }

        public override byte Execute(Registers6502 registers, Bus6502 bus)
        {
            return 0;
        }
    }
    public sealed class EOR_Absolute : EOR
    {
        public EOR_Absolute() : base(AddrMode6502.Absolute)
        {
        }

        public override byte Execute(Registers6502 registers, Bus6502 bus)
        {
            return 0;
        }
    }

    public sealed class EOR_ZeroPageX : EOR
    {
        public EOR_ZeroPageX() : base(AddrMode6502.ZeroPageX)
        {
        }

        public override byte Execute(Registers6502 registers, Bus6502 bus)
        {
            return 0;
        }
    }
    public sealed class EOR_AbsoluteX : EOR
    {
        public EOR_AbsoluteX() : base(AddrMode6502.AbsoluteX)
        {
        }

        public override byte Execute(Registers6502 registers, Bus6502 bus)
        {
            return 0;
        }
    }
    public sealed class EOR_AbsoluteY : EOR
    {
        public EOR_AbsoluteY() : base(AddrMode6502.AbsoluteY)
        {
        }

        public override byte Execute(Registers6502 registers, Bus6502 bus)
        {
            return 0;
        }
    }
}

