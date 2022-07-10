namespace VirtualMachineTutorial
{
    [Flags]
    public enum AddrMode6502 : ushort
    {
        None = 0,
        Accumulator = 1,
        Immediate = 2,
        Implied = 4,
        Relative = 8,
        Absolute = 16,
        ZeroPage = 32,
        Indirect = 64,
        AbsoluteIndexed = 128,
        ZeroPageIndexed = 256,
        IndexedIndirect = 1024,
        IndirectIndexed = 2048,
        ZeroPageX = 4096,
        ZeroPageY = 8192,
        AbsoluteX = 16384,
        AbsoluteY = 32768,
    }

}
