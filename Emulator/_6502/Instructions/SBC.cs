

namespace Emulator._6502.Instructions
{
    public abstract class SBC : Instruction6502
    {
        protected SBC(byte bytesUsed, AddrMode6502 mode) : base("SBC", bytesUsed, mode, Status6502.Carry | Status6502.Zero | Status6502.OverFlow | Status6502.Negative)
        {
        }
        protected static void SetFlags(ref Cpu6502 cpu, ushort temp, ushort value)
        {

            // The carry flag out exists in the high byte bit 0
            cpu.SetFlag(Status6502.Carry, (temp & 0xFF00) > 0);

            // The Zero flag is set if the result is 0
            cpu.SetFlag(Status6502.Zero, (temp & 0x00FF) == 0x00);

            // The Overflow flag is set if the results sign has incorrectly flipped.
            cpu.SetFlag(Status6502.OverFlow, ((temp ^ cpu.A) & (temp ^ value) & 0x0080) > 0);

            // The negative flag is set to the most significant bit of the result
            cpu.SetFlag(Status6502.Negative, (temp & 0x80) > 0);
        }
    }

    public sealed class SBC_Immediate : SBC
    {
        public SBC_Immediate() : base(2, AddrMode6502.Immediate)
        {
        }

        public override byte Execute(Cpu6502 cpu)
        {
            ushort data = (ushort)(cpu.ReadByte(cpu.PC++) ^ 0x00FF);
            ushort temp = (ushort)(cpu.A + data + (ushort)(cpu.GetFlag(Status6502.Carry) ? 0x0001 : 0x0000));
            SetFlags(ref cpu, temp, data);
            cpu.A = (byte)(temp & 0x00FF);
            return 2;
        }
    }

    public sealed class SBC_IndexedIndirect : SBC
    {
        public SBC_IndexedIndirect() : base(2, AddrMode6502.IndexedIndirect)
        {
        }

        public override byte Execute(Cpu6502 cpu)
        {
            ushort data = (ushort)(cpu.ReadByte(IndexIndirect(ref cpu)) ^ 0x00FF);
            ushort temp = (ushort)(cpu.A + data + (ushort)(cpu.GetFlag(Status6502.Carry) ? 0x0001 : 0x0000));
            SetFlags(ref cpu, temp, data);
            cpu.A = (byte)(temp & 0x00FF);
            return 6;
        }
    }

    public sealed class SBC_IndirectIndexed : SBC
    {
        public SBC_IndirectIndexed() : base(2, AddrMode6502.IndirectIndexed)
        {
        }

        public override byte Execute(Cpu6502 cpu)
        {

            var (addr, clocks) = IndirectIndex(ref cpu);
            ushort data = (ushort)(cpu.ReadByte(addr) ^ 0x00FF);
            ushort temp = (ushort)(cpu.A + data + (ushort)(cpu.GetFlag(Status6502.Carry) ? 0x0001 : 0x0000));
            SetFlags(ref cpu, temp, data);
            cpu.A = (byte)(temp & 0x00FF);
            return (byte)(5 + clocks);
        }
    }

    public sealed class SBC_ZeroPage : SBC
    {
        public SBC_ZeroPage() : base(2, AddrMode6502.ZeroPage)
        {
        }

        public override byte Execute(Cpu6502 cpu)
        {
            ushort data = (ushort)(cpu.ReadByte(ZeroPage(ref cpu)) ^ 0x00FF);
            ushort temp = (ushort)(cpu.A + data + (ushort)(cpu.GetFlag(Status6502.Carry) ? 0x0001 : 0x0000));
            SetFlags(ref cpu, temp, data);
            cpu.A = (byte)(temp & 0x00FF);
            return 3;
        }
    }
    public sealed class SBC_Absolute : SBC
    {
        public SBC_Absolute() : base(3, AddrMode6502.Absolute)
        {
        }

        public override byte Execute(Cpu6502 cpu)
        {
            ushort data = (ushort)(cpu.ReadByte(Absolute(ref cpu)) ^ 0x00FF);
            ushort temp = (ushort)(cpu.A + data + (ushort)(cpu.GetFlag(Status6502.Carry) ? 0x0001 : 0x0000));
            SetFlags(ref cpu, temp, data);
            cpu.A = (byte)(temp & 0x00FF);
            return 4;
        }
    }

    public sealed class SBC_ZeroPageX : SBC
    {
        public SBC_ZeroPageX() : base(2, AddrMode6502.ZeroPageX)
        {
        }

        public override byte Execute(Cpu6502 cpu)
        {
            ushort data = (ushort)(cpu.ReadByte(ZeroPageX(ref cpu)) ^ 0x00FF);
            ushort temp = (ushort)(cpu.A + data + (ushort)(cpu.GetFlag(Status6502.Carry) ? 0x0001 : 0x0000));
            SetFlags(ref cpu, temp, data);
            cpu.A = (byte)(temp & 0x00FF);
            return 4;
        }
    }
    public sealed class SBC_AbsoluteX : SBC
    {
        public SBC_AbsoluteX() : base(3, AddrMode6502.AbsoluteX)
        {
        }

        public override byte Execute(Cpu6502 cpu)
        {
            var (addr, clocks) = AbsoluteX(ref cpu);
            ushort data = (ushort)(cpu.ReadByte(addr) ^ 0x00FF);
            ushort temp = (ushort)(cpu.A + data + (ushort)(cpu.GetFlag(Status6502.Carry) ? 0x0001 : 0x0000));
            SetFlags(ref cpu, temp, data);
            cpu.A = (byte)(temp & 0x00FF);
            return (byte)(4 + clocks);
        }
    }
    public sealed class SBC_AbsoluteY : SBC
    {
        public SBC_AbsoluteY() : base(3, AddrMode6502.AbsoluteY)
        {
        }

        public override byte Execute(Cpu6502 cpu)
        {
            var (addr, clocks) = AbsoluteY(ref cpu);
            ushort data = (ushort)(cpu.ReadByte(addr) ^ 0x00FF);
            ushort temp = (ushort)(cpu.A + data + (ushort)(cpu.GetFlag(Status6502.Carry) ? 0x0001 : 0x0000));
            SetFlags(ref cpu, temp, data);
            cpu.A = (byte)(temp & 0x00FF);
            return (byte)(4 + clocks);
        }
    }
}

