using Emulator._6502.Devices;

namespace Emulator._6502.CPU.Instructions
{
    public abstract class CPX : Instruction6502
    {
        protected CPX(byte bytesUsed, AddrMode6502 mode) : base("CPX", bytesUsed, mode, Status6502.Carry | Status6502.Zero | Status6502.Negative)
        {
        }
        protected static void SetFlags(ref Registers6502 registers, byte fetched)
        {
            var temp = registers.X - fetched;
            // The carry flag out exists in the high byte bit 0
            registers.SetFlag(Status6502.Carry, registers.X >= fetched);

            // The Zero flag is set if the result is 0
            registers.SetFlag(Status6502.Zero, (temp & 0x00FF) == 0x0000);

            // The negative flag is set to the most significant bit of low byte the result
            registers.SetFlag(Status6502.Negative, (temp & 0x0080) > 0);
        }
    }

    public sealed class CPX_Immediate : CPX
    {
        public CPX_Immediate() : base(2, AddrMode6502.Immediate)
        {

        }

        public override byte Execute(ref Registers6502 registers, Bus6502 bus)
        {
            SetFlags(ref registers, bus.ReadByte(Immediate(ref registers, bus)));
            return 2;
        }
    }
    public sealed class CPX_Absolute : CPX
    {
        public CPX_Absolute() : base(3, AddrMode6502.Absolute)
        {

        }

        public override byte Execute(ref Registers6502 registers, Bus6502 bus)
        {
            SetFlags(ref registers, bus.ReadByte(Absolute(ref registers, bus)));
            return 4;
        }
    }

    public sealed class CPX_ZeroPage : CPX
    {
        public CPX_ZeroPage() : base(2, AddrMode6502.ZeroPage)
        {

        }

        public override byte Execute(ref Registers6502 registers, Bus6502 bus)
        {
            SetFlags(ref registers, bus.ReadByte(ZeroPage(ref registers, bus)));
            return 3;
        }
    }
}
