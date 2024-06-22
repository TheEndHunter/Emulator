namespace Emulator.N.E.S
{
    using Emulator._6502;

    using System.Diagnostics;

    /// <summary>
    /// NES PPU Control Register Bit Flags
    /// </summary>
    [Flags]
    public enum PPUCTRL : byte
    {
        None = 0,
        NMI = 0x80,
        PPUMasterSlave = 0x40,
        SpriteHeight = 0x20,
        BackgroundTileSelect = 0x08,
        SpriteTileSelect = 0x04,
        IncrementMode = 0x02,
        NameTableSelect = 0x01,
    }



    /// <summary>
    /// NES PPU Mask Register Bit Flags
    /// </summary>
    public enum PPUMask : byte
    {
        None = 0,
        ColorEmphasis = 0xE0,
        SpriteEnable = 0x10,
        BackgroundEnable = 0x08,
        SpriteLeftColumnEnable = 0x04,
        BackgroundLeftColumnEnable = 0x02,
        Greyscale = 0x01,
    }

    /// <summary>
    ///  NES PPU Status Register Bit Flags
    /// </summary>
    public enum PPUStatus : byte
    {
        None = 0,
        SpriteOverflow = 0x20,
        Sprite0Hit = 0x40,
        VerticalBlank = 0x80,
    }

    /// <summary>
    /// NES PPU Emulation Class,
    /// Stores PPU _registers and
    /// closely emulates the NES PPUs hardware behavior
    /// 
    /// MemoryAccess from the PPU to the CHR ROM and Memory is handled Externally through the IDevice6502 interface
    /// to allow for asynchronous access to the RAM/cartridge between the cpu and ppu
    /// </summary>
    [DebuggerDisplay($"{{{nameof(GetDebuggerDisplay)}(),nq}}")]
    public class PPU : IDevice6502
    {
        private readonly PPUCTRL _Control;
        private readonly PPUMask _Mask;
        private readonly PPUStatus _Status;
        private readonly byte _OAMAddress;
        private readonly byte _OAMData;
        private readonly byte _ScrollX;
        private readonly byte _ScrollY;
        public readonly bool _WriteScrollY;
        private readonly byte _PPUAddress;
        private readonly byte _PPUData;

        private byte _dataLatch;

        public string DebugName => "PPU";
        public DeviceConfig DeviceFlags => DeviceConfig.ReadWrite;

        /// <summary>
        /// Writes to the specified Address. Mirroring takes place inside this method to map to internal memory from the cpu's address space
        /// </summary>
        /// <param name="addr">Address to write to</param>
        /// <param name="data">data being written</param>
        public void WriteByte(ushort addr, byte data)
        {

        }
        /// <summary>
        /// Reads the specified Address
        /// </summary>
        /// <param name="addr">Address to read from memory, mirroring takes place inside this method to map to internal memory from the cpu's address space</param>
        /// <returns></returns>
        public byte ReadByte(ushort addr)
        {
            switch (MapAddr(addr))
            {
                case 0x2000:
                    _dataLatch = (byte)_Control;
                    break;
                case 0x2001:
                    _dataLatch = (byte)_Mask;
                    break;
                case 0x2002:
                    _dataLatch = (byte)_Status;
                    break;
                case 0x2003:
                    _dataLatch = _OAMAddress;
                    break;
                case 0x2004:
                    _dataLatch = _OAMData;
                    break;

                case 0x2005:
                    _dataLatch = _ScrollX;
                    break;
                case 0x2007:
                    _dataLatch = _PPUData;
                    break;
                default:
                    break;
            }
            return _dataLatch;
        }

        /// <summary>
        /// Checks if the address is within the Range of the PPU's Register Addresses, automatically Converts to Correct address for the register if it is
        /// </summary>
        /// <param name="addr">The address input</param>
        /// <returns>The mapped address of the register</returns>
        public static ushort MapAddr(ushort addr) => (ushort)(addr & 0x2007);

        public string GetDebuggerDisplay()
        {
            throw new NotImplementedException();
        }

        public bool CheckDeviceInRange(ushort a, DeviceConfig config)
        {
            if (a >= 0x2000 && a <= 0x3FFF)
            {
                return true;
            }
            return false;
        }
    }

    public enum PPUAddressMask
    {
        a = 0b10000000000000,
        b = 0b10000000000111,
        c = 0b11111111111000,
        d = 0b11111111111111,

    }
}
