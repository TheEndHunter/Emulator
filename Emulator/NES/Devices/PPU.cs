using Emulator._6502;

using System.Runtime.InteropServices;

namespace Emulator.NES.Devices
{

    [StructLayout(LayoutKind.Sequential)]
    public record struct PPURegisters
    {
        public PPUCTRL controlFlags;
        public PPUMASK maskFlags;
        public PPUSTATUS statusFlags;
        public byte OAMAddr;
        public byte OAMData;
        public PPUScroll Scroll;
        public PPUAddr Address;
    }

    [StructLayout(LayoutKind.Sequential)]
    public record struct PPUScroll
    {
        public byte X;
        public byte Y;
    }

    [StructLayout(LayoutKind.Sequential)]
    public record struct PPUAddr
    {
        public byte Hi;
        public byte Lo;

        /*Conversion operator for converting to a Ushort*/

        public static implicit operator ushort(PPUAddr addr)
        {
            if (BitConverter.IsLittleEndian)
            {
                return (ushort)((addr.Hi << 8) | addr.Lo);
            }
            else
            {
                return (ushort)((addr.Lo << 8) | addr.Hi);
            }
        }
    }

    [Flags]
    public enum PPUCTRL : byte
    {
        NMI = 0x80,
        PRIMARY_MODE = 0x40,
        SPRITE_SIZE = 0x20,
        BG_TILE_SELECT = 0x10,
        SPRITE_TILE_SELECT = 0x08,
        ADDR_INCREMENT_MODE = 0x04,
        NAME_TABLE_SELECT_CTRL = NAME_TABLE_SELECT_UPPER | NAME_TABLE_SELECT_LOWER,
        NAME_TABLE_SELECT_UPPER = 0x02,
        NAME_TABLE_SELECT_LOWER = 0x01,
    }

    [Flags]
    public enum PPUMASK : byte
    {
        EMPH_BLUE = 0x80,
        EMPH_GREEN = 0x40,
        EMPH_RED = 0x20,
        SHOW_SPRITES = 0x10,

        SHOW_BACKGROUND = 0x08,
        SHOW_SPRITES_LEFT_MOST = 0x04,
        SHOW_BACKGROUND_LEFT_MOST = 0x02,
        GREYSCALE_MODE = 0x01,
    }

    [Flags]
    public enum PPUSTATUS : byte
    {
        VERTICAL_BLANKING = 0x80,
        SPRITE_HIT = 0x40,
        SPRITE_OVERFLOW = 0x20,

        PPU_STALE_0 = 0x10,
        PPU_STALE_1 = 0x08,
        PPU_STALE_2 = 0x04,
        PPU_STALE_3 = 0x02,
        PPU_STALE_4 = 0x01,
    }

    public class PPU : IDevice6502, IDisposable
    {
        public string DebugName => "PPU";
        public DeviceConfig DeviceFlags { get; } = DeviceConfig.ReadWrite;
        public PPU()
        {
        }


        private PPURegisters _registers;
        private byte _latch;
        private bool ReadScroll;
        private bool ReadAddr;
        private byte[] PPUMem = new byte[0x4000];
        private bool disposedValue;

        public byte ReadByte(ushort addr)
        {
            switch (addr)
            {
                case 0x2000:
                    ReadScroll = false;
                    ReadAddr = false;
                    return (byte)_registers.controlFlags;
                case 0x2001:
                    ReadScroll = false;
                    ReadAddr = false;
                    return (byte)_registers.maskFlags;
                case 0x2002:
                    ReadScroll = false;
                    ReadAddr = false;
                    // Reading PPUSTATUS also clears the vertical blank flag
                    byte d = (byte)_registers.statusFlags;
                    _registers.statusFlags &= ~(PPUSTATUS.VERTICAL_BLANKING);
                    return d;
                case 0x2003:
                    ReadScroll = false;
                    ReadAddr = false;
                    return _registers.OAMAddr;
                case 0x2004:
                    ReadScroll = false;
                    ReadAddr = false;
                    return _registers.OAMData;
                case 0x2005:
                    ReadAddr = false;
                    if (ReadScroll)
                    {
                        ReadScroll = false;
                        return _registers.Scroll.Y;
                    }
                    else
                    {
                        ReadScroll = false;
                        return _registers.Scroll.X;
                    }
                case 0x2006:
                    ReadScroll = false;
                    if (ReadAddr)
                    {
                        ReadAddr = false;
                        return _registers.Address.Hi;
                    }
                    else
                    {
                        ReadAddr = true;
                        return _registers.Address.Lo;
                    }
                case 0x2007:
                    // Reading PPUDATA returns the data from the previous read, and then updates the latch with the requested data
                    byte data = _latch;
                    _latch = PPUMem[(ushort)_registers.Address];
                    return data;
                case 0x4014:
                    byte mem = _latch;
                    return mem;
                default:
                    throw new Exception($"Invalid PPU read address: {addr}");
            }
        }

        public void WriteByte(ushort addr, byte data)
        {
            switch (addr)
            {
                case 0x2000:
                    _registers.controlFlags = (PPUCTRL)data;
                    break;
                case 0x2001:
                    _registers.maskFlags = (PPUMASK)data;
                    break;
                case 0x2002:
                    // Writing to PPUSTATUS has no effect
                    break;
                case 0x2003:
                    _registers.OAMAddr = data;
                    break;
                case 0x2004:
                    _registers.OAMData = data;
                    break;
                case 0x2005:
                    if (ReadScroll)
                    {
                        _registers.Scroll.Y = data;
                        ReadScroll = false;
                    }
                    else
                    {
                        _registers.Scroll.X = data;
                        ReadScroll = true;
                    }
                    break;
                case 0x2006:
                    if (ReadAddr)
                    {
                        _registers.Address.Hi = data;
                        ReadAddr = false;
                    }
                    else
                    {
                        _registers.Address.Lo = data;
                        ReadAddr = true;
                    }
                    break;
                case 0x2007:
                    PPUMem[(ushort)_registers.Address] = data;
                    break;
                case 0x4014:
                    // Handle OAMDMA
                    break;
                default:
                    throw new Exception($"Invalid PPU write address: {addr}");
            }
        }

        public bool CheckDeviceInRange(ushort a, DeviceConfig config)
        {
            return (((a & 0b0010_0000_0000_0000) > 0) || a == 0x4014 && DeviceFlags.CheckOverlap(config));
        }

        public string GetDebuggerDisplay()
        {
            return String.Empty;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                    Array.Clear(PPUMem);
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
