using System.Buffers.Binary;
using System.Diagnostics;
using System.Text;

namespace Emulator._6502
{
    [DebuggerDisplay($"{{{nameof(GetDebuggerDisplay)}(),nq}}")]
    public class Ram6502 : IDevice6502
    {
        private readonly bool convertEndian;

        public string GetDebuggerDisplay()
        {
            StringBuilder sb = new();

            sb.Append($"RAM: {(memory.Length / 1000) / 1000}KB");

            return sb.ToString();
        }
        public Ram6502()
        {
            DebugName = "RAM";
            convertEndian = !BitConverter.IsLittleEndian;
            memory = (byte[])Array.CreateInstance(typeof(byte), ushort.MaxValue + 1);
        }

        private readonly Memory<byte> memory;

        public string DebugName { get; }

        public DeviceConfig DeviceFlags { get; } = DeviceConfig.ReadWrite | DeviceConfig.Debug;

        public ushort LoadData(string filePath)
        {
            return (ushort)File.OpenRead(filePath).Read(memory.Span);
        }

        public void LoadData(byte[] bytes)
        {
            bytes.AsMemory().CopyTo(memory);
        }

        public void LoadData(ushort addr, byte[] bytes)
        {
            if (bytes.Length < 1) return;
            if (bytes.Length > memory.Length) throw new InsufficientMemoryException($"Not enough space to load data into memory: {bytes.Length:X} > {memory.Length:X} ");
            if (addr + bytes.Length > memory.Length) throw new InsufficientMemoryException($"Not enough space to load data into memory from Address:{addr:X} TO Address{addr + bytes.Length:X} > {memory.Length:X} ");

            bytes.CopyTo(memory);
            bytes.CopyTo(memory.Slice(addr, bytes.Length));
        }

        private readonly ParallelOptions parallelOptions = new()
        {
            TaskScheduler = null,
            MaxDegreeOfParallelism = Environment.ProcessorCount,
            CancellationToken = CancellationToken.None,
        };

        public void Clear()
        {
            memory.Span.Clear();
        }

        public byte ReadByte(ushort addr)
        {
            byte b = memory.Span[addr];
            return b;
        }

        public void WriteByte(ushort addr, byte data)
        {
            memory.Span[addr] = data;
        }

        public ushort ReadWord(ushort address)
        {
            return BinaryPrimitives.ReadUInt16LittleEndian(memory.Span.Slice(address, 2));
        }

        public void WriteWord(ushort address, ushort data)
        {
            BinaryPrimitives.WriteUInt16LittleEndian(memory.Span.Slice(address, 2), data);
        }

        public bool CheckDeviceInRange(ushort a, DeviceConfig config)
        {
            return true;
        }
    }

    public static class EnumExtension
    {
        public static bool CheckOverlap(this DeviceConfig value, DeviceConfig flags)
        {
            if (value.HasFlag(DeviceConfig.ReadOnly) && flags.HasFlag(DeviceConfig.ReadOnly)) return true;
            if (value.HasFlag(DeviceConfig.WriteOnly) && flags.HasFlag(DeviceConfig.WriteOnly)) return true;

            return false;
        }
    }
}
