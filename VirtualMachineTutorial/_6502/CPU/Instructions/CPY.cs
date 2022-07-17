using Emulator._6502.Devices;

namespace Emulator._6502.CPU.Instructions
{
    public abstract class CPY : Instruction6502
    {
        protected CPY(AddrMode6502 mode) : base("CPY", mode, Status6502.Carry | Status6502.Zero | Status6502.Negative)
        {
        }
    }

    public sealed class CPY_Immediate : CPY
    {
        public CPY_Immediate() : base(AddrMode6502.Immediate)
        {

        }

        public override byte Execute(Registers6502 registers, Bus6502 bus)
        {
            return 0;
        }
    }
    public sealed class CPY_Absolute : CPY
    {
        public CPY_Absolute() : base(AddrMode6502.Absolute)
        {

        }

        public override byte Execute(Registers6502 registers, Bus6502 bus)
        {
            return 0;
        }
    }

    public sealed class CPY_ZeroPage : CPY
    {
        public CPY_ZeroPage() : base(AddrMode6502.ZeroPage)
        {

        }

        public override byte Execute(Registers6502 registers, Bus6502 bus)
        {
            return 0;
        }
    }
}
