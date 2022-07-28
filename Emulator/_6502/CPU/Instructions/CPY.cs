using Emulator._6502.Devices;

namespace Emulator._6502.CPU.Instructions
{
    public abstract class CPY : Instruction6502
    {
        protected CPY(byte bytesUsed, AddrMode6502 mode) : base("CPY", bytesUsed, mode, Status6502.Carry | Status6502.Zero | Status6502.Negative)
        {
        }
        protected static void SetFlags(Registers6502 registers, byte fetched)
        {
            var temp = registers.Y - fetched;
            // The carry flag out exists in the high byte bit 0
            registers.SetFlag(Status6502.Carry, registers.Y >= fetched);

            // The Zero flag is set if the result is 0
            registers.SetFlag(Status6502.Zero, (temp & 0x00FF) == 0x0000);

            // The negative flag is set to the most significant bit of low byte the result
            registers.SetFlag(Status6502.Negative, (temp & 0x0080) > 0);
        }
    }

    public sealed class CPY_Immediate : CPY
    {
        public CPY_Immediate() : base(2, AddrMode6502.Immediate)
        {

        }

        public override byte Execute(Registers6502 registers, Bus6502 bus)
        {
            SetFlags(registers, bus.ReadByte(Immediate(registers, bus)));
            return 2;
        }
    }
    public sealed class CPY_Absolute : CPY
    {
        public CPY_Absolute() : base(3, AddrMode6502.Absolute)
        {

        }

        public override byte Execute(Registers6502 registers, Bus6502 bus)
        {
            SetFlags(registers, bus.ReadByte(Absolute(registers, bus)));
            return 4;
        }
    }

    public sealed class CPY_ZeroPage : CPY
    {
        public CPY_ZeroPage() : base(2, AddrMode6502.ZeroPage)
        {

        }

        public override byte Execute(Registers6502 registers, Bus6502 bus)
        {
            SetFlags(registers, bus.ReadByte(ZeroPage(registers, bus)));
            return 3;
        }
    }
}
