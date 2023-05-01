

namespace Emulator._6502.Instructions
{
    public abstract class STX : Instruction6502
    {
        protected STX(byte bytesUsed, AddrMode6502 mode) : base("STX", bytesUsed, mode, 0)
        {
        }
    }

    public sealed class STX_ZeroPage : STX
    {
        public STX_ZeroPage() : base(2, AddrMode6502.ZeroPage)
        {
        }

        public override byte Execute(Cpu6502 cpu)
        {
            cpu.WriteByte(cpu.ReadWord(ZeroPage(ref cpu)), cpu.X);
            return 3;
        }
    }
    public sealed class STX_Absolute : STX
    {
        public STX_Absolute() : base(3, AddrMode6502.Absolute)
        {
        }

        public override byte Execute(Cpu6502 cpu)
        {
            cpu.WriteByte(cpu.ReadWord(Absolute(ref cpu)), cpu.X);
            return 4;
        }
    }

    public sealed class STX_ZeroPageY : STX
    {
        public STX_ZeroPageY() : base(2, AddrMode6502.ZeroPageY)
        {
        }

        public override byte Execute(Cpu6502 cpu)
        {
            cpu.WriteByte(cpu.ReadWord(ZeroPageY(ref cpu)), cpu.X);
            return 4;
        }
    }
}

