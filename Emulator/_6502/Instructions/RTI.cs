

namespace Emulator._6502.Instructions
{
    public sealed class RTI : Instruction6502
    {
        public RTI() : base("RTI", 1, AddrMode6502.Implied, 0)
        {
        }

        public override byte Execute(Cpu6502 cpu)
        {
            cpu.PopStatus();
            cpu.PopPC();
            cpu.ClearIRQ();
            return 6;
        }
    }
}
