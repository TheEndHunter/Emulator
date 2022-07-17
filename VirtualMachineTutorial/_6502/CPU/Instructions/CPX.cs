using Emulator._6502.Devices;

namespace Emulator._6502.CPU.Instructions
{
    public abstract class CPX : Instruction6502
    {
        protected CPX(AddrMode6502 mode) : base("CPX", mode, Status6502.Carry | Status6502.Zero | Status6502.Negative)
        {
        }
    }

    public sealed class CPX_Immediate : CPX
    {
        public CPX_Immediate() : base(AddrMode6502.Immediate)
        {

        }

        public override byte Execute(Registers6502 registers, Bus6502 bus)
        {
            return 0;
        }
    }
    public sealed class CPX_Absolute : CPX
    {
        public CPX_Absolute() : base(AddrMode6502.Absolute)
        {

        }

        public override byte Execute(Registers6502 registers, Bus6502 bus)
        {
            return 0;
        }
    }

    public sealed class CPX_ZeroPage : CPX
    {
        public CPX_ZeroPage() : base(AddrMode6502.ZeroPage)
        {

        }

        public override byte Execute(Registers6502 registers, Bus6502 bus)
        {
            return 0;
        }
    }
}
