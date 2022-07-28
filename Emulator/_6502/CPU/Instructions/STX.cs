using Emulator._6502.Devices;

namespace Emulator._6502.CPU.Instructions
{
    public abstract class STX : Instruction6502
    {
        protected STX(byte bytesUsed, AddrMode6502 mode) : base("STX", bytesUsed, mode, Status6502.None)
        {
        }
    }

    public sealed class STX_ZeroPage : STX
    {
        public STX_ZeroPage() : base(2, AddrMode6502.ZeroPage)
        {
        }

        public override byte Execute(Registers6502 registers, Bus6502 bus)
        {
            bus.Write(bus.ReadWord(ZeroPage(registers, bus)), registers.X);
            return 3;
        }
    }
    public sealed class STX_Absolute : STX
    {
        public STX_Absolute() : base(3, AddrMode6502.Absolute)
        {
        }

        public override byte Execute(Registers6502 registers, Bus6502 bus)
        {
            bus.Write(bus.ReadWord(Absolute(registers, bus)), registers.X);
            return 4;
        }
    }

    public sealed class STX_ZeroPageY : STX
    {
        public STX_ZeroPageY() : base(2, AddrMode6502.ZeroPageY)
        {
        }

        public override byte Execute(Registers6502 registers, Bus6502 bus)
        {
            bus.Write(bus.ReadWord(ZeroPageY(registers, bus)), registers.X);
            return 4;
        }
    }
}

