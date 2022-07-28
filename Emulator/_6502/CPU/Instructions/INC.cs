using Emulator._6502.Devices;

namespace Emulator._6502.CPU.Instructions
{
    public abstract class INC : Instruction6502
    {
        protected INC(byte bytesUsed, AddrMode6502 mode) : base("INC", bytesUsed, mode, Status6502.Zero | Status6502.Negative)
        {
        }
        protected static void SetFlags(Registers6502 registers, ushort data)
        {
            // The Zero flag is set if the result is 0
            registers.SetFlag(Status6502.Zero, (data & 0x00FF) == 0);

            // The negative flag is set to the most significant bit of the result
            registers.SetFlag(Status6502.Negative, (data & 0x80) > 0);
        }
    }

    public sealed class INC_ZeroPage : INC
    {
        public INC_ZeroPage() : base(2, AddrMode6502.ZeroPage)
        {
        }

        public override byte Execute(Registers6502 registers, Bus6502 bus)
        {
            var addr = ZeroPage(registers, bus);
            var t = (ushort)(bus.ReadByte(addr) + 1);
            bus.Write(addr, (byte)(t & 0x00FF));
            SetFlags(registers, t);
            return 5;
        }
    }
    public sealed class INC_Absolute : INC
    {
        public INC_Absolute() : base(3, AddrMode6502.Absolute)
        {
        }

        public override byte Execute(Registers6502 registers, Bus6502 bus)
        {
            var addr = Absolute(registers, bus);
            var t = (ushort)(bus.ReadByte(addr) + 1);
            bus.Write(addr, (byte)(t & 0x00FF));
            SetFlags(registers, t);
            return 6;
        }
    }

    public sealed class INC_ZeroPageX : INC
    {
        public INC_ZeroPageX() : base(2, AddrMode6502.ZeroPageX)
        {
        }

        public override byte Execute(Registers6502 registers, Bus6502 bus)
        {
            var addr = ZeroPageX(registers, bus);
            var t = (ushort)(bus.ReadByte(addr) + 1);
            bus.Write(addr, (byte)(t & 0x00FF));
            SetFlags(registers, t);
            return 6;
        }
    }
    public sealed class INC_AbsoluteX : INC
    {
        public INC_AbsoluteX() : base(3, AddrMode6502.AbsoluteX)
        {
        }

        public override byte Execute(Registers6502 registers, Bus6502 bus)
        {
            var addr = AbsoluteX(registers, bus).addr;
            var t = (ushort)(bus.ReadByte(addr) + 1);
            bus.Write(addr, (byte)(t & 0x00FF));
            SetFlags(registers, t);
            return 7;
        }
    }
}

