namespace Emulator.CPUs
{
    /// <summary>
    /// Interface that defines interactions with items on the address/data bus that can read from and be written to
    /// </summary>
    /// <typeparam name="BusWidth">value type used to represent address widths</typeparam>
    public interface IDeviceReadWrite<BusWidth> : IDeviceRead<BusWidth>, IDeviceWrite<BusWidth> where BusWidth : unmanaged
    {
        public new byte this[BusWidth address]
        {
            get;
            set;
        }
    }
}
