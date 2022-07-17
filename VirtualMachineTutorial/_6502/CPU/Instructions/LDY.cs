using Emulator._6502.Devices;

namespace Emulator._6502.CPU.Instructions
{
    public abstract class LDY : Instruction6502
    {
        protected LDY(AddrMode6502 mode) : base("LDY", mode, Status6502.Zero | Status6502.Negative)
        {
        }
    }

    public sealed class LDY_Immediate : LDY
    {
        public LDY_Immediate() : base(AddrMode6502.Immediate)
        {
        }

        public override byte Execute(Registers6502 registers, Bus6502 bus)
        {
            return 0;
        }
    }

    public sealed class LDY_ZeroPage : LDY
    {
        public LDY_ZeroPage() : base(AddrMode6502.ZeroPage)
        {
        }

        public override byte Execute(Registers6502 registers, Bus6502 bus)
        {
            return 0;
        }
    }
    public sealed class LDY_Absolute : LDY
    {
        public LDY_Absolute() : base(AddrMode6502.Absolute)
        {
        }

        public override byte Execute(Registers6502 registers, Bus6502 bus)
        {
            return 0;
        }
    }

    public sealed class LDY_ZeroPageX : LDY
    {
        public LDY_ZeroPageX() : base(AddrMode6502.ZeroPageX)
        {
        }

        public override byte Execute(Registers6502 registers, Bus6502 bus)
        {
            return 0;
        }
    }
    public sealed class LDY_AbsoluteX : LDY
    {
        public LDY_AbsoluteX() : base(AddrMode6502.AbsoluteX)
        {
        }

        public override byte Execute(Registers6502 registers, Bus6502 bus)
        {
            return 0;
        }
    }
}

