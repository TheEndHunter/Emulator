using Emulator._6502.Devices;

namespace Emulator._6502.CPU.Instructions
{
    public abstract class ADC : Instruction6502
    {
        protected ADC(byte bytesUsed, AddrMode6502 mode) : base("ADC", bytesUsed, mode, Status6502.Carry | Status6502.Zero | Status6502.OverFlow | Status6502.Negative)
        {
        }

        protected static void SetFlags(Registers6502 registers, byte data, ushort value)
        {
            // The carry flag out exists in the high byte bit 0
            registers.SetFlag(Status6502.Carry, value > 255);

            // The Zero flag is set if the result is 0
            registers.SetFlag(Status6502.Zero, (value & 0x00FF) == 0);

            // The signed Overflow flag is set based on all that up there! :D
            registers.SetFlag(Status6502.OverFlow, (~(registers.A ^ data) & (registers.A ^ value) & 0x0080) > 0);

            // The negative flag is set to the most significant bit of the result
            registers.SetFlag(Status6502.Negative, (value & 0x80) > 0);
        }
    }

    public sealed class ADC_ZeroPage : ADC
    {
        public ADC_ZeroPage() : base(2, AddrMode6502.ZeroPage)
        {
        }
        public override byte Execute(Registers6502 registers, Bus6502 bus)
        {
            byte fetched = bus.ReadByte(ZeroPage(registers, bus));
            ushort temp = (ushort)(registers.A + fetched + (ushort)(registers.Status.HasFlag(Status6502.Carry) ? 1 : 0));
            registers.A = (byte)(temp & 0x00FF);
            SetFlags(registers, fetched, temp);
            return 3;
        }
    }

    public sealed class ADC_ZeroPageX : ADC
    {
        public ADC_ZeroPageX() : base(2, AddrMode6502.ZeroPageX)
        {
        }

        public override byte Execute(Registers6502 registers, Bus6502 bus)
        {
            byte fetched = bus.ReadByte(ZeroPageX(registers, bus));
            ushort temp = (ushort)(registers.A + fetched + (ushort)(registers.Status.HasFlag(Status6502.Carry) ? 1 : 0));
            registers.A = (byte)(temp & 0x00FF);
            SetFlags(registers, fetched, temp);
            return 4;
        }
    }
    public sealed class ADC_Absolute : ADC
    {
        public ADC_Absolute() : base(3, AddrMode6502.Absolute)
        {
        }

        public override byte Execute(Registers6502 registers, Bus6502 bus)
        {
            byte fetched = bus.ReadByte(Absolute(registers, bus));
            ushort temp = (ushort)(registers.A + fetched + (ushort)(registers.Status.HasFlag(Status6502.Carry) ? 1 : 0));
            registers.A = (byte)(temp & 0x00FF);
            SetFlags(registers, fetched, temp);
            return 4;
        }
    }

    public sealed class ADC_Immediate : ADC
    {
        public ADC_Immediate() : base(2, AddrMode6502.Immediate)
        {
        }

        public override byte Execute(Registers6502 registers, Bus6502 bus)
        {
            byte fetched = bus.ReadByte(Immediate(registers, bus));
            ushort temp = (ushort)(registers.A + fetched + (ushort)(registers.Status.HasFlag(Status6502.Carry) ? 1 : 0));
            registers.A = (byte)(temp & 0x00FF);
            SetFlags(registers, fetched, temp);
            return 2;
        }
    }

    public sealed class ADC_IndirectIndexed : ADC
    {
        public ADC_IndirectIndexed() : base(2, AddrMode6502.IndirectIndexed)
        {
        }

        public override byte Execute(Registers6502 registers, Bus6502 bus)
        {
            byte fetched = bus.ReadByte(IndirectIndex(registers, bus));
            ushort temp = (ushort)(registers.A + fetched + (ushort)(registers.Status.HasFlag(Status6502.Carry) ? 1 : 0));
            registers.A = (byte)(temp & 0x00FF);
            SetFlags(registers, fetched, temp);
            return 6;
        }
    }

    public sealed class ADC_IndexedIndirect : ADC
    {
        public ADC_IndexedIndirect() : base(2, AddrMode6502.IndexedIndirect)
        {
        }

        public override byte Execute(Registers6502 registers, Bus6502 bus)
        {
            var (addr, clocks) = IndexIndirect(registers, bus);
            byte fetched = bus.ReadByte(addr);
            ushort temp = (ushort)(registers.A + fetched + (ushort)(registers.Status.HasFlag(Status6502.Carry) ? 1 : 0));
            registers.A = (byte)(temp & 0x00FF);
            SetFlags(registers, fetched, temp);
            return (byte)(clocks + 5);
        }
    }

    public sealed class ADC_AbsoluteX : ADC
    {
        public ADC_AbsoluteX() : base(3, AddrMode6502.AbsoluteX)
        {
        }

        public override byte Execute(Registers6502 registers, Bus6502 bus)
        {
            var (addr, clocks) = AbsoluteX(registers, bus);
            byte fetched = bus.ReadByte(addr);
            ushort temp = (ushort)(registers.A + fetched + (ushort)(registers.Status.HasFlag(Status6502.Carry) ? 1 : 0));
            registers.A = (byte)(temp & 0x00FF);
            SetFlags(registers, fetched, temp);
            return (byte)(clocks + 4);
        }
    }
    public sealed class ADC_AbsoluteY : ADC
    {
        public ADC_AbsoluteY() : base(3, AddrMode6502.AbsoluteY)
        {
        }

        public override byte Execute(Registers6502 registers, Bus6502 bus)
        {
            var (addr, clocks) = AbsoluteY(registers, bus);
            byte fetched = bus.ReadByte(addr);
            ushort temp = (ushort)(registers.A + fetched + (ushort)(registers.Status.HasFlag(Status6502.Carry) ? 1 : 0));
            registers.A = (byte)(temp & 0x00FF);
            SetFlags(registers, fetched, temp);
            return (byte)(clocks + 4);
        }
    }
}
