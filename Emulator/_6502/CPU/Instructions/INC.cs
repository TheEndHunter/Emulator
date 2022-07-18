using Emulator._6502.Devices;

namespace Emulator._6502.CPU.Instructions
{
    public abstract class INC : Instruction6502
    {
        protected INC(AddrMode6502 mode) : base("INC", mode, Status6502.Zero | Status6502.Negative)
        {
        }
    }

    public sealed class INC_ZeroPage : INC
    {
        public INC_ZeroPage() : base(AddrMode6502.ZeroPage)
        {
        }

        public override byte Execute(Registers6502 registers, Bus6502 bus)
        {
            return 0;
        }
    }
    public sealed class INC_Absolute : INC
    {
        public INC_Absolute() : base(AddrMode6502.Absolute)
        {
        }

        public override byte Execute(Registers6502 registers, Bus6502 bus)
        {
            return 0;
        }
    }

    public sealed class INC_ZeroPageX : INC
    {
        public INC_ZeroPageX() : base(AddrMode6502.ZeroPageX)
        {
        }

        public override byte Execute(Registers6502 registers, Bus6502 bus)
        {
            return 0;
        }
    }
    public sealed class INC_AbsoluteX : INC
    {
        public INC_AbsoluteX() : base(AddrMode6502.AbsoluteX)
        {
        }

        public override byte Execute(Registers6502 registers, Bus6502 bus)
        {
            return 0;
        }
    }
}

