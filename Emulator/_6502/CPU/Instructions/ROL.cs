using Emulator._6502.Devices;

namespace Emulator._6502.CPU.Instructions
{
    public abstract class ROL : Instruction6502
    {
        protected ROL(byte bytesUsed, AddrMode6502 mode) : base("ROL", bytesUsed, mode, Status6502.Carry | Status6502.Zero | Status6502.Negative)
        {
        }
        protected static void SetFlags(ref Registers6502 registers, ushort fetched)
        {
            // The carry flag out exists in the high byte bit 0
            registers.SetFlag(Status6502.Carry, (fetched & 0xFF00) > 0);

            // The Zero flag is set if the result is 0
            registers.SetFlag(Status6502.Zero, fetched == 0x0000);

            // The negative flag is set to the most significant bit of low byte the result
            registers.SetFlag(Status6502.Negative, (fetched & 0x0080) > 0);
        }
    }

    public sealed class ROL_Accumulator : ROL
    {
        public ROL_Accumulator() : base(1, AddrMode6502.Accumulator)
        {
        }

        public override byte Execute(ref Registers6502 registers, Bus6502 bus)
        {
            var temp = (ushort)(registers.A << 1 | (registers.GetFlag(Status6502.Carry) ? 1 : 0));
            SetFlags(ref registers, temp);
            registers.A = (byte)(temp & 0x00FF);
            return 2;
        }
    }

    public sealed class ROL_ZeroPage : ROL
    {
        public ROL_ZeroPage() : base(2, AddrMode6502.ZeroPage)
        {
        }

        public override byte Execute(ref Registers6502 registers, Bus6502 bus)
        {
            var addr = ZeroPage(ref registers, bus);
            var temp = (ushort)(bus.ReadByte(addr) << 1 | (registers.GetFlag(Status6502.Carry) ? 1 : 0));
            SetFlags(ref registers, temp);
            bus.Write(addr, (byte)(temp & 0x00FF));
            return 5;
        }
    }
    public sealed class ROL_Absolute : ROL
    {
        public ROL_Absolute() : base(3, AddrMode6502.Absolute)
        {
        }

        public override byte Execute(ref Registers6502 registers, Bus6502 bus)
        {
            var addr = Absolute(ref registers, bus);
            var temp = (ushort)(bus.ReadByte(addr) << 1 | (registers.GetFlag(Status6502.Carry) ? 1 : 0));
            SetFlags(ref registers, temp);
            bus.Write(addr, (byte)(temp & 0x00FF));
            return 6;
        }
    }

    public sealed class ROL_ZeroPageX : ROL
    {
        public ROL_ZeroPageX() : base(2, AddrMode6502.ZeroPageX)
        {
        }

        public override byte Execute(ref Registers6502 registers, Bus6502 bus)
        {
            var addr = ZeroPageX(ref registers, bus);
            var temp = (ushort)(bus.ReadByte(addr) << 1 | (registers.GetFlag(Status6502.Carry) ? 1 : 0));
            SetFlags(ref registers, temp);
            bus.Write(addr, (byte)(temp & 0x00FF));
            return 6;
        }
    }
    public sealed class ROL_AbsoluteX : ROL
    {
        public ROL_AbsoluteX() : base(3, AddrMode6502.AbsoluteX)
        {
        }

        public override byte Execute(ref Registers6502 registers, Bus6502 bus)
        {
            var (addr, clocks) = AbsoluteX(ref registers, bus);
            var temp = (ushort)(bus.ReadByte(addr) << 1 | (registers.GetFlag(Status6502.Carry) ? 1 : 0));
            SetFlags(ref registers, temp);
            bus.Write(addr, (byte)(temp & 0x00FF));
            return (byte)(7 + clocks);
        }
    }
}

