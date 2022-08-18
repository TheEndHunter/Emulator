using Emulator._6502.Devices;

namespace Emulator._6502.CPU.Instructions
{
    public abstract class STA : Instruction6502
    {
        protected STA(byte bytesUsed, AddrMode6502 mode) : base("STA", bytesUsed, mode, Status6502.None)
        {
        }
    }

    public sealed class STA_IndexedIndirect : STA
    {
        public STA_IndexedIndirect() : base(2, AddrMode6502.IndexedIndirect)
        {
        }

        public override byte Execute(ref Registers6502 registers, Bus6502 bus)
        {
            bus.Write(bus.ReadWord(IndexIndirect(ref registers, bus).addr), registers.A);
            return 6;
        }
    }

    public sealed class STA_IndirectIndexed : STA
    {
        public STA_IndirectIndexed() : base(2, AddrMode6502.IndirectIndexed)
        {
        }

        public override byte Execute(ref Registers6502 registers, Bus6502 bus)
        {
            bus.Write(bus.ReadWord(IndirectIndex(ref registers, bus)), registers.A);
            return 6;
        }
    }

    public sealed class STA_ZeroPage : STA
    {
        public STA_ZeroPage() : base(2, AddrMode6502.ZeroPage)
        {
        }

        public override byte Execute(ref Registers6502 registers, Bus6502 bus)
        {
            bus.Write(bus.ReadWord(ZeroPage(ref registers, bus)), registers.A);
            return 3;
        }
    }
    public sealed class STA_Absolute : STA
    {
        public STA_Absolute() : base(3, AddrMode6502.Absolute)
        {
        }

        public override byte Execute(ref Registers6502 registers, Bus6502 bus)
        {
            bus.Write(bus.ReadWord(Absolute(ref registers, bus)), registers.A);
            return 4;
        }
    }

    public sealed class STA_ZeroPageX : STA
    {
        public STA_ZeroPageX() : base(2, AddrMode6502.ZeroPageX)
        {
        }

        public override byte Execute(ref Registers6502 registers, Bus6502 bus)
        {
            bus.Write(bus.ReadWord(ZeroPageX(ref registers, bus)), registers.A);
            return 4;
        }
    }
    public sealed class STA_AbsoluteX : STA
    {
        public STA_AbsoluteX() : base(3, AddrMode6502.AbsoluteX)
        {
        }

        public override byte Execute(ref Registers6502 registers, Bus6502 bus)
        {
            bus.Write(bus.ReadWord(AbsoluteX(ref registers, bus).addr), registers.A);
            return 5;
        }
    }
    public sealed class STA_AbsoluteY : STA
    {
        public STA_AbsoluteY() : base(3, AddrMode6502.AbsoluteY)
        {
        }

        public override byte Execute(ref Registers6502 registers, Bus6502 bus)
        {
            bus.Write(bus.ReadWord(AbsoluteY(ref registers, bus).addr), registers.A);
            return 5;
        }
    }
}

