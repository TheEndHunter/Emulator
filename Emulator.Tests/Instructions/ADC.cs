using Emulator._6502;

namespace Emulator.Tests.Instructions
{
    [TestClass]
    public sealed class ADC : InstructionTests6502
    {

        public static (byte sum, Status6502 status) AddBinaryMode(Status6502 s, byte A, byte fetched)
        {
            ushort sum = (ushort)(A + fetched + (s.HasFlag(Status6502.Carry) ? 1 : 0));

            s = SetADCFlags(s, A, fetched, sum);
            return ((byte)sum, s);
        }

        public static (byte sum, Status6502 status) AddDecimalMode(Status6502 s, byte A, byte fetched)
        {
            ushort tempA = (byte)(A & 0x0F);
            ushort tempB = (byte)(fetched & 0x0F);

            ushort tempSum = (byte)(tempA + tempB + (s.HasFlag(Status6502.Carry) ? 1 : 0));

            if (tempSum > 9)
            {
                tempSum = (byte)(((tempSum - 10) & 0x0F) + 0x10);
            }

            tempA = (byte)((A & 0xF0) + (fetched & 0xF0) + tempSum);

            bool b = tempA > 0x99;
            byte C;
            if (b)
            {
                C = (byte)(tempA + 0x60);
            }
            else
            {
                C = (byte)tempA;
            }
            s = SetADCFlags(s, C, fetched, tempA);
            return (C, s);
        }

        public static Status6502 SetADCFlags(Status6502 s, byte A, byte fetched, ushort sum)
        {
            s = SetStatus(s, Status6502.Zero, (byte)(sum & 0xFF) == 0);

            if (s.HasFlag(Status6502.Decimal))
            {
                s = SetStatus(s, Status6502.Carry, sum > 0x99);
                s = SetStatus(s, Status6502.Negative, (sum & 0x80) != 0);

                byte twosCompA = (byte)((A & 0x80) != 0 ? A - 0x100 : A);
                byte twosCompFetched = (byte)((fetched & 0x80) != 0 ? fetched - 0x100 : fetched);
                short twosCompSum = (short)(twosCompA + twosCompFetched + (s.HasFlag(Status6502.Carry) ? 1 : 0));

                s = SetStatus(s, Status6502.OverFlow, twosCompSum is > 127 or < (-128));
            }
            else
            {
                s = SetStatus(s, Status6502.Carry, sum > 0xFF);
                s = SetStatus(s, Status6502.Negative, (sum & 0x80) != 0);
                s = SetStatus(s, Status6502.OverFlow, (((A ^ fetched) ^ (A ^ sum)) & 0x80) != 0);
            }
            return s;
        }

        [DataTestMethod()]
        [TestCategory("Immediate")]
        [DataRow((byte)0x00, (byte)0x23, (ushort)0xDEAD, false, DisplayName = "T1")]
        [DataRow((byte)0x00, (byte)0x23, (ushort)0xDEAD, true, DisplayName = "T1(Decimal Mode)")]
        [DataRow((byte)0xFF, (byte)0x99, (ushort)0xBEEF, false, DisplayName = "T2")]
        [DataRow((byte)0xFF, (byte)0x99, (ushort)0xBEEF, true, DisplayName = "T2(Decimal Mode)")]
        [DataRow((byte)0xFA, (byte)0x67, (ushort)0xFEED, false, DisplayName = "T3")]
        [DataRow((byte)0xFA, (byte)0x67, (ushort)0xFEED, true, DisplayName = "T3(Decimal Mode)")]
        [DataRow((byte)0x39, (byte)0x45, (ushort)0x0000, false, DisplayName = "T4")]
        [DataRow((byte)0x39, (byte)0x45, (ushort)0x0000, true, DisplayName = "T4(Decimal Mode)")]
        public void Immediate(byte data, byte A, ushort address, bool DecimalMode)
        {
            LoadImmediate(0x69, address, data);

            Cpu.A = A;
            Cpu.Status = SetCarry(Cpu.Status, A, false);
            Cpu.SetFlag(Status6502.Decimal, DecimalMode);

            (byte sum, Status6502 status) = DecimalMode ? AddDecimalMode(Cpu.Status, A, data) : AddBinaryMode(Cpu.Status, A, data);
            var steps = Cpu.Step(1);

            FinishTest(sum, Cpu.X, Cpu.Y, status, Cpu.STKP, (ushort)(address + 2), 2UL, steps);
        }

