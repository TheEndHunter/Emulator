using Emulator._6502.Devices;

namespace Emulator._6502.CPU.Instructions
{
    public abstract class LDX : Instruction6502
    {
        protected LDX(AddrMode6502 mode) : base("LDX", mode, Status6502.Zero | Status6502.Negative)
        {
        }
    }

    public sealed class LDX_Immediate : LDX
    {
        public LDX_Immediate() : base(AddrMode6502.Immediate)
        {
        }

        public override byte Execute(Registers6502 registers, Bus6502 bus)
        {
            return 0;
        }
    }

    public sealed class LDX_ZeroPage : LDX
    {
        public LDX_ZeroPage() : base(AddrMode6502.ZeroPage)
        {
        }

        public override byte Execute(Registers6502 registers, Bus6502 bus)
        {
            return 0;
        }
    }
    public sealed class LDX_Absolute : LDX
    {
        public LDX_Absolute() : base(AddrMode6502.Absolute)
        {
        }

        public override byte Execute(Registers6502 registers, Bus6502 bus)
        {
            return 0;
        }
    }

    public sealed class LDX_ZeroPageY : LDX
    {
        public LDX_ZeroPageY() : base(AddrMode6502.ZeroPageY)
        {
        }

        public override byte Execute(Registers6502 registers, Bus6502 bus)
        {
            return 0;
        }
    }
    public sealed class LDX_AbsoluteY : LDX
    {
        public LDX_AbsoluteY() : base(AddrMode6502.AbsoluteY)
        {
        }

        public override byte Execute(Registers6502 registers, Bus6502 bus)
        {
            return 0;
        }
    }
}

