using Emulator._6502;

using System.Runtime.InteropServices;

namespace Emulator.NES.Devices
{

    [StructLayout(LayoutKind.Sequential)]
    public struct PPURegisters
    {
        public PPUCTRL controlFlags;
        public PPUMASK maskFlags;
        public PPUSTATUS statusFlags;
        PPUScroll Scroll;
        PPUAddr Address;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct PPUScroll
    {
        public byte X;
        public byte Y;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct PPUAddr
    {
        public byte MS;
        public byte LS;
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

    public class PPU : IDevice6502
    {
        public string DebugName => "PPU";
        public DeviceConfig DeviceFlags { get; } = DeviceConfig.ReadWrite;
        public PPU()
        {
        }

        private PPURegisters Registers;
        private readonly byte _latch;
        public byte ReadByte(ushort addr)
        {
            if (addr == 0x4014)
            {
                return _latch;
            }

            return 0;
        }

        public static ushort ReadWord(ushort addr) => 0x0000;

        public void WriteByte(ushort addr, byte data)
        {
        }

        public void WriteWord(ushort addr, ushort data)
        {
        }

        public bool CheckDeviceInRange(ushort a, DeviceConfig config)
        {
            return (((a & 0b0010_0000_0000_0000) > 0) || a == 0x4014 && DeviceFlags.CheckOverlap(config));
        }

        public string GetDebuggerDisplay()
        {
            throw new NotImplementedException();
        }
    }
}
