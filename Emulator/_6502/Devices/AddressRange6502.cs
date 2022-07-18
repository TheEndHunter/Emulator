using System.Diagnostics;

namespace Emulator._6502.Devices
{
    [DebuggerDisplay($"{{{nameof(GetDebuggerDisplay)}(),nq}}")]
    public struct AddressRange6502
    {
        public ushort StartAddress { get; init; }
        public ushort EndAddress { get; init; }

        private string GetDebuggerDisplay()
        {
            return $"0x{StartAddress.ToString("X4")} - 0x{EndAddress.ToString("X4")}";
        }
    }
}
