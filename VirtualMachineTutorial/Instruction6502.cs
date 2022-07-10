namespace VirtualMachineTutorial
{
    public struct Instruction6502
    {
        public string Name { get; init; }
        public AddrMode6502 AddressMode { get; init; }
        public Status6502 Flags { get; init; }
        public byte Execute(ref Registers6502 registers, ref Bus6502 memory)
        {
            return Func(registers, AddressMode, memory);
        }
        public Func<Registers6502, AddrMode6502, Bus6502, byte> Func { init; private get; }
    }

}
