

namespace Emulator._6502.Instructions
{
    public sealed class UnknownInstruction : Instruction6502
    {
        public UnknownInstruction() : base("???", 1, AddrMode6502.None, 0)
        {

        }

        public override byte Execute(Cpu6502 cpu)
        {
            throw new NotImplementedException("This Instruction is Not Implemented!");
        }
    }
}
