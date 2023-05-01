using Emulator._6502;

using System.Buffers.Binary;

namespace Emulator.Tests.Instructions
{
    public abstract class InstructionTests6502
    {
        protected Cpu6502 Cpu { get; private set; }
        protected Ram6502 Ram { get; private set; }

        public Status6502 SetNegative(Status6502 current, byte data)
        {
            return SetStatus(current, Status6502.Negative, (data & 0x80) > 0);
        }

        public Status6502 SetCarry(Status6502 current, byte data, bool borrow)
        {
            return SetStatus(current, Status6502.Carry, borrow ? (data & 0x01) == 0x01 : (data & 0x80) > 0);
        }

        public Status6502 SetDecimal(Status6502 current, bool set)
        {
            return SetStatus(current, Status6502.Decimal, set);
        }

        public Status6502 SetOverflow(Status6502 current, byte last, byte curdata)
        {
            return SetStatus(current, Status6502.OverFlow, (byte)(last & 0x80) != (byte)(curdata & 0x80));
        }

        public Status6502 SetZero(Status6502 current, byte data)
        {
            return SetStatus(current, Status6502.Zero, data == 0);
        }

        public Status6502 SetBreak(Status6502 current, bool set)
        {
            return SetStatus(current, Status6502.Break, set);
        }

        public Status6502 SetDisableInterrupt(Status6502 current, bool set)
        {
            return SetStatus(current, Status6502.InterruptDisable, set);
        }

        public Status6502 SetStatus(Status6502 current, Status6502 flag, bool condition)
        {
            Status6502 status = current;
            if (condition)
            {
                status |= flag;
            }
            else
            {
                status &= ~flag;
            }
            return status;
        }

        public void CheckStatus(Status6502 original)
        {
            Assert.AreEqual(original, Cpu.Status, "Status Incorrect");
        }

        private void CheckRegisters(byte A, byte X, byte Y, byte STKP, ushort PC)
        {
            Assert.AreEqual(A, Cpu.A, "A is not equal");
            Assert.AreEqual(X, Cpu.X, "offsetAddr is not equal");
            Assert.AreEqual(Y, Cpu.Y, "Y is not equal");
            Assert.AreEqual(STKP, Cpu.STKP, "STKP is not equal");
            Assert.AreEqual(PC, Cpu.PC, "PC is not equal");
        }

        private void CheckCycles(ulong cycles, ulong current)
        {
            Assert.AreEqual(cycles, current, "Cycles are not equal");
        }

        public void FinishTest(byte A, byte X, byte Y, Status6502 status, byte STKP, ushort PC, ulong cycles, ulong currentCycles)
        {
            CheckStatus(status);
            CheckRegisters(A, X, Y, STKP, PC);
            CheckCycles(cycles, currentCycles);
        }

        [TestInitialize()]
        public void Init()
        {
            Cpu = new Cpu6502();
            Ram = new Ram6502();

            Cpu.RegisterDevice(Ram);
        }

        public void LoadImplied(byte opcode, ushort resetVector)
        {
            Ram.WriteWord(0xFFFC, resetVector);
            Ram.WriteByte(resetVector, opcode);
        }

        public void LoadImmediate(byte opcode, ushort resetVector, byte data)
        {
            Ram.WriteWord(0xFFFC, resetVector);
            Ram.WriteByte(resetVector, opcode);
            Ram.WriteByte((ushort)(resetVector + 1), data);

            Cpu.Reset();
        }

        public static ushort GetNextIndirectAddr(ushort addr)
        {
            byte loAddr = BitConverter.IsLittleEndian ? (byte)(addr & 0xFF) : (byte)(addr >> 8);
            byte hiAddr = BitConverter.IsLittleEndian ? (byte)(addr >> 8) : (byte)(addr & 0xFF);
            ;
            if (loAddr == 0xFF)
            {
                return (ushort)(hiAddr << 8);
            }
            else
            {
                return (ushort)(addr + 1);
            }
        }

        public void LoadAccumulator(byte opcode, ushort resetVector, byte data)
        {
            Ram.WriteWord(0xFFFC, resetVector);
            Ram.WriteByte(resetVector, opcode);
            Cpu.Reset();
            Cpu.A = data;
        }
        public void LoadRelative(byte opcode, ushort resetVector, byte data)
        {
            Ram.WriteWord(0xFFFC, resetVector);
            Ram.WriteByte(resetVector, opcode);
            Ram.WriteByte((ushort)(resetVector + 1), data);

            Cpu.Reset();
        }

        public void LoadIndirect(byte opcode, ushort resetVector, ushort indAddr, ushort addr)
        {
            byte[] bytes = new byte[2];
            BinaryPrimitives.WriteUInt16LittleEndian(bytes, addr);

            Ram.WriteWord(0xFFFC, resetVector);
            Ram.WriteByte(resetVector, opcode);
            Ram.WriteWord((ushort)(resetVector + 1), indAddr);

            Ram.WriteByte(indAddr, bytes[0]);
            Ram.WriteByte(GetNextIndirectAddr(indAddr), bytes[1]);

            Cpu.Reset();
        }

