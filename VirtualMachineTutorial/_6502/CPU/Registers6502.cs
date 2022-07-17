namespace Emulator._6502.CPU
{
    public struct Registers6502
    {
        public byte A { get; set; }
        public byte X { get; set; }
        public byte Y { get; set; }
        public byte STKP { get; set; }
        public ushort PC { get; set; }
        public Status6502 Status { get; set; }
    }

}
