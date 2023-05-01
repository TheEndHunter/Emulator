

namespace Emulator._6502.Instructions
{
    public sealed class BRK : Instruction6502
    {
        public BRK() : base("BRK", 1, AddrMode6502.Implied, 0)
        {
        }

        public override byte Execute(Cpu6502 cpu)
        {
            cpu.PushPC();
            cpu.PushStatus();
            cpu.SetFlag(Status6502.Break, true);
            cpu.PC = cpu.ReadWord(0xFFFE);
            return 7;
        }
    }
}
