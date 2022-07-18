namespace Emulator._6502.Devices
{
    public class Bus6502
    {
        public Bus6502()
        {
            MemoryMap = new Dictionary<AddressRange6502, Device6502>();
        }

        private readonly Dictionary<AddressRange6502, Device6502> MemoryMap;

        private KeyValuePair<AddressRange6502, Device6502>? CheckDeviceOverlap(AddressRange6502 range)
        {
            foreach (var device in MemoryMap)
            {
                if (range.StartAddress >= device.Key.StartAddress && range.StartAddress <= device.Key.EndAddress)
                {
                    return device;
                }
                if (range.EndAddress >= device.Key.StartAddress && range.EndAddress <= device.Key.EndAddress)
                {
                    return device;
                }
            }
            return null;
        }

        public void RegisterDevice(AddressRange6502 addressRange, Device6502 device)
        {
            var res = CheckDeviceOverlap(addressRange);
            if (res != null)
            {
                if (res!.Value.Value == device)
                {

                    throw new ArgumentOutOfRangeException(nameof(addressRange), $"Given device is already registered");
                }
                else
                {
                    throw new ArgumentOutOfRangeException(nameof(addressRange), $"Given address range overlaps with a device already registered");
                }
            }
            else
            {
                MemoryMap.Add(addressRange, device);
            }
        }

        public void RegisterDevice(ushort startAddr, ushort endAddr, Device6502 device)
        {
            RegisterDevice(new AddressRange6502() { StartAddress = startAddr, EndAddress = endAddr }, device);
        }

        public void UnRegisterDevice(Device6502 device)
        {
            if (MemoryMap.ContainsValue(device))
            {
                var res = MemoryMap.Where(x => x.Value == device);
                foreach (var dev in res)
                {
                    MemoryMap.Remove(dev.Key);
                }
            }
        }

        public void UnRegisterDevice(AddressRange6502 addr)
        {
            if (MemoryMap.ContainsKey(addr))
            {
                MemoryMap.Remove(addr);
            }
        }

        public void Write(ushort addr, byte data)
        {
            foreach (var dev in MemoryMap)
            {
                if (dev.Key.StartAddress <= addr && dev.Key.EndAddress >= addr)
                {
                    dev.Value.Write(addr, data);
                }
            }
        }
        public void Write(ushort addr, ushort data)
        {
            foreach (var dev in MemoryMap)
            {
                if (dev.Key.StartAddress <= addr && dev.Key.EndAddress >= addr)
                {
                    dev.Value.Write(addr, data);
                }
            }
        }
        public byte ReadByte(ushort addr)
        {
            foreach (var dev in MemoryMap)
            {
                if (dev.Key.StartAddress <= addr && dev.Key.EndAddress >= addr)
                {
                    return dev.Value.ReadByte(addr);
                }
            }
            return 0;
        }
        public ushort ReadWord(ushort addr)
        {
            foreach (var dev in MemoryMap)
            {
                if (dev.Key.StartAddress <= addr && dev.Key.EndAddress >= addr)
                {
                    return dev.Value.ReadWord(addr);
                }
            }
            return 0;
        }
    }
}
