namespace Emulator._6502.Instructions
{
    public sealed class CLC : Instruction6502
    {
        public CLC() : base("CLC", 1, AddrMode6502.Implied, Status6502.Carry)
        {
        }

        public override byte Execute(Cpu6502 cpu)
        {
            cpu.SetFlag(Status6502.Carry, false);
            return 2;
        }
    }
}
