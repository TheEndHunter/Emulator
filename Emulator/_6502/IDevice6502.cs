namespace Emulator._6502
{
    [Flags]
    public enum DeviceConfig
    {
        Invalid = 0,
        ReadOnly = 1,
        WriteOnly = 2,
        ReadWrite = ReadOnly | WriteOnly,
        Debug = 4,
        All = ReadWrite | Debug,
    }
    public interface IDevice6502
    {
        public string DebugName { get; }
        public abstract string GetDebuggerDisplay();

        public string GetDebugDeviceName(ushort address, DeviceConfig config)
        {
            if (CheckDeviceInRange(address, config))
            {
                return GetDebuggerDisplay();
            }
            return string.Empty;
        }

        public DeviceConfig DeviceFlags { get; }

        public abstract byte ReadByte(ushort Address);
        public abstract void WriteByte(ushort Address, byte data);
        public bool CheckDeviceInRange(ushort a, DeviceConfig config);

        public byte this[ushort address]
        {
            get
            {
                return ReadByte(address);
            }
            set
            {
                WriteByte(address, value);
            }
        }
    }
}
