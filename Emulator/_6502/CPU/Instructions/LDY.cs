using Emulator._6502.Devices;

namespace Emulator._6502.CPU.Instructions
{
    public abstract class LDY : Instruction6502
    {
        protected LDY(byte bytesUsed, AddrMode6502 mode) : base("LDY", bytesUsed, mode, Status6502.Zero | Status6502.Negative)
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

    public sealed class LDY_Immediate : LDY
    {
        public LDY_Immediate() : base(2, AddrMode6502.Immediate)
        {
        }

        public override byte Execute(Registers6502 registers, Bus6502 bus)
        {
            registers.Y = bus.ReadByte(Immediate(registers, bus));
            SetFlags(registers, registers.Y);
            return 2;
        }
    }

    public sealed class LDY_ZeroPage : LDY
    {
        public LDY_ZeroPage() : base(2, AddrMode6502.ZeroPage)
        {
        }

        public override byte Execute(Registers6502 registers, Bus6502 bus)
        {
            registers.Y = bus.ReadByte(ZeroPage(registers, bus));
            SetFlags(registers, registers.Y);
            return 3;
        }
    }
    public sealed class LDY_Absolute : LDY
    {
        public LDY_Absolute() : base(3, AddrMode6502.Absolute)
        {
        }

        public override byte Execute(Registers6502 registers, Bus6502 bus)
        {
            registers.Y = bus.ReadByte(Absolute(registers, bus));
            SetFlags(registers, registers.Y);
            return 4;
        }
    }

    public sealed class LDY_ZeroPageX : LDY
    {
        public LDY_ZeroPageX() : base(2, AddrMode6502.ZeroPageX)
        {
        }

        public override byte Execute(Registers6502 registers, Bus6502 bus)
        {
            registers.Y = bus.ReadByte(ZeroPageX(registers, bus));
            SetFlags(registers, registers.Y);
            return 4;
        }
    }
    public sealed class LDY_AbsoluteX : LDY
    {
        public LDY_AbsoluteX() : base(3, AddrMode6502.AbsoluteX)
        {
        }

        public override byte Execute(Registers6502 registers, Bus6502 bus)
        {
            var (addr, clocks) = AbsoluteX(registers, bus);
            registers.Y = bus.ReadByte(addr);
            SetFlags(registers, registers.Y);
            return (byte)(4 + clocks);
        }
    }
}