        [DataTestMethod()]
        [TestCategory("ZeroPage")]
        [DataRow((byte)0x10, (byte)0x01, (byte)0x00, (ushort)0xDEAD, false, DisplayName = "T1")]
        [DataRow((byte)0x10, (byte)0x01, (byte)0x00, (ushort)0xDEAD, true, DisplayName = "T1(Decimal Mode)")]
        [DataRow((byte)0x01, (byte)0x67, (byte)0x81, (ushort)0xBEEF, false, DisplayName = "T2")]
        [DataRow((byte)0x01, (byte)0x67, (byte)0x81, (ushort)0xBEEF, true, DisplayName = "T2(Decimal Mode)")]
        [DataRow((byte)0xFA, (byte)0xFF, (byte)0x62, (ushort)0xFEED, false, DisplayName = "T3")]
        [DataRow((byte)0xFA, (byte)0xFF, (byte)0x62, (ushort)0xFEED, true, DisplayName = "T3(Decimal Mode)")]
        [DataRow((byte)0xBE, (byte)0x44, (byte)0x39, (ushort)0x0000, false, DisplayName = "T4")]
        [DataRow((byte)0xBE, (byte)0x44, (byte)0x39, (ushort)0x0000, true, DisplayName = "T4(Decimal Mode)")]
        public void ZeroPage(byte zpAddr, byte A, byte data, ushort address, bool DecimalMode)
        {
            LoadZeroPage(0x65, address, zpAddr, data);

            Cpu.A = A;
            Cpu.Status = SetCarry(Cpu.Status, A, false);
            Cpu.SetFlag(Status6502.Decimal, DecimalMode);

            (byte sum, Status6502 status) = DecimalMode ? AddDecimalMode(Cpu.Status, A, data) : AddBinaryMode(Cpu.Status, A, data);
            var steps = Cpu.Step(1);

            FinishTest(sum, Cpu.X, Cpu.Y, status, Cpu.STKP, (ushort)(address + 2), 3UL, steps);
        }

        [DataTestMethod()]

        [TestCategory("ZeroPageX")]
        [DataRow((byte)0x10, (byte)0x01, (byte)0x00, (byte)0x87, (ushort)0xDEAD, false, DisplayName = "T1")]
        [DataRow((byte)0x10, (byte)0x01, (byte)0x00, (byte)0x87, (ushort)0xDEAD, true, DisplayName = "T1(Decimal Mode)")]
        [DataRow((byte)0x01, (byte)0x20, (byte)0xFF, (byte)0xEF, (ushort)0xBEEF, false, DisplayName = "T2")]
        [DataRow((byte)0x01, (byte)0x20, (byte)0xFF, (byte)0xEF, (ushort)0xBEEF, true, DisplayName = "T2(Decimal Mode)")]
        [DataRow((byte)0xFA, (byte)0x12, (byte)0x32, (byte)0x00, (ushort)0xFEED, false, DisplayName = "T3")]
        [DataRow((byte)0xFA, (byte)0x12, (byte)0x32, (byte)0x00, (ushort)0xFEED, true, DisplayName = "T3(Decimal Mode)")]
        [DataRow((byte)0xBE, (byte)0x99, (byte)0x39, (byte)0xBA, (ushort)0x0000, false, DisplayName = "T4")]
        [DataRow((byte)0xBE, (byte)0x99, (byte)0x39, (byte)0xBA, (ushort)0x0000, true, DisplayName = "T4(Decimal Mode)")]
        public void ZeroPageX(byte zpAddr, byte A, byte data, byte xOffset, ushort address, bool DecimalMode)
        {
            LoadZeroPageX(0x75, address, zpAddr, xOffset, data);
            Cpu.A = A;
            Cpu.Status = SetCarry(Cpu.Status, A, false);
            Cpu.SetFlag(Status6502.Decimal, DecimalMode);

            (byte sum, Status6502 status) = DecimalMode ? AddDecimalMode(Cpu.Status, A, data) : AddBinaryMode(Cpu.Status, A, data);
            var steps = Cpu.Step(1);

            FinishTest(sum, Cpu.X, Cpu.Y, status, Cpu.STKP, (ushort)(address + 2), 4UL, steps);
        }

