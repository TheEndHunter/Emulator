using Emulator._6502.Devices;

namespace Emulator._6502.CPU.Instructions
{
    public abstract class AND : Instruction6502
    {
        protected AND(byte bytesUsed, AddrMode6502 mode) : base("AND", bytesUsed, mode, Status6502.Zero | Status6502.Negative)
        {
        }
        protected static void SetFlags(Registers6502 registers, byte data)
        {
            // The Zero flag is set if the result is 0
            registers.SetFlag(Status6502.Zero, data == 0x00);


            // The negative flag is set to the most significant bit of the result
            registers.SetFlag(Status6502.Negative, (data & 0x80) > 0);
        }
    }

    public sealed class AND_Immediate : AND
    {
        public AND_Immediate() : base(2, AddrMode6502.Immediate)
        {
        }

        public override byte Execute(Registers6502 registers, Bus6502 bus)
        {
            var temp = registers.A &= bus.ReadByte(Immediate(registers, bus));
            SetFlags(registers, temp);
            return 2;
        }
    }

    public sealed class AND_ZeroPage : AND
    {
        public AND_ZeroPage() : base(2, AddrMode6502.ZeroPage)
        {
        }

        public override byte Execute(Registers6502 registers, Bus6502 bus)
        {
            var temp = registers.A &= bus.ReadByte(ZeroPage(registers, bus));
            SetFlags(registers, temp);
            return 3;
        }
    }

    public sealed class AND_ZeroPageX : AND
    {
        public AND_ZeroPageX() : base(2, AddrMode6502.ZeroPageX)
        {
        }

        public override byte Execute(Registers6502 registers, Bus6502 bus)
        {
            var temp = registers.A &= bus.ReadByte(ZeroPageX(registers, bus));
            SetFlags(registers, temp);
            return 4;
        }
    }
    public sealed class AND_Absolute : AND
    {
        public AND_Absolute() : base(3, AddrMode6502.Absolute)
        {
        }

        public override byte Execute(Registers6502 registers, Bus6502 bus)
        {
            var temp = registers.A &= bus.ReadByte(Absolute(registers, bus));
            SetFlags(registers, temp);
            return 4;
        }
    }

    public sealed class AND_IndirectIndexed : AND
    {
        public AND_IndirectIndexed() : base(2, AddrMode6502.IndirectIndexed)
        {
        }

        public override byte Execute(Registers6502 registers, Bus6502 bus)
        {
            var temp = registers.A &= bus.ReadByte(IndirectIndex(registers, bus));
            SetFlags(registers, temp);
            return 6;
        }
    }

    public sealed class AND_IndexedIndirect : AND
    {
        public AND_IndexedIndirect() : base(2, AddrMode6502.IndexedIndirect)
        {
        }

        public override byte Execute(Registers6502 registers, Bus6502 bus)
        {
            var (addr, clocks) = IndexIndirect(registers, bus);
            var temp = registers.A &= bus.ReadByte(addr);
            SetFlags(registers, temp);
            return (byte)(5 + clocks);
        }
    }

    public sealed class AND_AbsoluteX : AND
    {
        public AND_AbsoluteX() : base(3, AddrMode6502.AbsoluteX)
        {
        }

        public override byte Execute(Registers6502 registers, Bus6502 bus)
        {
            var (addr, clocks) = AbsoluteX(registers, bus);
            var temp = registers.A &= bus.ReadByte(addr);
            SetFlags(registers, temp);
            return (byte)(4 + clocks);
        }
    }
    public sealed class AND_AbsoluteY : AND
    {
        public AND_AbsoluteY() : base(3, AddrMode6502.AbsoluteY)
        {
        }

        public override byte Execute(Registers6502 registers, Bus6502 bus)
        {
            var (addr, clocks) = AbsoluteY(registers, bus);
            var temp = registers.A &= bus.ReadByte(addr);
            SetFlags(registers, temp);
            return (byte)(4 + clocks);
        }
    }
}
