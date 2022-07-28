using Emulator._6502.Devices;

namespace Emulator._6502.CPU.Instructions
{
    public abstract class ROR : Instruction6502
    {
        protected ROR(byte bytesUsed, AddrMode6502 mode) : base("ROR", bytesUsed, mode, Status6502.Carry | Status6502.Zero | Status6502.Negative)
        {
        }
        protected static void SetFlags(Registers6502 registers, ushort fetched)
        {
            // The carry flag out exists in the high byte bit 0
            registers.SetFlag(Status6502.Carry, (fetched & 0xFF00) > 0);

            // The Zero flag is set if the result is 0
            registers.SetFlag(Status6502.Zero, fetched == 0x0000);

            // The negative flag is set to the most significant bit of low byte the result
            registers.SetFlag(Status6502.Negative, (fetched & 0x0080) > 0);
        }
    }

    public sealed class ROR_Accumulator : ROR
    {
        public ROR_Accumulator() : base(1, AddrMode6502.Accumulator)
        {
        }

        public override byte Execute(Registers6502 registers, Bus6502 bus)
        {
            var temp = (ushort)(((registers.GetFlag(Status6502.Carry) ? 1 : 0) << 7) | registers.A >> 1);
            SetFlags(registers, temp);
            registers.A = (byte)(temp & 0x00FF);
            return 2;
        }
    }

    public sealed class ROR_ZeroPage : ROR
    {
        public ROR_ZeroPage() : base(2, AddrMode6502.ZeroPage)
        {
        }

        public override byte Execute(Registers6502 registers, Bus6502 bus)
        {
            var addr = ZeroPage(registers, bus);
            var temp = (ushort)(((registers.GetFlag(Status6502.Carry) ? 1 : 0) << 7) | bus.ReadByte(addr) >> 1);
            SetFlags(registers, temp);
            bus.Write(addr, (byte)(temp & 0x00FF));
            return 5;
        }
    }
    public sealed class ROR_Absolute : ROR
    {
        public ROR_Absolute() : base(3, AddrMode6502.Absolute)
        {
        }

        public override byte Execute(Registers6502 registers, Bus6502 bus)
        {
            var addr = Absolute(registers, bus);
            var temp = (ushort)(((registers.GetFlag(Status6502.Carry) ? 1 : 0) << 7) | bus.ReadByte(addr) >> 1);
            SetFlags(registers, temp);
            bus.Write(addr, (byte)(temp & 0x00FF));
            return 6;
        }
    }

    public sealed class ROR_ZeroPageX : ROR
    {
        public ROR_ZeroPageX() : base(2, AddrMode6502.ZeroPageX)
        {
        }

        public override byte Execute(Registers6502 registers, Bus6502 bus)
        {
            var addr = ZeroPageX(registers, bus);
            var temp = (ushort)(((registers.GetFlag(Status6502.Carry) ? 1 : 0) << 7) | bus.ReadByte(addr) >> 1);
            SetFlags(registers, temp);
            bus.Write(addr, (byte)(temp & 0x00FF));
            return 6;
        }
    }
    public sealed class ROR_AbsoluteX : ROR
    {
        public ROR_AbsoluteX() : base(3, AddrMode6502.AbsoluteX)
        {
        }

        public override byte Execute(Registers6502 registers, Bus6502 bus)
        {
            var (addr, clocks) = AbsoluteX(registers, bus);
            var temp = (ushort)(((registers.GetFlag(Status6502.Carry) ? 1 : 0) << 7) | bus.ReadByte(addr) >> 1);
            SetFlags(registers, temp);
            bus.Write(addr, (byte)(temp & 0x00FF));
            return (byte)(7 + clocks);
        }
    }
}

