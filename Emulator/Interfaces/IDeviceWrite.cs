namespace Emulator.CPUs
{
    /// <summary>to
    /// Interface that defines interactions with items on the address/data bus that can be written to
    /// </summary>
    /// <typeparam name="BusWidth">value type used to represent address widths</typeparam>
    public interface IDeviceWrite<BusWidth> : IDevice<BusWidth> where BusWidth : unmanaged
    {
        /// <summary>
        /// Write byte to the memory address
        /// </summary>
        /// <param name="address">address to Write to</param>
        /// <returns>data to memory address</returns>
        public void Write(BusWidth address);

        /// <summary>
        /// Write byte to the memory address
        /// </summary>
        /// <param name="address">address to Write to</param>
        /// <returns>data to memory address</returns>
        public byte this[BusWidth address]
        {
            set;
        }

    }
}
