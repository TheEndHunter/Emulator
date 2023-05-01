using Emulator._6502;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using System.Net;
using System.Reflection.Emit;

using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Emulator.Tests.Instructions
{
    public abstract class CpuInstructionTests : InstructionTests6502
    {
        public Status6502 SetZN(Status6502 status, byte data)
        {
            return SetNegative(SetZero(status, data), data);
        }

        public ulong CheckIndirectIndexCycles(ulong cycles, byte startAddr, byte y, ushort address)
        {
            return CheckPageCross(cycles, startAddr, (ushort)(address + y));
        }

        public ulong CyclesCheck(ulong cycles, byte offsetAddr, ushort endAddr)
        {
            return CheckPageCross(cycles, endAddr, (ushort)(endAddr + offsetAddr));
        }
    }
}