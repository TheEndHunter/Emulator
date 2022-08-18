using Emulator._6502.Devices;

namespace Emulator._6502.CPU.Instructions
{
    public abstract class ASL : Instruction6502
    {
        protected ASL(byte bytesUsed, AddrMode6502 mode) : base("ASL", bytesUsed, mode, Status6502.Carry | Status6502.Zero | Status6502.Negative)
        {
        }
        protected static void SetFlags(ref Registers6502 registers, ushort data)
        {

            // The carry flag out exists in the high byte bit 0
            registers.SetFlag(Status6502.Carry, (data & 0xFF00) > 0);

            // The Zero flag is set if the result is 0
            registers.SetFlag(Status6502.Zero, (data & 0x00FF) == 0x00);


            // The negative flag is set to the most significant bit of the result
            registers.SetFlag(Status6502.Negative, (data & 0x80) > 0);
        }
    }

    public sealed class ASL_Acumulator : ASL
    {
        public ASL_Acumulator() : base(1, AddrMode6502.Accumulator)
        {
        }

        public override byte Execute(ref Registers6502 registers, Bus6502 bus)
        {
            var temp = (ushort)(registers.A << 1);
            registers.A = (byte)(temp & 0x00FF);
            SetFlags(ref registers, temp);
            return 2;
        }
    }

    public sealed class ASL_ZeroPage : ASL
    {
        public ASL_ZeroPage() : base(2, AddrMode6502.ZeroPage)
        {
        }

        public override byte Execute(ref Registers6502 registers, Bus6502 bus)
        {
            var raddr = registers.PC;
            var temp = (ushort)(bus.ReadByte(ZeroPage(ref registers, bus)) << 1);
            bus.Write(raddr, (byte)(temp & 0x00FF));
            SetFlags(ref registers, temp);
            return 5;
        }
    }

    public sealed class ASL_ZeroPageX : ASL
    {
        public ASL_ZeroPageX() : base(2, AddrMode6502.ZeroPageX)
        {
        }

        public override byte Execute(ref Registers6502 registers, Bus6502 bus)
        {
            var raddr = registers.PC;
            var temp = (ushort)(bus.ReadByte(ZeroPageX(ref registers, bus)) << 1);
            bus.Write(raddr, (byte)(temp & 0x00FF));
            SetFlags(ref registers, temp);
            return 6;
        }
    }
    public sealed class ASL_Absolute : ASL
    {
        public ASL_Absolute() : base(3, AddrMode6502.Absolute)
        {
        }

        public override byte Execute(ref Registers6502 registers, Bus6502 bus)
        {
            var raddr = registers.PC;
            var temp = (ushort)(bus.ReadByte(Absolute(ref registers, bus)) << 1);
            bus.Write(raddr, (byte)(temp & 0x00FF));
            SetFlags(ref registers, temp);
            return 6;
        }
    }

    public sealed class ASL_AbsoluteX : ASL
    {
        public ASL_AbsoluteX() : base(3, AddrMode6502.AbsoluteX)
        {
        }

        public override byte Execute(ref Registers6502 registers, Bus6502 bus)
        {
            var raddr = registers.PC;
            var temp = (ushort)(bus.ReadByte(AbsoluteX(ref registers, bus).addr) << 1);
            bus.Write(raddr, (byte)(temp & 0x00FF));
            SetFlags(ref registers, temp);
            return 7;
        }
    }
}
