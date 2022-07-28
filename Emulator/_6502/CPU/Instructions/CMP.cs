using Emulator._6502.Devices;

namespace Emulator._6502.CPU.Instructions
{
    public abstract class CMP : Instruction6502
    {
        protected CMP(byte bytesUsed, AddrMode6502 mode) : base("CMP", bytesUsed, mode, Status6502.Carry | Status6502.Zero | Status6502.Negative)
        {
        }
        protected static void SetFlags(Registers6502 registers, byte fetched)
        {
            var temp = registers.A - fetched;
            // The carry flag out exists in the high byte bit 0
            registers.SetFlag(Status6502.Carry, registers.A >= fetched);

            // The Zero flag is set if the result is 0
            registers.SetFlag(Status6502.Zero, (temp & 0x00FF) == 0x0000);

            // The negative flag is set to the most significant bit of low byte the result
            registers.SetFlag(Status6502.Negative, (temp & 0x0080) > 0);
        }
    }

    public sealed class CMP_IndexedIndirect : CMP
    {
        public CMP_IndexedIndirect() : base(2, AddrMode6502.IndexedIndirect)
        {

        }

        public override byte Execute(Registers6502 registers, Bus6502 bus)
        {
            var (addr, clocks) = IndexIndirect(registers, bus);
            SetFlags(registers, bus.ReadByte(addr));
            return (byte)(5 + clocks);
        }
    }
    public sealed class CMP_IndirectIndexed : CMP
    {
        public CMP_IndirectIndexed() : base(2, AddrMode6502.IndirectIndexed)
        {

        }

        public override byte Execute(Registers6502 registers, Bus6502 bus)
        {
            SetFlags(registers, bus.ReadByte(IndirectIndex(registers, bus)));
            return 6;
        }
    }

    public sealed class CMP_ZeroPage : CMP
    {
        public CMP_ZeroPage() : base(2, AddrMode6502.ZeroPage)
        {

        }

        public override byte Execute(Registers6502 registers, Bus6502 bus)
        {
            SetFlags(registers, bus.ReadByte(ZeroPage(registers, bus)));
            return 3;
        }
    }

    public sealed class CMP_Immediate : CMP
    {
        public CMP_Immediate() : base(2, AddrMode6502.Immediate)
        {

        }

        public override byte Execute(Registers6502 registers, Bus6502 bus)
        {
            SetFlags(registers, bus.ReadByte(Immediate(registers, bus)));
            return 2;
        }
    }

    public sealed class CMP_Absolute : CMP
    {
        public CMP_Absolute() : base(3, AddrMode6502.Absolute)
        {

        }

        public override byte Execute(Registers6502 registers, Bus6502 bus)
        {
            SetFlags(registers, bus.ReadByte(Absolute(registers, bus)));
            return 4;
        }
    }

    public sealed class CMP_ZeroPageX : CMP
    {
        public CMP_ZeroPageX() : base(2, AddrMode6502.ZeroPageX)
        {

        }

        public override byte Execute(Registers6502 registers, Bus6502 bus)
        {
            SetFlags(registers, bus.ReadByte(ZeroPageX(registers, bus)));
            return 4;
        }
    }

    public sealed class CMP_AbsoluteY : CMP
    {
        public CMP_AbsoluteY() : base(3, AddrMode6502.AbsoluteY)
        {

        }

        public override byte Execute(Registers6502 registers, Bus6502 bus)
        {
            var (addr, clocks) = AbsoluteX(registers, bus);
            SetFlags(registers, bus.ReadByte(addr));
            return (byte)(4 + clocks);
        }
    }

    public sealed class CMP_AbsoluteX : CMP
    {
        public CMP_AbsoluteX() : base(3, AddrMode6502.AbsoluteX)
        {

        }

        public override byte Execute(Registers6502 registers, Bus6502 bus)
        {
            var (addr, clocks) = AbsoluteY(registers, bus);
            SetFlags(registers, bus.ReadByte(addr));
            return (byte)(4 + clocks);
        }
    }
}
