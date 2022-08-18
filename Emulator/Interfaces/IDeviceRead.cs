namespace Emulator.CPUs
{
    /// <summary>
    /// Interface that defines interactions with items on the address/data bus that can be read from
    /// </summary>
    /// <typeparam name="BusWidth">value type used to represent address widths</typeparam>
    public interface IDeviceRead<BusWidth> : IDevice<BusWidth> where BusWidth : unmanaged
    {
        /// <summary>
        /// Read byte from the memory address
        /// </summary>
        /// <param name="address">address to read from</param>
        /// <returns>data from memory address</returns>
        public byte Read(BusWidth address);

        /// <summary>
        /// Read byte from the memory address
        /// </summary>
        /// <param name="address">address to read from</param>
        /// <returns>data from memory address</returns>
        public byte this[BusWidth address]
        {
            get;
        }

    }
}
