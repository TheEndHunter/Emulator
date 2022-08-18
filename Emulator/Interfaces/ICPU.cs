namespace Emulator.CPUs
{

    public abstract class SystemBus<BusWidth> where BusWidth : unmanaged, IComparable<BusWidth>, IEquatable<BusWidth>, IComparer<BusWidth>, IEqualityComparer<BusWidth>, IFormattable
    {
        protected SystemBus(ByteOrder byteOrder)
        {
            mDeviceMaps = new();
            mByteOrder = byteOrder;

            switch (byteOrder)
            {
                case ByteOrder.Little:
                    mConvertByteOrder = !BitConverter.IsLittleEndian;
                    break;
                case ByteOrder.Big:
                    mConvertByteOrder = BitConverter.IsLittleEndian;
                    break;
            }
        }

        protected List<DeviceMap<BusWidth>> mDeviceMaps;
        protected readonly ByteOrder mByteOrder;
        protected readonly bool mConvertByteOrder;

        public byte ReadByte(BusWidth address)
        {
            var r = mDeviceMaps.Where(x =>
            {
                return x.Start.CompareTo(address) >= 0 || x.End.CompareTo(address) <= 0 && x.Mode.HasFlag(BusMode.Read);
            });
            if (r.Any())
            {
                var a = r.ElementAt(0);

                if (a.Mode == BusMode.Read)
                    return a.Read![address];

                if (a.Mode == BusMode.ReadWrite)
                    return a.ReadWrite![address];
            }
            return 0;
        }
        public abstract uint ReadDWord(BusWidth address);
        public abstract ulong ReadQWord(BusWidth address);
        public abstract sbyte ReadSByte(BusWidth address);
        public abstract int ReadSDWord(BusWidth address);
        public abstract long ReadSQWord(BusWidth address);
        public abstract short ReadSWord(BusWidth address);
        public abstract ushort ReadWord(BusWidth address);
        public abstract void Write(BusWidth address, sbyte data);
        public abstract void Write(BusWidth address, short data);
        public abstract void Write(BusWidth address, int data);
        public abstract void Write(BusWidth address, long data);
        public abstract void Write(BusWidth address, byte data);
        public abstract void Write(BusWidth address, ushort data);
        public abstract void Write(BusWidth address, uint data);
        public abstract void Write(BusWidth address, ulong data);

        private int CheckDeviceOverlap(BusWidth start, BusWidth end, BusMode mode)
        {
            for (int i = 0; i < mDeviceMaps.Count; i++)
            {
                var device = mDeviceMaps[i];
                if (device.Start.CompareTo(start) < 0 && device.End.CompareTo(end) < 0 && device.Mode.HasFlag(mode))
                {
                    return i;
                }
            }
            return -1;
        }

        public int RegisterDevice(BusWidth StartAddr, BusWidth EndAddr, IDeviceRead<BusWidth> device)
        {
            var r = CheckDeviceOverlap(StartAddr, EndAddr, BusMode.Read);
            if (r == -1)
            {
                mDeviceMaps.Add(new DeviceMap<BusWidth>(StartAddr, EndAddr, device));
                return mDeviceMaps.Count - 1;
            }
            else
            {
                return r;
            }
        }
        public int RegisterDevice(BusWidth StartAddr, BusWidth EndAddr, IDeviceWrite<BusWidth> device)
        {
            var r = CheckDeviceOverlap(StartAddr, EndAddr, BusMode.Write);
            if (r == -1)
            {
                mDeviceMaps.Add(new DeviceMap<BusWidth>(StartAddr, EndAddr, device));
                return mDeviceMaps.Count - 1;
            }
            else
            {
                return r;
            }
        }

        public int RegisterDevice(BusWidth StartAddr, BusWidth EndAddr, IDeviceReadWrite<BusWidth> device)
        {
            var r = CheckDeviceOverlap(StartAddr, EndAddr, BusMode.ReadWrite);
            if (r == -1)
            {
                mDeviceMaps.Add(new DeviceMap<BusWidth>(StartAddr, EndAddr, device));
                return mDeviceMaps.Count - 1;
            }
            else
            {
                return r;
            }
        }

        public void UnregisterDevice(int deviceID)
        {
            if (deviceID > mDeviceMaps.Count - 1 || deviceID < 0)
                return;

            mDeviceMaps.RemoveAt(deviceID);
        }
    }
    /// <summary>
    /// Interface that defines basic interactions for a CPU
    /// </summary>
    public interface ICPU
    {
        /// <summary>
        /// Preforms a single clock cycle
        /// </summary>
        public void Clock();
        /// <summary>
        /// this steps through execution on instruction at a time, not just one clock.
        /// </summary>
        /// <param name="steps">number of steps to take</param>
        /// <returns>number of clocks/cycles taken to preform steps, useful for debugging</returns>
        public ulong Step(ulong steps);
        /// <summary>
        /// interrupts the CPU's execution(if the CPU has interrupts enabled and the CPU supports interrupts)
        /// </summary>
        public void Interupt();
        /// <summary>
        /// Resets the CPU to a known startup state.
        /// </summary>
        public void Reset();
        /// <summary>
        /// Interrupt that can not be masked by disabling interrupts(if the CPU has interrupts and supports non-mask-able interrupts)
        /// </summary>
        public void NonMaskableInterupt();
    }
}
