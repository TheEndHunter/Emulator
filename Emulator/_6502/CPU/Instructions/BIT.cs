using Emulator._6502.Devices;

namespace Emulator._6502.CPU.Instructions
{
    public abstract class BIT : Instruction6502
    {
        protected BIT(AddrMode6502 mode) : base("BIT", mode, Status6502.Zero | Status6502.OverFlow | Status6502.Negative)
        {
        }
    }

    public sealed class BIT_ZeroPage : BIT
    {
        public BIT_ZeroPage() : base(AddrMode6502.ZeroPage)
        {
        }

        public override byte Execute(Registers6502 registers, Bus6502 bus)
        {
            return 0;
        }
    }
    public sealed class BIT_Absolute : BIT
    {
        public BIT_Absolute() : base(AddrMode6502.Absolute)
        {
        }

        public override byte Execute(Registers6502 registers, Bus6502 bus)
        {
            return 0;
        }
    }


}

