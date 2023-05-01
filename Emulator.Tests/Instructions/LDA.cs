using Emulator._6502;
using Emulator.NES.Devices;

using System.Net.NetworkInformation;

namespace Emulator.Tests.Instructions
{
    [TestClass]
    public sealed class LDA : InstructionTests6502
    {
        [DataTestMethod()]

        [TestCategory("Immediate")]
        [DataRow((byte)0x00, (ushort)0xDEAD, DisplayName = "T1")]
        [DataRow((byte)0xFF, (ushort)0xBEEF, DisplayName = "T2")]
        [DataRow((byte)0xFA, (ushort)0xFEED, DisplayName = "T3")]
        [DataRow((byte)0x39, (ushort)0x0000, DisplayName = "T4")]
        public void Immediate(byte data, ushort address)
        {
            LoadImmediate(0xA9, address, data);
            var status = SetNegative(SetZero(Cpu.Status, data), data);
            var steps = Cpu.Step(1);

            FinishTest(data, Cpu.X, Cpu.Y, status, Cpu.STKP, (ushort)(address + 2), 2UL, steps);
        }

        [DataTestMethod()]

        [TestCategory("ZeroPage")]
        [DataRow((byte)0x10, (byte)0x00, (ushort)0xDEAD, DisplayName = "T1")]
        [DataRow((byte)0x01, (byte)0xFF, (ushort)0xBEEF, DisplayName = "T2")]
        [DataRow((byte)0xFA, (byte)0xFA, (ushort)0xFEED, DisplayName = "T3")]
        [DataRow((byte)0xBE, (byte)0x39, (ushort)0x0000, DisplayName = "T4")]
        public void ZeroPage(byte zpAddr, byte data, ushort address)
        {
            LoadZeroPage(0xA5, address, zpAddr, data);
            var status = SetNegative(SetZero(Cpu.Status, data), data);
            var steps = Cpu.Step(1);

            FinishTest(data, Cpu.X, Cpu.Y, status, Cpu.STKP, (ushort)(address + 2), 3UL, steps);
        }

        [DataTestMethod()]

        [TestCategory("ZeroPageX")]
        [DataRow((byte)0x10, (byte)0x00, (byte)0x87, (ushort)0xDEAD, DisplayName = "T1")]
        [DataRow((byte)0x01, (byte)0xFF, (byte)0xEF, (ushort)0xBEEF, DisplayName = "T2")]
        [DataRow((byte)0xFA, (byte)0xFA, (byte)0x00, (ushort)0xFEED, DisplayName = "T3")]
        [DataRow((byte)0xBE, (byte)0x39, (byte)0xBA, (ushort)0x0000, DisplayName = "T4")]
        public void ZeroPageX(byte zpAddr, byte data, byte xOffset, ushort address)
        {
            LoadZeroPageX(0xB5, address, zpAddr, xOffset, data);
            var status = SetNegative(SetZero(Cpu.Status, data), data);
            var steps = Cpu.Step(1);

            FinishTest(data, Cpu.X, Cpu.Y, status, Cpu.STKP, (ushort)(address + 2), 4UL, steps);
        }

        [DataTestMethod()]

        [TestCategory("Absolute")]
        [DataRow((ushort)0xAB10, (byte)0x00, (ushort)0xDEAD, DisplayName = "T1")]
        [DataRow((ushort)0xEF01, (byte)0xFF, (ushort)0xBEEF, DisplayName = "T2")]
        [DataRow((ushort)0xCDFA, (byte)0xFA, (ushort)0xFEED, DisplayName = "T3")]
        [DataRow((ushort)0xAABE, (byte)0x39, (ushort)0x0000, DisplayName = "T4")]
        public void Absolute(ushort absAddr, byte data, ushort address)
        {
            LoadAbsolute(0xAD, address, absAddr, data);
            var status = SetNegative(SetZero(Cpu.Status, data), data);
            var steps = Cpu.Step(1);

            FinishTest(data, Cpu.X, Cpu.Y, status, Cpu.STKP, (ushort)(address + 3), 4UL, steps);
        }

        [DataTestMethod()]

