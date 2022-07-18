using Emulator._6502.Devices;

namespace Emulator._6502.CPU.Instructions
{
    public abstract class STA : Instruction6502
    {
        protected STA(AddrMode6502 mode) : base("STA", mode, Status6502.None)
        {
        }
    }

    public sealed class STA_IndexedIndirect : STA
    {
        public STA_IndexedIndirect() : base(AddrMode6502.IndexedIndirect)
        {
        }

        public override byte Execute(Registers6502 registers, Bus6502 bus)
        {
            return 0;
        }
    }

    public sealed class STA_IndirectIndexed : STA
    {
        public STA_IndirectIndexed() : base(AddrMode6502.IndirectIndexed)
        {
        }

        public override byte Execute(Registers6502 registers, Bus6502 bus)
        {
            return 0;
        }
    }

    public sealed class STA_ZeroPage : STA
    {
        public STA_ZeroPage() : base(AddrMode6502.ZeroPage)
        {
        }

        public override byte Execute(Registers6502 registers, Bus6502 bus)
        {
            return 0;
        }
    }
    public sealed class STA_Absolute : STA
    {
        public STA_Absolute() : base(AddrMode6502.Absolute)
        {
        }

        public override byte Execute(Registers6502 registers, Bus6502 bus)
        {
            return 0;
        }
    }

    public sealed class STA_ZeroPageX : STA
    {
        public STA_ZeroPageX() : base(AddrMode6502.ZeroPageX)
        {
        }

        public override byte Execute(Registers6502 registers, Bus6502 bus)
        {
            return 0;
        }
    }
    public sealed class STA_AbsoluteX : STA
    {
        public STA_AbsoluteX() : base(AddrMode6502.AbsoluteX)
        {
        }

        public override byte Execute(Registers6502 registers, Bus6502 bus)
        {
            return 0;
        }
    }
    public sealed class STA_AbsoluteY : STA
    {
        public STA_AbsoluteY() : base(AddrMode6502.AbsoluteY)
        {
        }

        public override byte Execute(Registers6502 registers, Bus6502 bus)
        {
            return 0;
        }
    }
}