        public void LoadAbsolute(byte opcode, ushort resetVector, ushort dataAddr, byte data)
        {
            Ram.WriteWord(0xFFFC, resetVector);
            Ram.WriteByte(resetVector, opcode);
            Ram.WriteWord((ushort)(resetVector + 1), dataAddr);
            Ram.WriteByte(dataAddr, data);

            Cpu.Reset();
        }
        public void LoadZeroPage(byte opcode, ushort resetVector, byte dataAddr, byte data)
        {
            ushort zpAddr = (ushort)(0x0000 + dataAddr);
            Ram.WriteWord(0xFFFC, resetVector);
            Ram.WriteByte(resetVector, opcode);
            Ram.WriteByte((ushort)(resetVector + 1), dataAddr);
            Ram.WriteByte(zpAddr, data);

            Cpu.Reset();
        }
        public void LoadIndirect(byte opcode, ushort resetVector, ushort indirectAddr, ushort dataAddr, byte data)
        {
            Ram.WriteWord(0xFFFC, resetVector);
            Ram.WriteByte(resetVector, opcode);
            Ram.WriteWord((ushort)(resetVector + 1), indirectAddr);
            Ram.WriteWord(indirectAddr, dataAddr);
            Ram.WriteByte(dataAddr, data);

            Cpu.Reset();
        }
        public void LoadAbsoluteX(byte opcode, ushort resetVector, ushort dataAddr, byte xOffset, byte data)
        {
            Ram.WriteWord(0xFFFC, resetVector);
            Ram.WriteByte(resetVector, opcode);
            Ram.WriteWord((ushort)(resetVector + 1), dataAddr);
            Ram.WriteByte((ushort)(dataAddr + xOffset), data);

            Cpu.Reset();
            Cpu.X = xOffset;
        }
        public void LoadAbsoluteY(byte opcode, ushort resetVector, ushort dataAddr, byte yOffset, byte data)
        {
            Ram.WriteWord(0xFFFC, resetVector);
            Ram.WriteByte(resetVector, opcode);
            Ram.WriteWord((ushort)(resetVector + 1), dataAddr);
            Ram.WriteByte((ushort)(dataAddr + yOffset), data);

            Cpu.Reset();
            Cpu.Y = yOffset;
        }
        public void LoadZeroPageX(byte opcode, ushort resetVector, byte dataAddr, byte xOffset, byte data)
        {
            ushort zpAddr = (ushort)(0x0000 + (byte)(dataAddr + xOffset));
            Ram.WriteWord(0xFFFC, resetVector);
            Ram.WriteByte(resetVector, opcode);
            Ram.WriteByte((ushort)(resetVector + 1), dataAddr);
            Ram.WriteByte(zpAddr, data);

            Cpu.Reset();
            Cpu.X = xOffset;
        }
        public void LoadZeroPageY(byte opcode, ushort resetVector, byte dataAddr, byte yOffset, byte data)
        {
            ushort zpAddr = (ushort)(0x0000 + (byte)(dataAddr + yOffset));
            Ram.WriteWord(0xFFFC, resetVector);
            Ram.WriteByte(resetVector, opcode);
            Ram.WriteByte((ushort)(resetVector + 1), dataAddr);
            Ram.WriteByte(zpAddr, data);

            Cpu.Reset();
            Cpu.Y = yOffset;
        }
        public void LoadIndirectIndexed(byte opcode, ushort resetVector, byte indirectAddr, byte yOffset, ushort dataAddr, byte data)
        {
            Ram.WriteWord(0xFFFC, resetVector);
            Ram.WriteByte(resetVector, opcode);
            Ram.WriteByte((ushort)(resetVector + 1), indirectAddr);
            Ram.WriteWord((ushort)(0x0000 + indirectAddr), dataAddr);
            Ram.WriteByte((ushort)(dataAddr + yOffset), data);

            Cpu.Reset();
            Cpu.Y = yOffset;
        }
        public void LoadIndexedIndirect(byte opcode, ushort resetVector, byte indirectAddr, byte xOffset, ushort dataAddr, byte data)
        {
            Ram.WriteWord(0xFFFC, resetVector);
            Ram.WriteByte(resetVector, opcode);
            Ram.WriteByte((ushort)(resetVector + 1), indirectAddr);
            Ram.WriteWord((ushort)(0x0000 + (byte)(indirectAddr + xOffset)), dataAddr);
            Ram.WriteByte(dataAddr, data);

            Cpu.Reset();
            Cpu.X = xOffset;
        }


        public ulong CheckPageCross(ulong cycles, ushort startAddr, ushort endAddr)
        {
            byte hiStart;
            byte hiEnd;

            if (BitConverter.IsLittleEndian)
            {
                hiStart = (byte)(startAddr >> 8);
                hiEnd = (byte)(endAddr >> 8);
            }
            else
            {
                hiStart = (byte)(startAddr << 8);
                hiEnd = (byte)(endAddr << 8);
            }

            if (hiStart != hiEnd)
            {
                return cycles + 1;
            }
            else
            {
                return cycles;
            }
        }

        [TestCleanup()]
        public void Cleanup()
        {
            Cpu.Reset();
            Ram.Clear();
        }

    }
}