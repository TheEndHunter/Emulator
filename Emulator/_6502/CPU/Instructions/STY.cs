using Emulator._6502.Devices;

namespace Emulator._6502.CPU.Instructions
{
    public abstract class STY : Instruction6502
    {
        protected STY(AddrMode6502 mode) : base("STY", mode, Status6502.None)
        {
        }
    }

    public sealed class STY_ZeroPage : STY
    {
        public STY_ZeroPage() : base(AddrMode6502.ZeroPage)
        {
        }

        public override byte Execute(Registers6502 registers, Bus6502 bus)
        {
            return 0;
        }
    }
    public sealed class STY_Absolute : STY
    {
        public STY_Absolute() : base(AddrMode6502.Absolute)
        {
        }

        public override byte Execute(Registers6502 registers, Bus6502 bus)
        {
            return 0;
        }
    }

    public sealed class STY_ZeroPageX : STY
    {
        public STY_ZeroPageX() : base(AddrMode6502.ZeroPageX)
        {
        }

        public override byte Execute(Registers6502 registers, Bus6502 bus)
        {
            return 0;
        }
    }
}

