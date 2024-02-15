using Emulator._6502;

namespace Emulator.Tests.Instructions
{
    [TestClass]
    public sealed class BRK : InstructionTests6502
    {
        [DataTestMethod()]

        [TestCategory("Implied")]
        [DataRow((ushort)0x0000, (ushort)0xDEAD, DisplayName = "T1")]
        [DataRow((ushort)0xFFAA, (ushort)0xBEEF, DisplayName = "T2")]
        [DataRow((ushort)0xFACE, (ushort)0xFEED, DisplayName = "T3")]
        [DataRow((ushort)0x0000, (ushort)0x0000, DisplayName = "T4")]
        public void Implied(ushort resetVector, ushort interruptVector)
        {
            Ram.WriteWord(0xFFFC, resetVector);//Reset Vector
            Ram.WriteWord(0xFFFE, interruptVector);//Interupt Vector

            Cpu.Reset();

            byte stkpOriginal = Cpu.STKP;
            Status6502 status = Cpu.Status;
            var bstatus = SetBreak(status, true);

            ulong steps = Cpu.Step(1);

            FinishTest(Cpu.A, Cpu.X, Cpu.Y, bstatus, (byte)(stkpOriginal - 3), interruptVector, 7UL, steps);
            Assert.AreEqual(status, (Status6502)Ram.ReadByte((ushort)(0x0100 | stkpOriginal - 2)), "Stack Status incorrect");
            Assert.AreEqual(status, (Status6502)Ram.ReadByte((ushort)(0x0100 | stkpOriginal - 2)), "Stack Status incorrect");
        }
    }
}