using Emulator._6502.Devices;

namespace Emulator._6502.CPU.Instructions
{
    public abstract class ADC : Instruction6502
    {
        protected ADC(AddrMode6502 mode) : base("ADC", mode, Status6502.Carry | Status6502.Zero | Status6502.OverFlow | Status6502.Negative)
        {
        }
    }

    public sealed class ADC_ZeroPage : ADC
    {
        public ADC_ZeroPage() : base(AddrMode6502.ZeroPage)
        {
        }

        public override byte Execute(Registers6502 registers, Bus6502 bus)
        {
            return 0;
        }
    }

    public sealed class ADC_ZeroPageX : ADC
    {
        public ADC_ZeroPageX() : base(AddrMode6502.ZeroPageX)
        {
        }

        public override byte Execute(Registers6502 registers, Bus6502 bus)
        {
            return 0;
        }
    }
    public sealed class ADC_Absolute : ADC
    {
        public ADC_Absolute() : base(AddrMode6502.Absolute)
        {
        }

        public override byte Execute(Registers6502 registers, Bus6502 bus)
        {
            return 0;
        }
    }

    public sealed class ADC_IndirectIndexed : ADC
    {
        public ADC_IndirectIndexed() : base(AddrMode6502.IndirectIndexed)
        {
        }

        public override byte Execute(Registers6502 registers, Bus6502 bus)
        {
            return 0;
        }
    }

    public sealed class ADC_IndexedIndirect : ADC
    {
        public ADC_IndexedIndirect() : base(AddrMode6502.IndexedIndirect)
        {
        }

        public override byte Execute(Registers6502 registers, Bus6502 bus)
        {
            return 0;
        }
    }

    public sealed class ADC_AbsoluteX : ADC
    {
        public ADC_AbsoluteX() : base(AddrMode6502.AbsoluteX)
        {
        }

        public override byte Execute(Registers6502 registers, Bus6502 bus)
        {
            return 0;
        }
    }
    public sealed class ADC_AbsoluteY : ADC
    {
        public ADC_AbsoluteY() : base(AddrMode6502.AbsoluteY)
        {
        }

        public override byte Execute(Registers6502 registers, Bus6502 bus)
        {
            return 0;
        }
    }
}
