using Emulator._6502.Devices;

namespace Emulator._6502.CPU.Instructions
{
    public abstract class LDX : Instruction6502
    {
        protected LDX(AddrMode6502 mode) : base("LDX", mode, Status6502.Zero | Status6502.Negative)
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

    public sealed class LDX_Immediate : LDX
    {
        public LDX_Immediate() : base(AddrMode6502.Immediate)
        {
        }

        public override byte Execute(Registers6502 registers, Bus6502 bus)
        {
            registers.X = bus.ReadByte(Immediate(registers, bus));
            SetFlags(registers, registers.X);
            return 2;
        }
    }

    public sealed class LDX_ZeroPage : LDX
    {
        public LDX_ZeroPage() : base(AddrMode6502.ZeroPage)
        {
        }

        public override byte Execute(Registers6502 registers, Bus6502 bus)
        {
            registers.X = bus.ReadByte(ZeroPage(registers, bus));
            SetFlags(registers, registers.X);
            return 3;
        }
    }
    public sealed class LDX_Absolute : LDX
    {
        public LDX_Absolute() : base(AddrMode6502.Absolute)
        {
        }

        public override byte Execute(Registers6502 registers, Bus6502 bus)
        {
            registers.X = bus.ReadByte(Absolute(registers, bus));
            SetFlags(registers, registers.X);
            return 4;
        }
    }

    public sealed class LDX_ZeroPageY : LDX
    {
        public LDX_ZeroPageY() : base(AddrMode6502.ZeroPageY)
        {
        }

        public override byte Execute(Registers6502 registers, Bus6502 bus)
        {
            registers.X = bus.ReadByte(ZeroPageY(registers, bus));
            SetFlags(registers, registers.X);
            return 4;
        }
    }
    public sealed class LDX_AbsoluteY : LDX
    {
        public LDX_AbsoluteY() : base(AddrMode6502.AbsoluteY)
        {
        }

        public override byte Execute(Registers6502 registers, Bus6502 bus)
        {
            var (addr, clocks) = AbsoluteY(registers, bus);
            registers.X = bus.ReadByte(addr);
            SetFlags(registers, registers.X);
            return (byte)(4 + clocks);
        }
    }
}