        [DataTestMethod()]

        [TestCategory("Absolute")]
        [DataRow((ushort)0xAB10, (byte)0x23, (byte)0x00, (ushort)0xDEAD, false, DisplayName = "T1")]
        [DataRow((ushort)0xAB10, (byte)0x12, (byte)0x00, (ushort)0xDEAD, true, DisplayName = "T1(Decimal Mode)")]
        [DataRow((ushort)0xEF01, (byte)0x32, (byte)0xFF, (ushort)0xBEEF, false, DisplayName = "T2")]
        [DataRow((ushort)0xEF01, (byte)0x64, (byte)0xFF, (ushort)0xBEEF, true, DisplayName = "T2(Decimal Mode)")]
        [DataRow((ushort)0xCDFA, (byte)0xFF, (byte)0xFA, (ushort)0xFEED, false, DisplayName = "T3")]
        [DataRow((ushort)0xCDFA, (byte)0xBA, (byte)0xFA, (ushort)0xFEED, true, DisplayName = "T3(Decimal Mode)")]
        [DataRow((ushort)0xAABE, (byte)0x99, (byte)0x39, (ushort)0x0000, false, DisplayName = "T4")]
        [DataRow((ushort)0xAABE, (byte)0x00, (byte)0x39, (ushort)0x0000, true, DisplayName = "T4(Decimal Mode)")]
        public void Absolute(ushort absAddr, byte A, byte data, ushort address, bool DecimalMode)
        {
            LoadAbsolute(0x6D, address, absAddr, data);
            Cpu.A = A;
            Cpu.Status = SetCarry(Cpu.Status, A, false);
            Cpu.SetFlag(Status6502.Decimal, DecimalMode);

            (byte sum, Status6502 status) = DecimalMode ? AddDecimalMode(Cpu.Status, A, data) : AddBinaryMode(Cpu.Status, A, data);
            var steps = Cpu.Step(1);

            FinishTest(sum, Cpu.X, Cpu.Y, status, Cpu.STKP, (ushort)(address + 3), 4UL, steps);
        }

        [DataTestMethod()]

        [TestCategory("AbsoluteX")]
        [DataRow((ushort)0xAB10, (byte)0x23, (byte)0x00, (byte)0x87, (ushort)0xDEAD, false, DisplayName = "T1")]
        [DataRow((ushort)0xAB10, (byte)0x12, (byte)0x00, (byte)0x87, (ushort)0xDEAD, true, DisplayName = "T1(Decimal Mode)")]
        [DataRow((ushort)0xEF01, (byte)0x32, (byte)0xFF, (byte)0xEF, (ushort)0xBEEF, false, DisplayName = "T2")]
        [DataRow((ushort)0xEF01, (byte)0x64, (byte)0xFF, (byte)0xEF, (ushort)0xBEEF, true, DisplayName = "T2(Decimal Mode)")]
        [DataRow((ushort)0xCDFA, (byte)0xFF, (byte)0xFA, (byte)0x00, (ushort)0xFEED, false, DisplayName = "T3")]
        [DataRow((ushort)0xCDFA, (byte)0xBA, (byte)0xFA, (byte)0x00, (ushort)0xFEED, true, DisplayName = "T3(Decimal Mode)")]
        [DataRow((ushort)0xAABE, (byte)0x99, (byte)0x39, (byte)0xBA, (ushort)0x0000, false, DisplayName = "T4")]
        [DataRow((ushort)0xAABE, (byte)0x00, (byte)0x39, (byte)0xBA, (ushort)0x0000, true, DisplayName = "T4(Decimal Mode)")]
        public void AbsoluteX(ushort absAddr, byte A, byte data, byte xOffset, ushort address, bool DecimalMode)
        {
            LoadAbsoluteX(0x7D, address, absAddr, xOffset, data);
            Cpu.A = A;
            Cpu.Status = SetCarry(Cpu.Status, A, false);
            Cpu.SetFlag(Status6502.Decimal, DecimalMode);

            (byte sum, Status6502 status) = DecimalMode ? AddDecimalMode(Cpu.Status, A, data) : AddBinaryMode(Cpu.Status, A, data);
            var steps = Cpu.Step(1);

            FinishTest(sum, Cpu.X, Cpu.Y, status, Cpu.STKP, (ushort)(address + 3), CheckPageCross(4UL, absAddr, (ushort)(absAddr + xOffset)), steps);
        }

