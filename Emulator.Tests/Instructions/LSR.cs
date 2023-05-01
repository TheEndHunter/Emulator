using Emulator._6502;

using System.Net;

namespace Emulator.Tests.Instructions
{
    [TestClass]
    public sealed class LSR : CpuInstructionTests
    {
        [DataTestMethod()]

        [TestCategory("Accumulator")]
        [DataRow((byte)0x00, (ushort)0xDEAD, DisplayName = "T1")]
        [DataRow((byte)0xFF, (ushort)0xBEEF, DisplayName = "T2")]
        [DataRow((byte)0xFA, (ushort)0xFEED, DisplayName = "T3")]
        [DataRow((byte)0x39, (ushort)0x0000, DisplayName = "T4")]
        public void Accumulator(byte data, ushort instAddr)
        {
            LoadAccumulator(0x4A, instAddr, data);

            var result = (byte)(data >> 1);
            Status6502 status = SetCarry(SetZN(Cpu.Status, result), data, true);
            var steps = Cpu.Step(1);

            FinishTest(result, Cpu.X, Cpu.Y, status, Cpu.STKP, (ushort)(instAddr + 1), 2UL, steps);
        }

        [DataTestMethod()]

        [TestCategory("ZeroPage")]
        [DataRow((byte)0x10, (byte)0x00, (ushort)0xDEAD, DisplayName = "T1")]
        [DataRow((byte)0x01, (byte)0xFF, (ushort)0xBEEF, DisplayName = "T2")]
        [DataRow((byte)0xFA, (byte)0xFA, (ushort)0xFEED, DisplayName = "T3")]
        [DataRow((byte)0xBE, (byte)0x39, (ushort)0x0000, DisplayName = "T4")]
        public void ZeroPage(byte zpByte, byte data, ushort address)
        {
            LoadZeroPage(0x46, address, zpByte, data);

            var result = (byte)(data >> 1);
            Status6502 status = SetCarry(SetZN(Cpu.Status, result), data, true);
            var steps = Cpu.Step(1);

            Assert.AreEqual(result, Ram.ReadByte((ushort)(0x0000 + zpByte)), "Value is not equal");
            FinishTest(Cpu.A, Cpu.X, Cpu.Y, status, Cpu.STKP, (ushort)(address + 2), 5UL, steps);
        }

        [DataTestMethod()]

        [TestCategory("ZeroPageX")]
        [DataRow((byte)0x10, (byte)0x00, (byte)0x87, (ushort)0xDEAD, DisplayName = "T1")]
        [DataRow((byte)0x01, (byte)0xFF, (byte)0xEF, (ushort)0xBEEF, DisplayName = "T2")]
        [DataRow((byte)0xFA, (byte)0xFA, (byte)0x00, (ushort)0xFEED, DisplayName = "T3")]
        [DataRow((byte)0xBE, (byte)0x39, (byte)0xBA, (ushort)0x0000, DisplayName = "T4")]
        public void ZeroPageX(byte zpAddr, byte data, byte xOffset, ushort address)
        {
            ushort ZPOAddress = (ushort)(0x0000 + (byte)(zpAddr + xOffset));
            Ram.WriteWord(0xFFFC, address);
            Ram.WriteByte(address, 0x56);
            Ram.WriteByte((ushort)(address + 1), zpAddr);
            Ram.WriteByte(ZPOAddress, data);

            Cpu.Reset();
            byte result = (byte)(data >> 1);
            Cpu.X = xOffset;
            var steps = Cpu.Step(1);
            var status = SetCarry(SetZN(Cpu.Status, result), data, true);

            Assert.AreEqual(6UL, steps, "Cycles are incorrect");
            Assert.AreEqual((ushort)(address + 2), Cpu.PC, "PC is incorrect");
            Assert.AreEqual(result, Ram.ReadByte(ZPOAddress), "Value is incorrect");
            Assert.AreEqual(status, Cpu.Status, "Status incorrect");
        }

        [DataTestMethod()]

        [TestCategory("Absolute")]
        [DataRow((ushort)0xAB10, (byte)0x00, (ushort)0xDEAD, DisplayName = "T1")]
        [DataRow((ushort)0xEF01, (byte)0xFF, (ushort)0xBEEF, DisplayName = "T2")]
        [DataRow((ushort)0xCDFA, (byte)0xFA, (ushort)0xFEED, DisplayName = "T3")]
        [DataRow((ushort)0xAABE, (byte)0x39, (ushort)0x0000, DisplayName = "T4")]
        public void Absolute(ushort absAddr, byte data, ushort address)
        {
            Ram.WriteWord(0xFFFC, address);
            Ram.WriteByte(address, 0x4E);
            Ram.WriteWord((ushort)(address + 1), absAddr);
            Ram.WriteByte(absAddr, data);

            Cpu.Reset();
            byte result = (byte)(data >> 1);
            var steps = Cpu.Step(1);
            var status = SetCarry(SetZN(Cpu.Status, result), data, true);

            Assert.AreEqual(6UL, steps, "Cycles are incorrect");
            Assert.AreEqual((ushort)(address + 3), Cpu.PC, "PC is incorrect");
            Assert.AreEqual(result, Ram.ReadByte(absAddr), "Value is incorrect");
            Assert.AreEqual(status, Cpu.Status, "Status incorrect");

        }

        [DataTestMethod()]

        [TestCategory("AbsoluteX")]
        [DataRow((ushort)0xAB10, (byte)0x00, (byte)0x87, (ushort)0xDEAD, DisplayName = "T1")]
        [DataRow((ushort)0xEF01, (byte)0xFF, (byte)0xEF, (ushort)0xBEEF, DisplayName = "T2")]
        [DataRow((ushort)0xCDFA, (byte)0xFA, (byte)0x00, (ushort)0xFEED, DisplayName = "T3")]
        [DataRow((ushort)0xAABE, (byte)0x39, (byte)0xBA, (ushort)0x0000, DisplayName = "T4")]
        public void AbsoluteX(ushort absAddr, byte data, byte xOffset, ushort address)
        {
            ushort fAddr = (ushort)(absAddr + xOffset);
            Ram.WriteWord(0xFFFC, address);
            Ram.WriteByte(address, 0x5E);
            Ram.WriteWord((ushort)(address + 1), absAddr);
            Ram.WriteByte(fAddr, data);

            Cpu.Reset();

            Cpu.X = xOffset;

            byte result = (byte)(data >> 1);
            var steps = Cpu.Step(1);
            var status = SetCarry(SetZN(Cpu.Status, result), data, true);

            Assert.AreEqual(7UL, steps, "Cycles are incorrect");
            Assert.AreEqual((ushort)(address + 3), Cpu.PC, "PC is incorrect");
            Assert.AreEqual(result, Ram.ReadByte(fAddr), "Value is incorrect");
            Assert.AreEqual(status, Cpu.Status, "Status incorrect");
        }
    }
}