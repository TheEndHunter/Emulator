

namespace Emulator._6502.Instructions
{
    public sealed class JSR : Instruction6502
    {
        public JSR() : base("JSR", 3, AddrMode6502.Absolute, 0)
        {
        }

        public override byte Execute(Cpu6502 cpu)
        {
            var addr = Absolute(ref cpu);
            cpu.WriteWord((ushort)(0x0100 + cpu.STKP), (ushort)(cpu.PC - 1));
            cpu.STKP -= 2;
            cpu.PC = addr;
            return 6;
        }
    }
}

