namespace Emulator.CPUs
{
    [Flags]
    public enum BusMode
    {
        Read = 1,
        Write = 2,
        ReadWrite = Read | Write
    }

    public class DeviceMap<BusWidth> where BusWidth : unmanaged
    {
        private DeviceMap()
        {
        }
        public DeviceMap(BusWidth start, BusWidth end, IDeviceRead<BusWidth> device) : this(start, end, BusMode.Read, device, null, null)
        {

        }
        public DeviceMap(BusWidth start, BusWidth end, IDeviceWrite<BusWidth> device) : this(start, end, BusMode.Write, null, device, null)
        {

        }
        public DeviceMap(BusWidth start, BusWidth end, IDeviceReadWrite<BusWidth> device) : this(start, end, BusMode.ReadWrite, null, null, device)
        {

        }
        private DeviceMap(BusWidth start, BusWidth end, BusMode mode, IDeviceRead<BusWidth>? read = null, IDeviceWrite<BusWidth>? write = null, IDeviceReadWrite<BusWidth>? readWrite = null)
        {
            Start = start;
            End = end;
            Mode = mode;
            Read = read;
            Write = write;
            ReadWrite = readWrite;
        }

        public BusWidth Start { get; init; } = default;
        public BusWidth End { get; init; } = default;
        public BusMode Mode { get; init; } = BusMode.Read;

        public IDeviceRead<BusWidth>? Read { get; init; } = null;
        public IDeviceWrite<BusWidth>? Write { get; init; } = null;
        public IDeviceReadWrite<BusWidth>? ReadWrite { get; init; } = null;
    }

    /// <summary>
    /// Interface that defines an interface to connect peripherals/devices to a CPU's address/data bus
    /// </summary>
    /// <typeparam name="BusWidth">value type used to represent address widths</typeparam>
    public interface IDevice<BusWidth> where BusWidth : unmanaged
    {
        public ByteOrder ByteOrder { get; init; }
    }
}
