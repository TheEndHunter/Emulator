using Emulator._6502.Devices;

namespace Emulator._6502.CPU.Instructions
{
    public abstract class AND : Instruction6502
    {
        protected AND(byte bytesUsed, AddrMode6502 mode) : base("AND", bytesUsed, mode, Status6502.Zero | Status6502.Negative)
        {
        }
        protected static void SetFlags(ref Registers6502 registers, byte data)
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

        public override byte Execute(ref Registers6502 registers, Bus6502 bus)
        {
            var temp = registers.A &= bus.ReadByte(Immediate(ref registers, bus));
            SetFlags(ref registers, temp);
            return 2;
        }
    }

    public sealed class AND_ZeroPage : AND
    {
        public AND_ZeroPage() : base(2, AddrMode6502.ZeroPage)
        {
        }

        public override byte Execute(ref Registers6502 registers, Bus6502 bus)
        {
            var temp = registers.A &= bus.ReadByte(ZeroPage(ref registers, bus));
            SetFlags(ref registers, temp);
            return 3;
        }
    }

    public sealed class AND_ZeroPageX : AND
    {
        public AND_ZeroPageX() : base(2, AddrMode6502.ZeroPageX)
        {
        }

        public override byte Execute(ref Registers6502 registers, Bus6502 bus)
        {
            var temp = registers.A &= bus.ReadByte(ZeroPageX(ref registers, bus));
            SetFlags(ref registers, temp);
            return 4;
        }
    }
    public sealed class AND_Absolute : AND
    {
        public AND_Absolute() : base(3, AddrMode6502.Absolute)
        {
        }

        public override byte Execute(ref Registers6502 registers, Bus6502 bus)
        {
            var temp = registers.A &= bus.ReadByte(Absolute(ref registers, bus));
            SetFlags(ref registers, temp);
            return 4;
        }
    }

    public sealed class AND_IndirectIndexed : AND
    {
        public AND_IndirectIndexed() : base(2, AddrMode6502.IndirectIndexed)
        {
        }

        public override byte Execute(ref Registers6502 registers, Bus6502 bus)
        {
            var temp = registers.A &= bus.ReadByte(IndirectIndex(ref registers, bus));
            SetFlags(ref registers, temp);
            return 6;
        }
    }

    public sealed class AND_IndexedIndirect : AND
    {
        public AND_IndexedIndirect() : base(2, AddrMode6502.IndexedIndirect)
        {
        }

        public override byte Execute(ref Registers6502 registers, Bus6502 bus)
        {
            var (addr, clocks) = IndexIndirect(ref registers, bus);
            var temp = registers.A &= bus.ReadByte(addr);
            SetFlags(ref registers, temp);
            return (byte)(5 + clocks);
        }
    }

    public sealed class AND_AbsoluteX : AND
    {
        public AND_AbsoluteX() : base(3, AddrMode6502.AbsoluteX)
        {
        }

        public override byte Execute(ref Registers6502 registers, Bus6502 bus)
        {
            var (addr, clocks) = AbsoluteX(ref registers, bus);
            var temp = registers.A &= bus.ReadByte(addr);
            SetFlags(ref registers, temp);
            return (byte)(4 + clocks);
        }
    }
    public sealed class AND_AbsoluteY : AND
    {
        public AND_AbsoluteY() : base(3, AddrMode6502.AbsoluteY)
        {
        }

        public override byte Execute(ref Registers6502 registers, Bus6502 bus)
        {
            var (addr, clocks) = AbsoluteY(ref registers, bus);
            var temp = registers.A &= bus.ReadByte(addr);
            SetFlags(ref registers, temp);
            return (byte)(4 + clocks);
        }
    }
}
