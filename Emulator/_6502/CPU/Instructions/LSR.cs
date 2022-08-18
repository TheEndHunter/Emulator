using Emulator._6502.Devices;

namespace Emulator._6502.CPU.Instructions
{
    public abstract class LSR : Instruction6502
    {
        protected LSR(byte bytesUsed, AddrMode6502 mode) : base("LSR", bytesUsed, mode, Status6502.Zero | Status6502.Negative)
        {
        }
        protected static void SetFlags(ref Registers6502 registers, byte data)
        {
            // The Zero flag is set if the result is 0
            registers.SetFlag(Status6502.Zero, data == 0x00);

            // The negative flag is set to the most significant bit of the result
            registers.SetFlag(Status6502.Negative, false);
        }
    }

    public sealed class LSR_Accumulator : LSR
    {
        public LSR_Accumulator() : base(1, AddrMode6502.Accumulator)
        {
        }

        public override byte Execute(ref Registers6502 registers, Bus6502 bus)
        {
            registers.SetFlag(Status6502.Carry, (registers.A & 0x01) > 0);
            registers.A = (byte)(registers.A >> 1);
            SetFlags(ref registers, registers.A);
            return 2;
        }
    }

    public sealed class LSR_ZeroPage : LSR
    {
        public LSR_ZeroPage() : base(2, AddrMode6502.ZeroPage)
        {
        }

        public override byte Execute(ref Registers6502 registers, Bus6502 bus)
        {
            var addr = ZeroPage(ref registers, bus);
            var d = bus.ReadByte(addr);
            registers.SetFlag(Status6502.Carry, (d & 0x01) > 0);
            d = (byte)(d >> 1);
            SetFlags(ref registers, d);
            bus.Write(addr, d);
            return 5;
        }
    }
    public sealed class LSR_Absolute : LSR
    {
        public LSR_Absolute() : base(3, AddrMode6502.Absolute)
        {
        }

        public override byte Execute(ref Registers6502 registers, Bus6502 bus)
        {
            var addr = Absolute(ref registers, bus);
            var d = bus.ReadByte(addr);
            registers.SetFlag(Status6502.Carry, (d & 0x01) > 0);
            d = (byte)(d >> 1);
            SetFlags(ref registers, d);
            bus.Write(addr, d);
            return 6;
        }
    }

    public sealed class LSR_ZeroPageX : LSR
    {
        public LSR_ZeroPageX() : base(2, AddrMode6502.ZeroPageX)
        {
        }

        public override byte Execute(ref Registers6502 registers, Bus6502 bus)
        {
            var addr = ZeroPageX(ref registers, bus);
            var d = bus.ReadByte(addr);
            registers.SetFlag(Status6502.Carry, (d & 0x01) > 0);
            d = (byte)(d >> 1);
            SetFlags(ref registers, d);
            bus.Write(addr, d);
            return 6;
        }
    }
    public sealed class LSR_AbsoluteX : LSR
    {
        public LSR_AbsoluteX() : base(3, AddrMode6502.AbsoluteX)
        {
        }

        public override byte Execute(ref Registers6502 registers, Bus6502 bus)
        {
            var addr = AbsoluteX(ref registers, bus).addr;
            var d = bus.ReadByte(addr);
            registers.SetFlag(Status6502.Carry, (d & 0x01) > 0);
            d = (byte)(d >> 1);
            SetFlags(ref registers, d);
            bus.Write(addr, d);
            return 7;
        }
    }
}

