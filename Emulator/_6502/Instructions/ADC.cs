

using Kaitai;

namespace Emulator._6502.Instructions
{
    public abstract class ADC : Instruction6502
    {
        protected ADC(byte bytesUsed, AddrMode6502 mode) : base("ADC", bytesUsed, mode, Status6502.Carry | Status6502.Zero | Status6502.OverFlow | Status6502.Negative)
        {
        }

        protected static void SetFlags(ref Cpu6502 cpu, byte data, ushort value)
        {
            // The carry flag out exists in the high byte bit 0
            cpu.SetFlag(Status6502.Carry, value > 255);

            // The Zero flag is set if the result is 0
            cpu.SetFlag(Status6502.Zero, (value & 0x00FF) == 0);

            // The signed Overflow flag is set based on all that up there! :D
            cpu.SetFlag(Status6502.OverFlow, (~(cpu.A ^ data) & (cpu.A ^ value) & 0x0080) > 0);

            // The negative flag is set to the most significant bit of the result
            cpu.SetFlag(Status6502.Negative, (value & 0x80) > 0);
        }
    }

    public sealed class ADC_ZeroPage : ADC
    {
        public ADC_ZeroPage() : base(2, AddrMode6502.ZeroPage)
        {
        }
        public override byte Execute(Cpu6502 cpu)
        {
            byte fetched = cpu.ReadByte(ZeroPage(ref cpu));
            if (cpu.GetFlag(Status6502.Decimal))
            {
                ushort temp = (ushort)(cpu.A + fetched + (ushort)(cpu.Status.HasFlag(Status6502.Carry) ? 1 : 0));
                cpu.A = (byte)(temp & 0x00FF);
                SetFlags(ref cpu, fetched, temp);
            }
            else
            {
                //TODO: Work on BCD Mode
                var bcd1 = new Bcd(2, 4, true, new KaitaiStream(new[] { fetched }));
                var bcd2 = new Bcd(2, 4, true, new KaitaiStream(new[] { cpu.A }));

                var bcd3l = new Bcd(2, 4, true, new KaitaiStream(new[] { (byte)(bcd1.Digits[0] + bcd2.Digits[0]) }));
                var bcd3h = new Bcd(2, 4, true, new KaitaiStream(new[] { (byte)(bcd1.Digits[1] + bcd2.Digits[1]) }));

                byte temp = (byte)((byte)(bcd3h.AsInt << 4) | (byte)bcd3l.AsInt & 0x0F);
                cpu.A = temp;
                SetFlags(ref cpu, fetched, temp);
            }
            return 3;
        }
    }

    public sealed class ADC_ZeroPageX : ADC
    {
        public ADC_ZeroPageX() : base(2, AddrMode6502.ZeroPageX)
        {
        }

        public override byte Execute(Cpu6502 cpu)
        {
            byte fetched = cpu.ReadByte(ZeroPageX(ref cpu));
            ushort temp = (ushort)(cpu.A + fetched + (ushort)(cpu.Status.HasFlag(Status6502.Carry) ? 1 : 0));
            cpu.A = (byte)(temp & 0x00FF);
            SetFlags(ref cpu, fetched, temp);
            return 4;
        }
    }
    public sealed class ADC_Absolute : ADC
    {
        public ADC_Absolute() : base(3, AddrMode6502.Absolute)
        {
        }

        public override byte Execute(Cpu6502 cpu)
        {
            byte fetched = cpu.ReadByte(Absolute(ref cpu));
            ushort temp = (ushort)(cpu.A + fetched + (ushort)(cpu.Status.HasFlag(Status6502.Carry) ? 1 : 0));
            cpu.A = (byte)(temp & 0x00FF);
            SetFlags(ref cpu, fetched, temp);
            return 4;
        }
    }

    public sealed class ADC_Immediate : ADC
    {
        public ADC_Immediate() : base(2, AddrMode6502.Immediate)
        {
        }

        public override byte Execute(Cpu6502 cpu)
        {
            byte fetched = cpu.ReadByte(cpu.PC++);
            ushort temp = (ushort)(cpu.A + fetched + (ushort)(cpu.Status.HasFlag(Status6502.Carry) ? 1 : 0));
            cpu.A = (byte)(temp & 0x00FF);
            SetFlags(ref cpu, fetched, temp);
            return 2;
        }
    }

    public sealed class ADC_IndirectIndexed : ADC
    {
        public ADC_IndirectIndexed() : base(2, AddrMode6502.IndirectIndexed)
        {
        }

        public override byte Execute(Cpu6502 cpu)
        {
            var (addr, clocks) = IndirectIndex(ref cpu);
            byte fetched = cpu.ReadByte(addr);
            ushort temp = (ushort)(cpu.A + fetched + (ushort)(cpu.Status.HasFlag(Status6502.Carry) ? 1 : 0));
            cpu.A = (byte)(temp & 0x00FF);
            SetFlags(ref cpu, fetched, temp);
            return (byte)(clocks + 5);

        }
    }

    public sealed class ADC_IndexedIndirect : ADC
    {
        public ADC_IndexedIndirect() : base(2, AddrMode6502.IndexedIndirect)
        {
        }

        public override byte Execute(Cpu6502 cpu)
        {
            byte fetched = cpu.ReadByte(IndexIndirect(ref cpu));
            ushort temp = (ushort)(cpu.A + fetched + (ushort)(cpu.Status.HasFlag(Status6502.Carry) ? 1 : 0));
            cpu.A = (byte)(temp & 0x00FF);
            SetFlags(ref cpu, fetched, temp);
            return 6;
        }
    }

    public sealed class ADC_AbsoluteX : ADC
    {
        public ADC_AbsoluteX() : base(3, AddrMode6502.AbsoluteX)
        {
        }

        public override byte Execute(Cpu6502 cpu)
        {
            var (addr, clocks) = AbsoluteX(ref cpu);
            byte fetched = cpu.ReadByte(addr);
            ushort temp = (ushort)(cpu.A + fetched + (ushort)(cpu.Status.HasFlag(Status6502.Carry) ? 1 : 0));
            cpu.A = (byte)(temp & 0x00FF);
            SetFlags(ref cpu, fetched, temp);
            return (byte)(clocks + 4);
        }
    }
    public sealed class ADC_AbsoluteY : ADC
    {
        public ADC_AbsoluteY() : base(3, AddrMode6502.AbsoluteY)
        {
        }

        public override byte Execute(Cpu6502 cpu)
        {
            var (addr, clocks) = AbsoluteY(ref cpu);
            byte fetched = cpu.ReadByte(addr);
            ushort temp = (ushort)(cpu.A + fetched + (ushort)(cpu.Status.HasFlag(Status6502.Carry) ? 1 : 0));
            cpu.A = (byte)(temp & 0x00FF);
            SetFlags(ref cpu, fetched, temp);
            return (byte)(clocks + 4);
        }
    }
}
