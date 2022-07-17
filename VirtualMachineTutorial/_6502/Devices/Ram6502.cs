namespace Emulator._6502.Devices
{
    public class Ram6502 : Device6502
    {
        private readonly bool convertEdian;
        public Ram6502()
        {
            convertEdian = !BitConverter.IsLittleEndian;
            memory = new byte[0xFFFF];
        }

        private readonly byte[] memory;

        public override byte ReadByte(ushort addr)
        {
            return memory[addr];
        }

        public override ushort ReadWord(ushort addr)
        {
            var byte1 = ReadByte(addr);
            ushort addr2 = (ushort)(addr + 1);
            var byte2 = ReadByte(addr2);

            if (convertEdian)
            {
                return BitConverter.ToUInt16(new[] { byte2, byte1 }, 0);
            }
            else
            {
                return BitConverter.ToUInt16(new[] { byte1, byte2 }, 0);
            }
        }

        public override void Write(ushort addr, byte data)
        {
            memory[addr] = data;
        }

        public override void Write(ushort addr, ushort data)
        {
            var bytes = BitConverter.GetBytes(data);
            if (convertEdian)
            {
                Write(addr, bytes[1]);
                Write((ushort)(addr + 1u), bytes[0]);
            }
            else
            {
                Write(addr, bytes[0]);
                Write((ushort)(addr + 1u), bytes[1]);
            }
        }
    }
}
