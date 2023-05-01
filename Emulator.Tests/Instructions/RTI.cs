using Emulator._6502;

using System.Buffers.Binary;
using System.Net.NetworkInformation;

namespace Emulator.Tests.Instructions
{
    [TestClass]
    public sealed class RTI : InstructionTests6502
    {
        [DataTestMethod()]
        [TestCategory("Implied")]
        [DataRow((ushort)0x0000, (ushort)0xDEAD, DisplayName = "T1")]
        [DataRow((ushort)0xFFAA, (ushort)0xBEEF, DisplayName = "T2")]
        [DataRow((ushort)0xFACE, (ushort)0xFEED, DisplayName = "T3")]
        [DataRow((ushort)0x0000, (ushort)0x00FF, DisplayName = "T4")]
        public void Implied(ushort resetVector, ushort interruptVector)
        {
            Ram.WriteWord(0xFFFC, resetVector);//Reset Vector Location
            Ram.WriteWord(0xFFFE, interruptVector);//Interrupt vector Location
            Ram.WriteByte(interruptVector, 0x40);//RTI instruction call


            byte[] bytes = new byte[2];
            byte rLo;
            byte rHi;
            BinaryPrimitives.WriteUInt16LittleEndian(bytes, (ushort)(resetVector + 1));

            if (BitConverter.IsLittleEndian)
            {
                rLo = bytes[0];
                rHi = bytes[1];
            }
            else
            {
                rLo = bytes[1];
                rHi = bytes[0];
            }

            Cpu.Reset();

            ushort pcOriginal = (ushort)(Cpu.PC + 1);
            byte stkpOriginal = Cpu.STKP;
            Status6502 status = Cpu.Status;
            var bstatus = SetBreak(status, true);

            ulong steps = Cpu.Step(1);


            FinishTest(Cpu.A, Cpu.X, Cpu.Y, bstatus, (byte)(stkpOriginal - 3), interruptVector, 7UL, steps);
            Assert.AreEqual(pcOriginal, Ram.ReadWord((ushort)(0x0100 | stkpOriginal - 1)), "Stack PC incorrect");
            Assert.AreEqual(status, (Status6502)Ram.ReadByte((ushort)(0x0100 | stkpOriginal - 2)), "Stack Status incorrect");

            steps += Cpu.Step(1);

            FinishTest(Cpu.A, Cpu.X, Cpu.Y, status, stkpOriginal, (ushort)(resetVector + 1), 13UL, steps);
        }
    }
}