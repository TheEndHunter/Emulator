namespace VirtualMachineTutorial
{
    public abstract class Device6502
    {
        public abstract byte ReadByte(ushort addr);

        public abstract ushort ReadWord(ushort addr);

        public abstract void Write(ushort addr, byte data);

        public abstract void Write(ushort addr, ushort data);
    }
}
