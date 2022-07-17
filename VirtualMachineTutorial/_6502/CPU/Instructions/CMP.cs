using Emulator._6502.Devices;

namespace Emulator._6502.CPU.Instructions
{
    public abstract class CMP : Instruction6502
    {
        protected CMP(AddrMode6502 mode) : base("CMP", mode, Status6502.Carry | Status6502.Zero | Status6502.Negative)
        {
        }
    }

    public sealed class CMP_IndexedIndirect : CMP
    {
        public CMP_IndexedIndirect() : base(AddrMode6502.IndexedIndirect)
        {

        }

        public override byte Execute(Registers6502 registers, Bus6502 bus)
        {
            return 0;
        }
    }
    public sealed class CMP_IndirectIndexed : CMP
    {
        public CMP_IndirectIndexed() : base(AddrMode6502.IndirectIndexed)
        {

        }

        public override byte Execute(Registers6502 registers, Bus6502 bus)
        {
            return 0;
        }
    }

    public sealed class CMP_ZeroPage : CMP
    {
        public CMP_ZeroPage() : base(AddrMode6502.ZeroPage)
        {

        }

        public override byte Execute(Registers6502 registers, Bus6502 bus)
        {
            return 0;
        }
    }

    public sealed class CMP_Immediate : CMP
    {
        public CMP_Immediate() : base(AddrMode6502.Immediate)
        {

        }

        public override byte Execute(Registers6502 registers, Bus6502 bus)
        {
            return 0;
        }
    }

    public sealed class CMP_Absolute : CMP
    {
        public CMP_Absolute() : base(AddrMode6502.Absolute)
        {

        }

        public override byte Execute(Registers6502 registers, Bus6502 bus)
        {
            return 0;
        }
    }

    public sealed class CMP_ZeroPageX : CMP
    {
        public CMP_ZeroPageX() : base(AddrMode6502.ZeroPageX)
        {

        }

        public override byte Execute(Registers6502 registers, Bus6502 bus)
        {
            return 0;
        }
    }

    public sealed class CMP_AbsoluteY : CMP
    {
        public CMP_AbsoluteY() : base(AddrMode6502.AbsoluteY)
        {

        }

        public override byte Execute(Registers6502 registers, Bus6502 bus)
        {
            return 0;
        }
    }

    public sealed class CMP_AbsoluteX : CMP
    {
        public CMP_AbsoluteX() : base(AddrMode6502.AbsoluteX)
        {

        }

        public override byte Execute(Registers6502 registers, Bus6502 bus)
        {
            return 0;
        }
    }
}
