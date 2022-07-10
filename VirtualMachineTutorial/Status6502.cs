namespace VirtualMachineTutorial
{
    [Flags]
    public enum Status6502 : byte
    {
        None = 0,
        Carry = 1,
        Zero = 2,
        IRQDisable = 4,
        DecimalMode = 8,
        BRK = 16,
        OVRFLW = 32,
        Negative = 64,
        Unused = 128
    }

}
