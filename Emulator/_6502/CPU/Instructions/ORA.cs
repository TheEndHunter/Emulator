using Emulator._6502.Devices;

namespace Emulator._6502.CPU.Instructions
{
    public abstract class ORA : Instruction6502
    {
        protected ORA(AddrMode6502 mode) : base("ORA", mode, Status6502.Zero | Status6502.Negative)
        {
        }
    }
    public sealed class ORA_IndexedIndirect : ORA
    {
        public ORA_IndexedIndirect() : base(AddrMode6502.IndexedIndirect)
        {

        }
        public override byte Execute(Registers6502 registers, Bus6502 bus)
        {
            return 0;
        }
    }
    public sealed class ORA_ZeroPage : ORA
    {
        public ORA_ZeroPage() : base(AddrMode6502.ZeroPage)
        {
        }
        public override byte Execute(Registers6502 registers, Bus6502 bus)
        {
            return 0;
        }
    }
    public sealed class ORA_Immediate : ORA
    {
        public ORA_Immediate() : base(AddrMode6502.Immediate)
        {

        }
        public override byte Execute(Registers6502 registers, Bus6502 bus)
        {
            return 0;
        }
    }
    public sealed class ORA_Absolute : ORA
    {
        public ORA_Absolute() : base(AddrMode6502.Absolute)
        {

        }
        public override byte Execute(Registers6502 registers, Bus6502 bus)
        {
            return 0;
        }
    }
    public sealed class ORA_IndirectIndexed : ORA
    {
        public ORA_IndirectIndexed() : base(AddrMode6502.IndirectIndexed)
        {

        }
        public override byte Execute(Registers6502 registers, Bus6502 bus)
        {
            return 0;
        }
    }
    public sealed class ORA_AbsoluteY : ORA
    {
        public ORA_AbsoluteY() : base(AddrMode6502.AbsoluteY)
        {

        }
        public override byte Execute(Registers6502 registers, Bus6502 bus)
        {
            return 0;
        }
    }
    public sealed class ORA_AbsoluteX : ORA
    {
        public ORA_AbsoluteX() : base(AddrMode6502.AbsoluteX)
        {

        }
        public override byte Execute(Registers6502 registers, Bus6502 bus)
        {
            return 0;
        }
    }
    public sealed class ORA_ZeroPageX : ORA
    {
        public ORA_ZeroPageX() : base(AddrMode6502.ZeroPageX)
        {

        }
        public override byte Execute(Registers6502 registers, Bus6502 bus)
        {
            return 0;
        }
    }
}
