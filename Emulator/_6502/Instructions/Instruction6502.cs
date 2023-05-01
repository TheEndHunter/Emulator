

using System.Buffers.Binary;
using System.Diagnostics;
using System.Net;
using System.Reflection.Emit;
using System.Text;

using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Emulator._6502.Instructions
{
    /// <summary>
    /// 6502 instruction
    /// </summary>
    [DebuggerDisplay($"{{{nameof(GetDebuggerDisplay)}(),nq}}")]
    public abstract class Instruction6502
    {
        /// <summary>
        /// Default Constructor
        /// </summary>
        /// <param name="name"></param>
        /// <param name="addressMode"></param>
        /// <param name="flags"></param>
        protected Instruction6502(string name, byte bytesUsed, AddrMode6502 addressMode, Status6502 flags)
        {
            Name = name;
            AddressMode = addressMode;
            Flags = flags;
            BytesUsed = bytesUsed;
        }
        public string Name { get; }
        public AddrMode6502 AddressMode { get; }
        public Status6502 Flags { get; }
        public byte BytesUsed { get; }
        public abstract byte Execute(Cpu6502 cpu);
        public string GetDebuggerDisplay()
        {
            return $"{Name}[A:{AddressMode}][F:{Flags}]";
        }

        public void Disassemble(StringBuilder sb, Cpu6502 cpu, ref ushort addr, bool showFlags = false)
        {
            byte datab = 0;
            ushort dataus = 0;

            sb.AppendFormat("0x{0}({1}): {2} ", addr.ToString("X4"), addr, Name);

            addr++;

            if (BytesUsed == 2)
            {
                datab = cpu.ReadByte(addr);
                addr++;
            }
            else if (BytesUsed == 3)
            {
                dataus = cpu.ReadWord(addr);
                addr += 2;
            }
            else if (BytesUsed > 3)
            {
                throw new InvalidCastException("The specified number of bytes used by this instruction is invalid");
            }

            sb.Append(AddressMode switch
            {
                AddrMode6502.Immediate => $"#{datab:X2}",
                AddrMode6502.Indirect => $"(${dataus:X4})",
                AddrMode6502.IndexedIndirect => $"(${dataus:X4},X)",
                AddrMode6502.IndirectIndexed => $"(${dataus:X4}),Y",
                AddrMode6502.ZeroPage => $"${datab:X2}",
                AddrMode6502.ZeroPageX => $"${datab:X2},X",
                AddrMode6502.ZeroPageY => $"${datab:X2},Y",
                AddrMode6502.Relative => $"${datab:X2}",
                AddrMode6502.Accumulator => "A",
                AddrMode6502.Absolute => $"${dataus:X4}",
                AddrMode6502.AbsoluteX => $"${dataus:X4},X",
                AddrMode6502.AbsoluteY => $"${dataus:X4},Y",
                _ => string.Empty,
            });

            if (showFlags && Flags != 0)
            {
                sb.AppendFormat(" [Flags]{0}", Flags);
            }
        }

        /// <summary>
        /// hi byte for some addressing modes
        /// </summary>
        private const byte hi = 0;
        protected static ushort ZeroPage(ref Cpu6502 cpu)
        {
            var lo = cpu.ReadByte(cpu.PC++);

            if (BitConverter.IsLittleEndian)
            {
                return BinaryPrimitives.ReadUInt16LittleEndian(new[] { lo, hi });
            }
            else
            {
                return BinaryPrimitives.ReadUInt16BigEndian(new[] { lo, hi });
            }
        }
        protected static ushort ZeroPageX(ref Cpu6502 cpu)
        {
            byte lo = (byte)(cpu.ReadByte(cpu.PC++) + cpu.X);
            if (BitConverter.IsLittleEndian)
            {
                return BinaryPrimitives.ReadUInt16LittleEndian(new[] { lo, hi });
            }
            else
            {
                return BinaryPrimitives.ReadUInt16BigEndian(new[] { lo, hi });
            }
        }
        protected static ushort ZeroPageY(ref Cpu6502 cpu)
        {

            byte lo = (byte)(cpu.ReadByte(cpu.PC++) + cpu.Y);
            if (BitConverter.IsLittleEndian)
            {
                return BinaryPrimitives.ReadUInt16LittleEndian(new[] { lo, hi });
            }
            else
            {
                return BinaryPrimitives.ReadUInt16BigEndian(new[] { lo, hi });
            }
        }
        protected static ushort Absolute(ref Cpu6502 cpu)
        {
            var ret = cpu.ReadWord(cpu.PC);
            cpu.PC += 2;
            return ret;
        }
        public static (ushort addr, byte clocks) AbsoluteX(ref Cpu6502 cpu)
        {
            ushort addr = cpu.ReadWord(cpu.PC);
            cpu.PC += 2;
            ushort val = (ushort)(addr + cpu.X);
            byte hiAddr;
            byte hiVal;

            if (BitConverter.IsLittleEndian)
            {
                hiAddr = (byte)(addr >> 8);
                hiVal = (byte)(val >> 8);
            }
            else
            {
                hiAddr = (byte)(addr << 8);
                hiVal = (byte)(val << 8);
            }

            if (hiVal != hiAddr)
            {
                return (val, 1);
            }
            else
            {
                return (val, 0);
            }
        }
        public static (ushort addr, byte clocks) AbsoluteY(ref Cpu6502 cpu)
        {
            ushort addr = cpu.ReadWord(cpu.PC);
            cpu.PC += 2;
            ushort val = (ushort)(addr + cpu.Y);
            byte hiAddr;
            byte hiVal;

            if (BitConverter.IsLittleEndian)
            {
                hiAddr = (byte)(addr >> 8);
                hiVal = (byte)(val >> 8);
            }
            else
            {
                hiAddr = (byte)(addr << 8);
                hiVal = (byte)(val << 8);
            }

            if (hiVal != hiAddr)
            {
                return (val, 1);
            }
            else
            {
                return (val, 0);
            }
        }

        public static ushort Indirect(ref Cpu6502 cpu)
        {
            ushort addr = cpu.ReadWord(cpu.PC);
            cpu.PC += 2;

            byte loAddr = BitConverter.IsLittleEndian ? (byte)(addr & 0xFF) : (byte)(addr >> 8);
            byte hiAddr = BitConverter.IsLittleEndian ? (byte)(addr >> 8) : (byte)(addr & 0xFF);

            byte[] bytes = new byte[2];
            if (loAddr == 0xFF)
            {
                bytes[0] = cpu.ReadByte(addr);
                bytes[1] = cpu.ReadByte((ushort)(hiAddr << 8));
                return BinaryPrimitives.ReadUInt16LittleEndian(bytes);
            }
            else
            {
                return cpu.ReadWord(addr);
            }
        }

        /// <summary>
        /// Y  indirect indexed
        /// </summary>
        /// <param name="cpu">The cpu.</param>
        /// <returns></returns>
        public static (ushort addr, byte clocks) IndirectIndex(ref Cpu6502 cpu)
        {
            ushort addr = cpu.ReadByte(cpu.PC++);
            ushort val = (ushort)(cpu.ReadWord(addr) + cpu.Y);

            byte hiVal = BitConverter.IsLittleEndian ? (byte)(val >> 8) : (byte)(val << 8);

            if ((addr & 0xFF00) != hiVal << 8)
            {
                return (val, 1);
            }
            else
            {
                return (val, 0);
            }
        }
        /// <summary>
        /// X indexed indirect 
        /// </summary>
        /// <param name="cpu">The cpu.</param>
        /// <returns></returns>
        public static ushort IndexIndirect(ref Cpu6502 cpu)
        {
            return cpu.ReadWord((ushort)(0x0000 + (byte)(cpu.ReadByte(cpu.PC++) + cpu.X)));

        }
        public static ushort Relative(ref Cpu6502 cpu)
        {

            ushort loc = cpu.ReadByte(cpu.PC++);

            if ((loc & 0x80) == 1)
            {
                loc |= 0xFF00;
            }

            return loc;
        }
    }
}
