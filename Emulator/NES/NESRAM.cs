namespace Emulator.NES
{
    using System.Numerics;
    using System.Runtime.CompilerServices;

    public interface IReadOnly<in AddrWidth> where AddrWidth : IUnsignedNumber<AddrWidth>
    {
        public byte ReadByte(AddrWidth addr);
        public ushort ReadWord(AddrWidth addr);
        public byte this[AddrWidth addr] => ReadByte(addr);
    }

    public interface IWriteOnly<in AddrWidth> where AddrWidth : IUnsignedNumber<AddrWidth>
    {
        public void WriteByte(AddrWidth addr, byte data);

        public void WriteWord(AddrWidth addr, ushort data);

        public byte this[AddrWidth addr] { set { WriteByte(addr, value); } }
    }

    public interface IReadWrite<in AddrWidth> : IReadOnly<AddrWidth>, IWriteOnly<AddrWidth> where AddrWidth : IUnsignedNumber<AddrWidth>
    {
    }


    [InlineArray(2048)]
    public struct NESRAM : IReadWrite<ushort>
    {
        private byte _element0;

        public readonly byte ReadByte(ushort addr) => this[addr];

        public readonly ushort ReadWord(ushort addr)
        {
            var bytes = new byte[2];
            if (BitConverter.IsLittleEndian)
            {
                bytes[0] = this[addr];
                bytes[1] = this[(ushort)(addr + 1)];
            }
            else
            {
                bytes[1] = this[addr];
                bytes[0] = this[(ushort)(addr + 1)];
            }
            return BitConverter.ToUInt16(bytes);
        }

        public void WriteByte(ushort addr, byte data)
        {
            this[addr] = data;
        }

        public void WriteWord(ushort addr, ushort data)
        {
            var bytes = BitConverter.GetBytes(data);
            if (BitConverter.IsLittleEndian)
            {
                this[addr] = bytes[0];
                this[(ushort)(addr + 1)] = bytes[1];
            }
            else
            {
                this[addr] = bytes[1];
                this[(ushort)(addr + 1)] = bytes[0];
            }
        }
    }
}
