namespace Emulator.CPUs
{
    public sealed class SystemBusBE<BusWidth> : SystemBus<BusWidth> where BusWidth : unmanaged, IComparable<BusWidth>, IEquatable<BusWidth>, IComparer<BusWidth>, IEqualityComparer<BusWidth>, IFormattable
    {
        public SystemBusBE() : base(ByteOrder.Big)
        {

        }

        public override uint ReadDWord(BusWidth address)
        {
            throw new NotImplementedException();
        }

        public override ulong ReadQWord(BusWidth address)
        {
            throw new NotImplementedException();
        }

        public override sbyte ReadSByte(BusWidth address)
        {
            throw new NotImplementedException();
        }

        public override int ReadSDWord(BusWidth address)
        {
            throw new NotImplementedException();
        }

        public override long ReadSQWord(BusWidth address)
        {
            throw new NotImplementedException();
        }

        public override short ReadSWord(BusWidth address)
        {
            throw new NotImplementedException();
        }

        public override ushort ReadWord(BusWidth address)
        {
            throw new NotImplementedException();
        }

        public override void Write(BusWidth address, sbyte data)
        {
            throw new NotImplementedException();
        }

        public override void Write(BusWidth address, short data)
        {
            throw new NotImplementedException();
        }

        public override void Write(BusWidth address, int data)
        {
            throw new NotImplementedException();
        }

        public override void Write(BusWidth address, long data)
        {
            throw new NotImplementedException();
        }

        public override void Write(BusWidth address, byte data)
        {
            throw new NotImplementedException();
        }

        public override void Write(BusWidth address, ushort data)
        {
            throw new NotImplementedException();
        }

        public override void Write(BusWidth address, uint data)
        {
            throw new NotImplementedException();
        }

        public override void Write(BusWidth address, ulong data)
        {
            throw new NotImplementedException();
        }
    }
}
