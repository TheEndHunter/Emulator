namespace Emulator._6502.Instructions
{
    public abstract class ADC(byte bytesUsed, AddrMode6502 mode) : Instruction6502("ADC", bytesUsed, mode, Status6502.Carry | Status6502.Zero | Status6502.OverFlow | Status6502.Negative)
    {
        protected static void SetFlags(ref Cpu6502 cpu, byte fetched, ushort sum)
        {
            cpu.SetFlag(Status6502.Zero, (byte)(sum & 0xFF) == 0);

            if (cpu.Status.HasFlag(Status6502.Decimal))
            {
                cpu.SetFlag(Status6502.Carry, sum > 0x99);
                cpu.SetFlag(Status6502.Negative, (sum & 0x80) != 0);

                byte twosCompA = (byte)((cpu.A & 0x80) != 0 ? cpu.A - 0x100 : cpu.A);
                byte twosCompFetched = (byte)((fetched & 0x80) != 0 ? fetched - 0x100 : fetched);
                short twosCompSum = (short)(twosCompA + twosCompFetched + (cpu.Status.HasFlag(Status6502.Carry) ? 1 : 0));

                cpu.SetFlag(Status6502.OverFlow, twosCompSum is > 127 or < (-128));
            }
            else
            {
                cpu.SetFlag(Status6502.Carry, sum > 0xFF);
                cpu.SetFlag(Status6502.Negative, (sum & 0x80) != 0);
                cpu.SetFlag(Status6502.OverFlow, ((cpu.A ^ fetched ^ (cpu.A ^ sum)) & 0x80) != 0);
            }
        }

        public static void BinaryMode(ref Cpu6502 cpu, byte fetched)
        {
            ushort sum = (ushort)(cpu.A + fetched + (cpu.Status.HasFlag(Status6502.Carry) ? 1 : 0));

            cpu.A = (byte)sum;
            SetFlags(ref cpu, fetched, sum);
        }

        public static void DecimalMode(ref Cpu6502 cpu, byte fetched)
        {
            ushort tempA = (byte)(cpu.A & 0x0F);
            ushort tempB = (byte)(fetched & 0x0F);

            ushort tempSum = (byte)(tempA + tempB + (cpu.Status.HasFlag(Status6502.Carry) ? 1 : 0));

            if (tempSum > 9)
            {
                tempSum = (byte)(((tempSum - 10) & 0x0F) + 0x10);
            }

            tempA = (byte)((cpu.A & 0xF0) + (fetched & 0xF0) + tempSum);

            bool b = tempA > 0x99;
            if (b)
            {
                cpu.A = (byte)(tempA + 0x60);
            }
            else
            {
                cpu.A = (byte)tempA;
            }
            SetFlags(ref cpu, fetched, tempA);
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
                DecimalMode(ref cpu, fetched);
            else
                BinaryMode(ref cpu, fetched);

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
            if (cpu.GetFlag(Status6502.Decimal))
                DecimalMode(ref cpu, fetched);
            else
                BinaryMode(ref cpu, fetched);
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
            if (cpu.GetFlag(Status6502.Decimal))
                DecimalMode(ref cpu, fetched);
            else
                BinaryMode(ref cpu, fetched);
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
            if (cpu.GetFlag(Status6502.Decimal))
                DecimalMode(ref cpu, fetched);
            else
                BinaryMode(ref cpu, fetched);
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
            if (cpu.GetFlag(Status6502.Decimal))
                DecimalMode(ref cpu, fetched);
            else
                BinaryMode(ref cpu, fetched);
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
            if (cpu.GetFlag(Status6502.Decimal))
                DecimalMode(ref cpu, fetched);
            else
                BinaryMode(ref cpu, fetched);
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
            if (cpu.GetFlag(Status6502.Decimal))
                DecimalMode(ref cpu, fetched);
            else
                BinaryMode(ref cpu, fetched);
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
            if (cpu.GetFlag(Status6502.Decimal))
                DecimalMode(ref cpu, fetched);
            else
                BinaryMode(ref cpu, fetched);
            return (byte)(clocks + 4);
        }
    }
}
