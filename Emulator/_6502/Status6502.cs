namespace Emulator._6502
{
    [Flags]
    public enum Status6502 : byte
    {
        None = 0,
        Carry = 1,
        Zero = 2,
        InterruptDisable = 4,
        Decimal = 8,
        Break = 16,
        Unused = 32,
        OverFlow = 64,
        Negative = 128,
    }

}
