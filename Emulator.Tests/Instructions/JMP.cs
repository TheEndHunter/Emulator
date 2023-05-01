namespace Emulator.Tests.Instructions
{
    [TestClass]
    public sealed class JMP : InstructionTests6502
    {
        [DataTestMethod()]

        [TestCategory("Indirect")]
        [DataRow((ushort)0x0000, (ushort)0xDEAD, (ushort)0xABCD, DisplayName = "T1")]
        [DataRow((ushort)0xFFAA, (ushort)0xBEEF, (ushort)0xDBCA, DisplayName = "T2")]
        [DataRow((ushort)0xFACE, (ushort)0xFEED, (ushort)0xEEFF, DisplayName = "T3")]
        [DataRow((ushort)0xDEAD, (ushort)0xeaff, (ushort)0xCFDB, DisplayName = "T4")]
        public void Indirect(ushort resetVector, ushort indaddr, ushort addr)
        {
            LoadIndirect(0x6C, resetVector, indaddr, addr);

            Cpu.Reset();

            ulong steps = Cpu.Step(1);

            FinishTest(Cpu.A, Cpu.X, Cpu.Y, Cpu.Status, Cpu.STKP, addr, 5UL, steps);
        }

        [DataTestMethod()]

        [TestCategory("Absolute")]
        [DataRow((ushort)0x0000, (ushort)0xDEAD, DisplayName = "T1")]
        [DataRow((ushort)0xFFAA, (ushort)0xBEEF, DisplayName = "T2")]
        [DataRow((ushort)0xFACE, (ushort)0xFEED, DisplayName = "T3")]
        [DataRow((ushort)0xDEAD, (ushort)0x0000, DisplayName = "T4")]
        public void Absolute(ushort resetVector, ushort addr)
        {
            LoadAbsolute(0x4C, resetVector, addr, 0x00);

            Cpu.Reset();

            ulong steps = Cpu.Step(1);

            FinishTest(Cpu.A, Cpu.X, Cpu.Y, Cpu.Status, Cpu.STKP, addr, 3UL, steps);
        }
    }
}