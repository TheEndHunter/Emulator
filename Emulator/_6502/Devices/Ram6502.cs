namespace Emulator._6502.Devices
{
    public class Ram6502 : Device6502
    {
        private readonly bool convertEdian;
        public Ram6502() : base("RAM")
        {
            convertEdian = !BitConverter.IsLittleEndian;
            memory = new byte[0xFFFF + 1];
        }

        private readonly byte[] memory;

        public void LoadData(byte[] bytes)
        {
            bytes.CopyTo(memory, 0);
        }

        public void LoadData(ushort addr, byte[] bytes)
        {
            bytes.CopyTo(memory, addr);
        }
        public void LoadData(byte addr, byte[] bytes)
        {
            bytes.CopyTo(memory, BitConverter.IsLittleEndian ? BitConverter.ToInt16(new[] { (byte)0x00, addr }, 0) : BitConverter.ToInt16(new[] { addr, (byte)0x00 }, 0));
        }

        public override byte ReadByte(ushort addr)
        {
            return memory[addr];
        }

        public override ushort ReadWord(ushort addr)
        {
            var byte1 = ReadByte(addr);
            var byte2 = ReadByte((ushort)(addr + 1));

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
                Write((ushort)(addr + 1), bytes[0]);
            }
            else
            {
                Write(addr, bytes[0]);
                Write((ushort)(addr + 1), bytes[1]);
            }
        }
    }
}
