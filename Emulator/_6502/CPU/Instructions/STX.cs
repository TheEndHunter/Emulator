using Emulator._6502.Devices;

namespace Emulator._6502.CPU.Instructions
{
    public abstract class STX : Instruction6502
    {
        protected STX(AddrMode6502 mode) : base("STX", mode, Status6502.None)
        {
        }
    }

    public sealed class STX_ZeroPage : STX
    {
        public STX_ZeroPage() : base(AddrMode6502.ZeroPage)
        {
        }

        public override byte Execute(Registers6502 registers, Bus6502 bus)
        {
            return 0;
        }
    }
    public sealed class STX_Absolute : STX
    {
        public STX_Absolute() : base(AddrMode6502.Absolute)
        {
        }

        public override byte Execute(Registers6502 registers, Bus6502 bus)
        {
            return 0;
        }
    }

    public sealed class STX_ZeroPageY : STX
    {
        public STX_ZeroPageY() : base(AddrMode6502.ZeroPageY)
        {
        }

        public override byte Execute(Registers6502 registers, Bus6502 bus)
        {
            return 0;
        }
    }
}

