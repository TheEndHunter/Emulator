

namespace Emulator._6502.Instructions
{
    public abstract class CMP : Instruction6502
    {
        protected CMP(byte bytesUsed, AddrMode6502 mode) : base("CMP", bytesUsed, mode, Status6502.Carry | Status6502.Zero | Status6502.Negative)
        {
        }
        protected static void SetFlags(ref Cpu6502 cpu, byte fetched)
        {
            var temp = cpu.A - fetched;
            // The carry flag out exists in the high byte bit 0
            cpu.SetFlag(Status6502.Carry, cpu.A >= fetched);

            // The Zero flag is set if the result is 0
            cpu.SetFlag(Status6502.Zero, (temp & 0x00FF) == 0x0000);

            // The negative flag is set to the most significant bit of low byte the result
            cpu.SetFlag(Status6502.Negative, (temp & 0x0080) > 0);
        }
    }

    public sealed class CMP_IndexedIndirect : CMP
    {
        public CMP_IndexedIndirect() : base(2, AddrMode6502.IndexedIndirect)
        {

        }

        public override byte Execute(Cpu6502 cpu)
        {
            SetFlags(ref cpu, cpu.ReadByte(IndexIndirect(ref cpu)));
            return 6;
        }
    }
    public sealed class CMP_IndirectIndexed : CMP
    {
        public CMP_IndirectIndexed() : base(2, AddrMode6502.IndirectIndexed)
        {

        }

        public override byte Execute(Cpu6502 cpu)
        {


            var (addr, clocks) = IndirectIndex(ref cpu);
            SetFlags(ref cpu, cpu.ReadByte(addr));
            return (byte)(5 + clocks);
        }
    }

    public sealed class CMP_ZeroPage : CMP
    {
        public CMP_ZeroPage() : base(2, AddrMode6502.ZeroPage)
        {

        }

        public override byte Execute(Cpu6502 cpu)
        {
            SetFlags(ref cpu, cpu.ReadByte(ZeroPage(ref cpu)));
            return 3;
        }
    }

    public sealed class CMP_Immediate : CMP
    {
        public CMP_Immediate() : base(2, AddrMode6502.Immediate)
        {

        }

        public override byte Execute(Cpu6502 cpu)
        {
            SetFlags(ref cpu, cpu.ReadByte(cpu.PC++));
            return 2;
        }
    }

    public sealed class CMP_Absolute : CMP
    {
        public CMP_Absolute() : base(3, AddrMode6502.Absolute)
        {

        }

        public override byte Execute(Cpu6502 cpu)
        {
            SetFlags(ref cpu, cpu.ReadByte(Absolute(ref cpu)));
            return 4;
        }
    }

    public sealed class CMP_ZeroPageX : CMP
    {
        public CMP_ZeroPageX() : base(2, AddrMode6502.ZeroPageX)
        {

        }

        public override byte Execute(Cpu6502 cpu)
        {
            SetFlags(ref cpu, cpu.ReadByte(ZeroPageX(ref cpu)));
            return 4;
        }
    }

    public sealed class CMP_AbsoluteY : CMP
    {
        public CMP_AbsoluteY() : base(3, AddrMode6502.AbsoluteY)
        {

        }

        public override byte Execute(Cpu6502 cpu)
        {
            var (addr, clocks) = AbsoluteX(ref cpu);
            SetFlags(ref cpu, cpu.ReadByte(addr));
            return (byte)(4 + clocks);
        }
    }

    public sealed class CMP_AbsoluteX : CMP
    {
        public CMP_AbsoluteX() : base(3, AddrMode6502.AbsoluteX)
        {

        }

        public override byte Execute(Cpu6502 cpu)
        {
            var (addr, clocks) = AbsoluteY(ref cpu);
            SetFlags(ref cpu, cpu.ReadByte(addr));
            return (byte)(4 + clocks);
        }
    }
}
