namespace Emulator.Tests.Instructions
{
    [TestClass]
    public sealed class NOP : InstructionTests6502
    {
        [DataTestMethod()]
        [TestCategory("Implied")]
        [DataRow((ushort)0x0000, DisplayName = "T1")]
        [DataRow((ushort)0xFFAA, DisplayName = "T2")]
        [DataRow((ushort)0xFACE, DisplayName = "T3")]
        [DataRow((ushort)0xDEAD, DisplayName = "T4")]
        public void Implied(ushort resetVector)
        {
            LoadImplied(0xEA, resetVector);

            Cpu.Reset();

            ulong steps = Cpu.Step(1);

            FinishTest(Cpu.A, Cpu.X, Cpu.Y, Cpu.Status, Cpu.STKP, (ushort)(resetVector + 1), 2UL, steps);
        }
    }
}