        [DataTestMethod()]

        [TestCategory("AbsoluteY")]
        [DataRow((ushort)0xAB10, (byte)0x23, (byte)0x12, (byte)0x87, (ushort)0xDEAD, false, DisplayName = "T1")]
        [DataRow((ushort)0xAB10, (byte)0x84, (byte)0x82, (byte)0x87, (ushort)0xDEAD, true, DisplayName = "T1(Decimal Mode)")]
        [DataRow((ushort)0xEF01, (byte)0x4F, (byte)0xDD, (byte)0xEF, (ushort)0xBEEF, false, DisplayName = "T2")]
        [DataRow((ushort)0xEF01, (byte)0x87, (byte)0x99, (byte)0xEF, (ushort)0xBEEF, true, DisplayName = "T2(Decimal Mode)")]
        [DataRow((ushort)0xCDFA, (byte)0xFA, (byte)0x23, (byte)0x00, (ushort)0xFEED, false, DisplayName = "T3")]
        [DataRow((ushort)0xCDFA, (byte)0x92, (byte)0x8D, (byte)0x00, (ushort)0xFEED, true, DisplayName = "T3(Decimal Mode)")]
        [DataRow((ushort)0xAABE, (byte)0xEF, (byte)0xB3, (byte)0xBA, (ushort)0x0000, false, DisplayName = "T4")]
        [DataRow((ushort)0xAABE, (byte)0xFF, (byte)0xA2, (byte)0xBA, (ushort)0x0000, true, DisplayName = "T4(Decimal Mode)")]
        public void AbsoluteY(ushort absAddr, byte A, byte data, byte yOffset, ushort address, bool DecimalMode)
        {
            LoadAbsoluteY(0x79, address, absAddr, yOffset, data);
            Cpu.A = A;
            Cpu.Status = SetCarry(Cpu.Status, A, false);
            Cpu.SetFlag(Status6502.Decimal, DecimalMode);

            (byte sum, Status6502 status) = DecimalMode ? AddDecimalMode(Cpu.Status, A, data) : AddBinaryMode(Cpu.Status, A, data);
            var steps = Cpu.Step(1);

            FinishTest(sum, Cpu.X, Cpu.Y, status, Cpu.STKP, (ushort)(address + 3), CheckPageCross(4UL, absAddr, (ushort)(absAddr + yOffset)), steps);
        }

        [DataTestMethod()]

