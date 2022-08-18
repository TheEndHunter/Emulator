using Emulator._6502.Devices;

namespace Emulator._6502.CPU.Instructions
{
    public abstract class LDA : Instruction6502
    {
        protected LDA(byte bytesUsed, AddrMode6502 mode) : base("LDA", bytesUsed, mode, Status6502.Zero | Status6502.Negative)
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

    public sealed class LDA_Immediate : LDA
    {
        public LDA_Immediate() : base(2, AddrMode6502.Immediate)
        {
        }

        public override byte Execute(ref Registers6502 registers, Bus6502 bus)
        {
            registers.A = bus.ReadByte(Immediate(ref registers, bus));
            SetFlags(ref registers, registers.A);
            return 2;
        }
    }

    public sealed class LDA_IndexedIndirect : LDA
    {
        public LDA_IndexedIndirect() : base(2, AddrMode6502.IndexedIndirect)
        {
        }

        public override byte Execute(ref Registers6502 registers, Bus6502 bus)
        {
            var (addr, clocks) = IndexIndirect(ref registers, bus);
            registers.A = bus.ReadByte(addr);
            SetFlags(ref registers, registers.A);
            return (byte)(5 + clocks);
        }
    }
    public sealed class LDA_IndirectIndexed : LDA
    {
        public LDA_IndirectIndexed() : base(2, AddrMode6502.IndirectIndexed)
        {
        }

        public override byte Execute(ref Registers6502 registers, Bus6502 bus)
        {
            registers.A = bus.ReadByte(IndirectIndex(ref registers, bus));
            SetFlags(ref registers, registers.A);
            return 6;
        }
    }

    public sealed class LDA_ZeroPage : LDA
    {
        public LDA_ZeroPage() : base(2, AddrMode6502.ZeroPage)
        {
        }

        public override byte Execute(ref Registers6502 registers, Bus6502 bus)
        {
            registers.A = bus.ReadByte(ZeroPage(ref registers, bus));
            SetFlags(ref registers, registers.A);
            return 3;
        }
    }
    public sealed class LDA_Absolute : LDA
    {
        public LDA_Absolute() : base(3, AddrMode6502.Absolute)
        {
        }

        public override byte Execute(ref Registers6502 registers, Bus6502 bus)
        {
            registers.A = bus.ReadByte(Absolute(ref registers, bus));
            SetFlags(ref registers, registers.A);
            return 4;
        }
    }

    public sealed class LDA_ZeroPageX : LDA
    {
        public LDA_ZeroPageX() : base(2, AddrMode6502.ZeroPageX)
        {
        }

        public override byte Execute(ref Registers6502 registers, Bus6502 bus)
        {
            registers.A = bus.ReadByte(ZeroPageX(ref registers, bus));
            SetFlags(ref registers, registers.A);
            return 4;
        }
    }
    public sealed class LDA_AbsoluteX : LDA
    {
        public LDA_AbsoluteX() : base(3, AddrMode6502.AbsoluteX)
        {
        }

        public override byte Execute(ref Registers6502 registers, Bus6502 bus)
        {
            var (addr, clocks) = AbsoluteX(ref registers, bus);
            registers.A = bus.ReadByte(addr);
            SetFlags(ref registers, registers.A);
            return (byte)(4 + clocks);
        }
    }
    public sealed class LDA_AbsoluteY : LDA
    {
        public LDA_AbsoluteY() : base(3, AddrMode6502.AbsoluteY)
        {
        }

        public override byte Execute(ref Registers6502 registers, Bus6502 bus)
        {
            var (addr, clocks) = AbsoluteY(ref registers, bus);
            registers.A = bus.ReadByte(addr);
            SetFlags(ref registers, registers.A);
            return (byte)(4 + clocks);
        }
    }
}

