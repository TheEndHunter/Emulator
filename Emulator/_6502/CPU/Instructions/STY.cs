using Emulator._6502.Devices;

namespace Emulator._6502.CPU.Instructions
{
    public abstract class STY : Instruction6502
    {
        protected STY(byte bytesUsed, AddrMode6502 mode) : base("STY", bytesUsed, mode, Status6502.None)
        {
        }
    }

    public sealed class STY_ZeroPage : STY
    {
        public STY_ZeroPage() : base(2, AddrMode6502.ZeroPage)
        {
        }

        public override byte Execute(Registers6502 registers, Bus6502 bus)
        {
            bus.Write(bus.ReadWord(ZeroPage(registers, bus)), registers.Y);
            return 4;
        }
    }
    public sealed class STY_Absolute : STY
    {
        public STY_Absolute() : base(3, AddrMode6502.Absolute)
        {
        }

        public override byte Execute(Registers6502 registers, Bus6502 bus)
        {
            bus.Write(bus.ReadWord(Absolute(registers, bus)), registers.Y);
            return 3;
        }
    }

    public sealed class STY_ZeroPageX : STY
    {
        public STY_ZeroPageX() : base(2, AddrMode6502.ZeroPageX)
        {
        }

        public override byte Execute(Registers6502 registers, Bus6502 bus)
        {
            bus.Write(bus.ReadWord(ZeroPageX(registers, bus)), registers.Y);
            return 4;
        }
    }
}