        [TestCategory("IndexedIndirect")]
        [DataRow((byte)0x10, (ushort)0xABCD, (byte)0x23, (byte)0x00, (byte)0x87, (ushort)0xDEAD, false, DisplayName = "T1")]
        [DataRow((byte)0x10, (ushort)0xABCD, (byte)0x23, (byte)0x00, (byte)0x87, (ushort)0xDEAD, true, DisplayName = "T1(Decimal Mode)")]
        [DataRow((byte)0x01, (ushort)0xDBCA, (byte)0x73, (byte)0xFF, (byte)0xEF, (ushort)0xBEEF, false, DisplayName = "T2")]
        [DataRow((byte)0x01, (ushort)0xDBCA, (byte)0x73, (byte)0xFF, (byte)0xEF, (ushort)0xBEEF, true, DisplayName = "T2(Decimal Mode)")]
        [DataRow((byte)0xFA, (ushort)0xEFDA, (byte)0x88, (byte)0xFA, (byte)0x00, (ushort)0xFEED, false, DisplayName = "T3")]
        [DataRow((byte)0xFA, (ushort)0xEFDA, (byte)0x88, (byte)0xFA, (byte)0x00, (ushort)0xFEED, true, DisplayName = "T3(Decimal Mode)")]
        [DataRow((byte)0xBE, (ushort)0xCDAB, (byte)0x92, (byte)0x39, (byte)0xBA, (ushort)0x0000, false, DisplayName = "T4")]
        [DataRow((byte)0xBE, (ushort)0xCDAB, (byte)0x92, (byte)0x39, (byte)0xBA, (ushort)0x0000, true, DisplayName = "T4(Decimal Mode)")]
        public void IndexedIndirect(byte offsetAddr, ushort indirectAddress, byte A, byte data, byte xOffset, ushort address, bool DecimalMode)
        {
            LoadIndexedIndirect(0x61, address, offsetAddr, xOffset, indirectAddress, data);

            Cpu.A = A;
            Cpu.Status = SetCarry(Cpu.Status, A, false);
            Cpu.SetFlag(Status6502.Decimal, DecimalMode);

            (byte sum, Status6502 status) = DecimalMode ? AddDecimalMode(Cpu.Status, A, data) : AddBinaryMode(Cpu.Status, A, data);
            var steps = Cpu.Step(1);

            FinishTest(sum, Cpu.X, Cpu.Y, status, Cpu.STKP, (ushort)(address + 2), 6UL, steps);
        }

        [DataTestMethod()]

        [TestCategory("IndirectIndexed")]
        [DataRow((byte)0x10, (ushort)0xABCD, (byte)0x01, (byte)0x00, (byte)0x87, (ushort)0xDEAD, false, DisplayName = "T1")]
        [DataRow((byte)0x10, (ushort)0xABCD, (byte)0x01, (byte)0x00, (byte)0x87, (ushort)0xDEAD, true, DisplayName = "T1(Decimal Mode)")]
        [DataRow((byte)0x01, (ushort)0xDBCA, (byte)0x92, (byte)0xFF, (byte)0xEF, (ushort)0xBEEF, false, DisplayName = "T2")]
        [DataRow((byte)0x01, (ushort)0xDBCA, (byte)0x92, (byte)0xFF, (byte)0xEF, (ushort)0xBEEF, true, DisplayName = "T2(Decimal Mode)")]
        [DataRow((byte)0xFA, (ushort)0xEFDA, (byte)0xEE, (byte)0xFA, (byte)0x00, (ushort)0xFEED, false, DisplayName = "T3")]
        [DataRow((byte)0xFA, (ushort)0xEFDA, (byte)0xEE, (byte)0xFA, (byte)0x00, (ushort)0xFEED, true, DisplayName = "T3(Decimal Mode)")]
        [DataRow((byte)0xBE, (ushort)0xCDAB, (byte)0x72, (byte)0x39, (byte)0xBA, (ushort)0x0000, false, DisplayName = "T4")]
        [DataRow((byte)0xBE, (ushort)0xCDAB, (byte)0x72, (byte)0x39, (byte)0xBA, (ushort)0x0000, true, DisplayName = "T4(Decimal Mode)")]
        public void IndirectIndexed(byte offsetAddr, ushort indirectAddress, byte A, byte data, byte yOffset, ushort address, bool DecimalMode)
        {
            LoadIndirectIndexed(0x71, address, offsetAddr, yOffset, indirectAddress, data);

            Cpu.A = A;
            Cpu.Status = SetCarry(Cpu.Status, A, false);
            Cpu.SetFlag(Status6502.Decimal, DecimalMode);

            (byte sum, Status6502 status) = DecimalMode ? AddDecimalMode(Cpu.Status, A, data) : AddBinaryMode(Cpu.Status, A, data);
            var steps = Cpu.Step(1);

            FinishTest(sum, Cpu.X, Cpu.Y, status, Cpu.STKP, (ushort)(address + 2), CheckPageCross(5UL, (ushort)(0x0000 + offsetAddr + yOffset), indirectAddress), steps);
        }
    }
}