        [TestCategory("AbsoluteX")]
        [DataRow((ushort)0xAB10, (byte)0x00, (byte)0x87, (ushort)0xDEAD, DisplayName = "T1")]
        [DataRow((ushort)0xEF01, (byte)0xFF, (byte)0xEF, (ushort)0xBEEF, DisplayName = "T2")]
        [DataRow((ushort)0xCDFA, (byte)0xFA, (byte)0x00, (ushort)0xFEED, DisplayName = "T3")]
        [DataRow((ushort)0xAABE, (byte)0x39, (byte)0xBA, (ushort)0x0000, DisplayName = "T4")]
        public void AbsoluteX(ushort absAddr, byte data, byte xOffset, ushort address)
        {
            LoadAbsoluteX(0xBD, address, absAddr, xOffset, data);
            var status = SetNegative(SetZero(Cpu.Status, data), data);
            var steps = Cpu.Step(1);

            FinishTest(data, Cpu.X, Cpu.Y, status, Cpu.STKP, (ushort)(address + 3), CheckPageCross(4UL, absAddr, (ushort)(absAddr + xOffset)), steps);
        }

        [DataTestMethod()]

        [TestCategory("AbsoluteY")]
        [DataRow((ushort)0xAB10, (byte)0x00, (byte)0x87, (ushort)0xDEAD, DisplayName = "T1")]
        [DataRow((ushort)0xEF01, (byte)0xFF, (byte)0xEF, (ushort)0xBEEF, DisplayName = "T2")]
        [DataRow((ushort)0xCDFA, (byte)0xFA, (byte)0x00, (ushort)0xFEED, DisplayName = "T3")]
        [DataRow((ushort)0xAABE, (byte)0x39, (byte)0xBA, (ushort)0x0000, DisplayName = "T4")]
        public void AbsoluteY(ushort absAddr, byte data, byte yOffset, ushort address)
        {
            LoadAbsoluteY(0xB9, address, absAddr, yOffset, data);
            var status = SetNegative(SetZero(Cpu.Status, data), data);
            var steps = Cpu.Step(1);

            FinishTest(data, Cpu.X, Cpu.Y, status, Cpu.STKP, (ushort)(address + 3), CheckPageCross(4UL, absAddr, (ushort)(absAddr + yOffset)), steps);
        }

        [DataTestMethod()]

        [TestCategory("IndexedIndirect")]
        [DataRow((byte)0x10, (ushort)0xABCD, (byte)0x00, (byte)0x87, (ushort)0xDEAD, DisplayName = "T1")]
        [DataRow((byte)0x01, (ushort)0xDBCA, (byte)0xFF, (byte)0xEF, (ushort)0xBEEF, DisplayName = "T2")]
        [DataRow((byte)0xFA, (ushort)0xEFDA, (byte)0xFA, (byte)0x00, (ushort)0xFEED, DisplayName = "T3")]
        [DataRow((byte)0xBE, (ushort)0xCDAB, (byte)0x39, (byte)0xBA, (ushort)0x0000, DisplayName = "T4")]
        public void IndexedIndirect(byte offsetAddr, ushort indaddr, byte data, byte xOffset, ushort address)
        {
            LoadIndexedIndirect(0xA1, address, offsetAddr, xOffset, indaddr, data);
            var status = SetNegative(SetZero(Cpu.Status, data), data);
            var steps = Cpu.Step(1);

            FinishTest(data, Cpu.X, Cpu.Y, status, Cpu.STKP, (ushort)(address + 2), 6UL, steps);
        }

        [DataTestMethod()]

        [TestCategory("IndirectIndexed")]
        [DataRow((byte)0x10, (ushort)0xABCD, (byte)0x00, (byte)0x87, (ushort)0xDEAD, DisplayName = "T1")]
        [DataRow((byte)0x01, (ushort)0xDBCA, (byte)0xFF, (byte)0xEF, (ushort)0xBEEF, DisplayName = "T2")]
        [DataRow((byte)0xFA, (ushort)0xEFDA, (byte)0xFA, (byte)0x00, (ushort)0xFEED, DisplayName = "T3")]
        [DataRow((byte)0xBE, (ushort)0xCDAB, (byte)0x39, (byte)0xBA, (ushort)0x0000, DisplayName = "T4")]
        public void IndirectIndexed(byte offsetAddr, ushort indaddr, byte data, byte yOffset, ushort address)
        {
            LoadIndirectIndexed(0xB1, address, offsetAddr, yOffset, indaddr, data);
            var status = SetNegative(SetZero(Cpu.Status, data), data);
            var steps = Cpu.Step(1);

            FinishTest(data, Cpu.X, Cpu.Y, status, Cpu.STKP, (ushort)(address + 2), CheckPageCross(5UL, (ushort)(0x0000 + offsetAddr + yOffset), indaddr), steps);
        }
    }
}