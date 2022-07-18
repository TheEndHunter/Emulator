using System.Diagnostics;

namespace Emulator._6502.Devices
{
    [DebuggerDisplay($"{{{nameof(GetDebuggerDisplay)}(),nq}}")]
    public abstract class Device6502
    {
        protected Device6502(string? name = null)
        {
            DebugName = name ?? GetType().Name;
        }
        protected string DebugName { get; init; }
        public abstract byte ReadByte(ushort addr);

        public abstract ushort ReadWord(ushort addr);

        public abstract void Write(ushort addr, byte data);

        public abstract void Write(ushort addr, ushort data);

        public string GetDebuggerDisplay()
        {
            return $"{DebugName}";
        }
    }
}
