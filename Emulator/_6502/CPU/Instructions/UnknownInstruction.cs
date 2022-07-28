using Emulator._6502.Devices;

namespace Emulator._6502.CPU.Instructions
{
    public sealed class UnknownInstruction : Instruction6502
    {
        public UnknownInstruction() : base("???", 1, AddrMode6502.None, Status6502.None)
        {

        }

        public override byte Execute(Registers6502 registers, Bus6502 bus)
        {
            throw new NotImplementedException("This Instruction is Not Implemented!");
        }
    }
}
