namespace Emulator._6502.CPU
{
    /// <summary>
    /// Addressing modes for 6502
    /// </summary>
    [Flags]
    public enum AddrMode6502 : ushort
    {
        None = 0,
        Accumulator = 1,
        Absolute = 2,
        AbsoluteX = 4,
        AbsoluteY = 8,
        Immediate = 16,
        Implied = 32,
        Indirect = 64,
        IndexedIndirect = 128,
        IndirectIndexed = 256,
        Relative = 1024,
        ZeroPage = 2048,
        ZeroPageX = 4096,
        ZeroPageY = 8192,
    }

}
