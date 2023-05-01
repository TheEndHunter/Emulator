namespace Emulator.Tests.Instructions
{
    [TestClass]
    public sealed class LDX : CpuInstructionTests
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
            Ram.WriteByte(address, 0xA2);
            Ram.WriteByte((ushort)(address + 1), data);
            Cpu.Reset();
            var status = SetZN(Cpu.Status, data);
            var steps = Cpu.Step(1);

            Assert.AreEqual(2UL, steps, "Cycles are incorrect");
            Assert.AreEqual((ushort)(address + 2), Cpu.PC, "PC is incorrect");
            Assert.AreEqual(data, Cpu.X, "X is incorrect");
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
            Ram.WriteByte(address, 0xA6);
            Ram.WriteByte((ushort)(address + 1), zpAddr);
            Ram.WriteByte((ushort)(0x0000 + zpAddr), data);

            Cpu.Reset();
            var status = SetZN(Cpu.Status, data);
            var steps = Cpu.Step(1);

            Assert.AreEqual(3UL, steps, "Cycles are incorrect");
            Assert.AreEqual((ushort)(address + 2), Cpu.PC, "PC is incorrect");
            Assert.AreEqual(data, Cpu.X, "X is incorrect");
            Assert.AreEqual(status, Cpu.Status, "Status incorrect");

        }

        [DataTestMethod()]

        [TestCategory("ZeroPageY")]
        [DataRow((byte)0x10, (byte)0x00, (byte)0x87, (ushort)0xDEAD, DisplayName = "T1")]
        [DataRow((byte)0x01, (byte)0xFF, (byte)0xEF, (ushort)0xBEEF, DisplayName = "T2")]
        [DataRow((byte)0xFA, (byte)0xFA, (byte)0x00, (ushort)0xFEED, DisplayName = "T3")]
        [DataRow((byte)0xBE, (byte)0x39, (byte)0xBA, (ushort)0x0000, DisplayName = "T4")]
        public void ZeroPageY(byte zpAddr, byte data, byte yOffset, ushort address)
        {
            Ram.WriteWord(0xFFFC, address);
            Ram.WriteByte(address, 0xB6);
            Ram.WriteByte((ushort)(address + 1), zpAddr);
            Ram.WriteByte((ushort)(0x0000 + (byte)(zpAddr + yOffset)), data);

            Cpu.Reset();
            var status = SetZN(Cpu.Status, data);
            /*
             * this is set to value manually to avoid running
             * the CPU longer than needed for test.
             */
            Cpu.Y = yOffset;
            var steps = Cpu.Step(1);

            Assert.AreEqual(4UL, steps, "Cycles are incorrect");
            Assert.AreEqual((ushort)(address + 2), Cpu.PC, "PC is incorrect");
            Assert.AreEqual(data, Cpu.X, "X is incorrect");
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
            Ram.WriteByte(address, 0xAE);
            Ram.WriteWord((ushort)(address + 1), absAddr);
            Ram.WriteByte(absAddr, data);

            Cpu.Reset();
            var status = SetZN(Cpu.Status, data);
            var steps = Cpu.Step(1);

            Assert.AreEqual(4UL, steps, "Cycles are incorrect");
            Assert.AreEqual((ushort)(address + 3), Cpu.PC, "PC is incorrect");
            Assert.AreEqual(data, Cpu.X, "X is incorrect");
            Assert.AreEqual(status, Cpu.Status, "Status incorrect");

        }

        [DataTestMethod()]

        [TestCategory("AbsoluteY")]
        [DataRow((ushort)0xAB10, (byte)0x00, (byte)0x87, (ushort)0xDEAD, DisplayName = "T1")]
        [DataRow((ushort)0xEF01, (byte)0xFF, (byte)0xEF, (ushort)0xBEEF, DisplayName = "T2")]
        [DataRow((ushort)0xCDFA, (byte)0xFA, (byte)0x00, (ushort)0xFEED, DisplayName = "T3")]
        [DataRow((ushort)0xAABE, (byte)0x39, (byte)0xBA, (ushort)0x0000, DisplayName = "T4")]
        public void AbsoluteY(ushort absAddr, byte data, byte yOffset, ushort address)
        {
            Ram.WriteWord(0xFFFC, address);
            Ram.WriteByte(address, 0xBE);
            Ram.WriteWord((ushort)(address + 1), absAddr);
            Ram.WriteByte((ushort)(absAddr + yOffset), data);

            Cpu.Reset();
            var status = SetZN(Cpu.Status, data);
            /*
             * this is set to value manually to avoid running
             * the CPU longer than needed for test.
             */
            Cpu.Y = yOffset;
            var steps = Cpu.Step(1);

            Assert.AreEqual(CyclesCheck(4UL, yOffset, absAddr), steps, "Cycles are incorrect");
            Assert.AreEqual((ushort)(address + 3), Cpu.PC, "PC is incorrect");
            Assert.AreEqual(data, Cpu.X, "X is incorrect");
            Assert.AreEqual(status, Cpu.Status, "Status incorrect");
        }
    }
}