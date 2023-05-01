using Emulator._6502;

namespace Emulator.Tests.Instructions
{
    [TestClass]
    public sealed class LDY : CpuInstructionTests
    {
        [DataTestMethod()]

        [TestCategory("Immediate")]
        [DataRow((byte)0x00, (ushort)0xDEAD, DisplayName = "T1")]
        [DataRow((byte)0xFF, (ushort)0xBEEF, DisplayName = "T2")]
        [DataRow((byte)0xFA, (ushort)0xFEED, DisplayName = "T3")]
        [DataRow((byte)0x39, (ushort)0x0000, DisplayName = "T4")]
        public void Immediate(byte data, ushort address)
        {
            Ram.WriteWord(0xFFFC, address);
            Ram.WriteByte(address, 0xA0);
            Ram.WriteByte((ushort)(address + 1), data);
            Cpu.Reset();
            var status = SetZN(Cpu.Status, data);
            var steps = Cpu.Step(1);

            Assert.AreEqual(2UL, steps, "Cycles are incorrect");
            Assert.AreEqual((ushort)(address + 2), Cpu.PC, "PC is incorrect");
            Assert.AreEqual(data, Cpu.Y, "Y is incorrect");
            Assert.AreEqual(status, Cpu.Status, "Status incorrect");

        }

        [DataTestMethod()]

        [TestCategory("ZeroPage")]
        [DataRow((byte)0x10, (byte)0x00, (ushort)0xDEAD, DisplayName = "T1")]
        [DataRow((byte)0x01, (byte)0xFF, (ushort)0xBEEF, DisplayName = "T2")]
        [DataRow((byte)0xFA, (byte)0xFA, (ushort)0xFEED, DisplayName = "T3")]
        [DataRow((byte)0xBE, (byte)0x39, (ushort)0x0000, DisplayName = "T4")]
        public void ZeroPage(byte zpAddr, byte data, ushort address)
        {
            Ram.WriteWord(0xFFFC, address);
            Ram.WriteByte(address, 0xA4);
            Ram.WriteByte((ushort)(address + 1), zpAddr);
            Ram.WriteByte((ushort)(0x0000 + zpAddr), data);

            Cpu.Reset();
            var status = SetZN(Cpu.Status, data);
            var steps = Cpu.Step(1);

            Assert.AreEqual(3UL, steps, "Cycles are incorrect");
            Assert.AreEqual((ushort)(address + 2), Cpu.PC, "PC is incorrect");
            Assert.AreEqual(data, Cpu.Y, "Y is incorrect");
            Assert.AreEqual(status, Cpu.Status, "Status incorrect");

        }

        [DataTestMethod()]

        [TestCategory("ZeroPageX")]
        [DataRow((byte)0x10, (byte)0x00, (byte)0x87, (ushort)0xDEAD, DisplayName = "T1")]
        [DataRow((byte)0x01, (byte)0xFF, (byte)0xEF, (ushort)0xBEEF, DisplayName = "T2")]
        [DataRow((byte)0xFA, (byte)0xFA, (byte)0x00, (ushort)0xFEED, DisplayName = "T3")]
        [DataRow((byte)0xBE, (byte)0x39, (byte)0xBA, (ushort)0x0000, DisplayName = "T4")]
        public void ZeroPageX(byte zpAddr, byte data, byte xOffset, ushort address)
        {
            Ram.WriteWord(0xFFFC, address);
            Ram.WriteByte(address, 0xB4);
            Ram.WriteByte((ushort)(address + 1), zpAddr);
            Ram.WriteByte((ushort)(0x0000 + (byte)(zpAddr + xOffset)), data);

            Cpu.Reset();
            var status = SetZN(Cpu.Status, data);
            /*
             * this is set to value manually to avoid running
             * the CPU longer than needed for test.
             */
            Cpu.X = xOffset;
            var steps = Cpu.Step(1);

            Assert.AreEqual(4UL, steps, "Cycles are incorrect");
            Assert.AreEqual((ushort)(address + 2), Cpu.PC, "PC is incorrect");
            Assert.AreEqual(data, Cpu.Y, "Y is incorrect");
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
            Ram.WriteByte(address, 0xAC);
            Ram.WriteWord((ushort)(address + 1), absAddr);
            Ram.WriteByte(absAddr, data);

            Cpu.Reset();
            var status = SetZN(Cpu.Status, data);
            var steps = Cpu.Step(1);

            Assert.AreEqual(4UL, steps, "Cycles are incorrect");
            Assert.AreEqual((ushort)(address + 3), Cpu.PC, "PC is incorrect");
            Assert.AreEqual(data, Cpu.Y, "Y is incorrect");
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
            Ram.WriteWord(0xFFFC, address);
            Ram.WriteByte(address, 0xBC);
            Ram.WriteWord((ushort)(address + 1), absAddr);
            Ram.WriteByte((ushort)(absAddr + xOffset), data);

            Cpu.Reset();
            var status = SetZN(Cpu.Status, data);
            /*
             * this is set to value manually to avoid running
             * the CPU longer than needed for test.
             */
            Cpu.X = xOffset;
            var steps = Cpu.Step(1);

            Assert.AreEqual(CyclesCheck(4UL, xOffset, absAddr), steps, "Cycles are incorrect");
            Assert.AreEqual((ushort)(address + 3), Cpu.PC, "PC is incorrect");
            Assert.AreEqual(data, Cpu.Y, "Y is incorrect");
            Assert.AreEqual(status, Cpu.Status, "Status incorrect");
        }
    }
}