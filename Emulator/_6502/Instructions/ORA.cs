

namespace Emulator._6502.Instructions
{
    public abstract class ORA(byte bytesUsed, AddrMode6502 mode) : Instruction6502("ORA", bytesUsed, mode, Status6502.Zero | Status6502.Negative)
    {
        protected static void SetFlags(ref Cpu6502 cpu, byte data)
        {
            // The Zero flag is set if the result is 0
            cpu.SetFlag(Status6502.Zero, data == 0x00);

            // The negative flag is set to the most significant bit of the result
            cpu.SetFlag(Status6502.Negative, (data & 0x80) > 0);
        }
    }
    public sealed class ORA_IndexedIndirect : ORA
    {
        public ORA_IndexedIndirect() : base(2, AddrMode6502.IndexedIndirect)
        {

        }
        public override byte Execute(Cpu6502 cpu)
        {
            var (addr, clocks) = IndirectIndex(ref cpu);
            cpu.A |= cpu.ReadByte(addr);
            SetFlags(ref cpu, cpu.A);
            return (byte)(5 + clocks);

        }
    }
    public sealed class ORA_ZeroPage : ORA
    {
        public ORA_ZeroPage() : base(2, AddrMode6502.ZeroPage)
        {
        }
        public override byte Execute(Cpu6502 cpu)
        {
            cpu.A |= cpu.ReadByte(ZeroPage(ref cpu));
            SetFlags(ref cpu, cpu.A);
            return 3;
        }
    }
    public sealed class ORA_Immediate : ORA
    {
        public ORA_Immediate() : base(2, AddrMode6502.Immediate)
        {

        }
        public override byte Execute(Cpu6502 cpu)
        {
            cpu.A |= cpu.ReadByte(cpu.PC++);
            SetFlags(ref cpu, cpu.A);
            return 2;
        }
    }
    public sealed class ORA_Absolute : ORA
    {
        public ORA_Absolute() : base(3, AddrMode6502.Absolute)
        {

        }
        public override byte Execute(Cpu6502 cpu)
        {
            cpu.A |= cpu.ReadByte(Absolute(ref cpu));
            SetFlags(ref cpu, cpu.A);
            return 4;
        }
    }
    public sealed class ORA_IndirectIndexed : ORA
    {
        public ORA_IndirectIndexed() : base(2, AddrMode6502.IndirectIndexed)
        {

        }
        public override byte Execute(Cpu6502 cpu)
        {
            cpu.A |= cpu.ReadByte(IndexIndirect(ref cpu));
            SetFlags(ref cpu, cpu.A);
            return 6;
        }
    }
    public sealed class ORA_AbsoluteY : ORA
    {
        public ORA_AbsoluteY() : base(3, AddrMode6502.AbsoluteY)
        {

        }
        public override byte Execute(Cpu6502 cpu)
        {
            var (addr, clocks) = AbsoluteY(ref cpu);
            cpu.A |= cpu.ReadByte(addr);
            SetFlags(ref cpu, cpu.A);
            return (byte)(4 + clocks);
        }
    }
    public sealed class ORA_AbsoluteX : ORA
    {
        public ORA_AbsoluteX() : base(3, AddrMode6502.AbsoluteX)
        {

        }
        public override byte Execute(Cpu6502 cpu)
        {
            var (addr, clocks) = AbsoluteX(ref cpu);
            cpu.A |= cpu.ReadByte(addr);
            SetFlags(ref cpu, cpu.A);
            return (byte)(4 + clocks);
        }
    }
    public sealed class ORA_ZeroPageX : ORA
    {
        public ORA_ZeroPageX() : base(2, AddrMode6502.ZeroPageX)
        {

        }
        public override byte Execute(Cpu6502 cpu)
        {
            cpu.A |= cpu.ReadByte(ZeroPageX(ref cpu));
            SetFlags(ref cpu, cpu.A);
            return 4;
        }
    }
}
