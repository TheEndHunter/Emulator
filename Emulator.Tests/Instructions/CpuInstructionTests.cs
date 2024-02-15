using Emulator._6502;

namespace Emulator.Tests.Instructions
{
    public abstract class CpuInstructionTests : InstructionTests6502
    {
        public static Status6502 SetZN(Status6502 status, byte data) => SetNegative(SetZero(status, data), data);

        public static ulong CheckIndirectIndexCycles(ulong cycles, byte startAddr, byte y, ushort address) => CheckPageCross(cycles, startAddr, (ushort)(address + y));

        public static ulong CyclesCheck(ulong cycles, byte offsetAddr, ushort endAddr) => CheckPageCross(cycles, endAddr, (ushort)(endAddr + offsetAddr));
    }
}