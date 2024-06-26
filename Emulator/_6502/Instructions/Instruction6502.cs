﻿

using System.Buffers.Binary;
using System.Diagnostics;
using System.Text;

namespace Emulator._6502.Instructions
{
    /// <summary>
    /// 6502 instruction
    /// </summary>
    /// <remarks>
    /// Default Constructor
    /// </remarks>
    /// <param name="name"></param>
    /// <param name="addressMode"></param>
    /// <param name="flags"></param>
    [DebuggerDisplay($"{{{nameof(GetDebuggerDisplay)}(),nq}}")]
    public abstract class Instruction6502(string name, byte bytesUsed, AddrMode6502 addressMode, Status6502 flags)
    {
        public string Name { get; } = name;
        public AddrMode6502 AddressMode { get; } = addressMode;
        public Status6502 Flags { get; } = flags;
        public byte BytesUsed { get; } = bytesUsed;
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
                return BinaryPrimitives.ReadUInt16LittleEndian([lo, hi]);
            }
            else
            {
                return BinaryPrimitives.ReadUInt16BigEndian([lo, hi]);
            }
        }
        protected static ushort ZeroPageX(ref Cpu6502 cpu)
        {
            byte lo = (byte)(cpu.ReadByte(cpu.PC++) + cpu.X);
            if (BitConverter.IsLittleEndian)
            {
                return BinaryPrimitives.ReadUInt16LittleEndian([lo, hi]);
            }
            else
            {
                return BinaryPrimitives.ReadUInt16BigEndian([lo, hi]);
            }
        }
        protected static ushort ZeroPageY(ref Cpu6502 cpu)
        {

            byte lo = (byte)(cpu.ReadByte(cpu.PC++) + cpu.Y);
            if (BitConverter.IsLittleEndian)
            {
                return BinaryPrimitives.ReadUInt16LittleEndian([lo, hi]);
            }
            else
            {
                return BinaryPrimitives.ReadUInt16BigEndian([lo, hi]);
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
            cpu.PC += 2;


            if ((BitConverter.IsLittleEndian ? (byte)(cpu.ReadWord(cpu.PC) & 0xFF) : (byte)(cpu.ReadWord(cpu.PC) >> 8)) == 0xFF)
            {
                var b = new byte[2];
                b[0] = cpu.ReadByte(cpu.ReadWord(cpu.PC));
                b[1] = cpu.ReadByte((ushort)((BitConverter.IsLittleEndian ? (byte)(cpu.ReadWord(cpu.PC) >> 8) : (byte)(cpu.ReadWord(cpu.PC) & 0xFF)) << 8));
                return BinaryPrimitives.ReadUInt16LittleEndian(new byte[2]);
            }
            else
            {
                return cpu.ReadWord(cpu.ReadWord(cpu.PC));
            }
        }

        /// <summary>
        /// Y  indirect indexed
        /// </summary>
        /// <param name="cpu">The cpu.</param>
        /// <returns></returns>
        public static (ushort addr, byte clocks) IndirectIndex(ref Cpu6502 cpu)
        {
            ushort val = (ushort)(cpu.ReadWord(cpu.ReadByte(cpu.PC++)) + cpu.Y);

            byte hiVal = BitConverter.IsLittleEndian ? (byte)(val >> 8) : (byte)(val << 8);

            if ((cpu.ReadByte(cpu.PC++) & 0xFF00) != hiVal << 8)
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
