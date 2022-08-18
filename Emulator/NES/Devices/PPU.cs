using Emulator._6502.Devices;

namespace Emulator.NES.Devices
{
    internal class PPU_PatternMem : Device6502
    {
        internal static readonly AddressRange6502 AddressingRange = new() { StartAddress = 0x00, EndAddress = 0x1FFF };
        public PPU_PatternMem()
        {

        }

        public override byte ReadByte(ushort addr)
        {
            throw new NotImplementedException();
        }

        public override ushort ReadWord(ushort addr)
        {
            throw new NotImplementedException();
        }

        public override void Write(ushort addr, byte data)
        {
            throw new NotImplementedException();
        }

        public override void Write(ushort addr, ushort data)
        {
            throw new NotImplementedException();
        }
    }

    public class PPU : Device6502
    {
        public PPU()
        {

        }

        public override byte ReadByte(ushort addr)
        {
            throw new NotImplementedException();
        }

        public override ushort ReadWord(ushort addr)
        {
            throw new NotImplementedException();
        }

        public override void Write(ushort addr, byte data)
        {
            throw new NotImplementedException();
        }

        public override void Write(ushort addr, ushort data)
        {
            throw new NotImplementedException();
        }
    }
}
