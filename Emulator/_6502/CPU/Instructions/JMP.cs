using Emulator._6502.Devices;

namespace Emulator._6502.CPU.Instructions
{
    public abstract class JMP : Instruction6502
    {
        protected JMP(byte bytesUsed, AddrMode6502 mode) : base("JMP", bytesUsed, mode, Status6502.None)
        {
        }
    }

    public sealed class JMP_Absolute : JMP
    {
        public JMP_Absolute() : base(3, AddrMode6502.Absolute)
        {
        }

        public override byte Execute(ref Registers6502 registers, Bus6502 bus)
        {
            registers.PC = Absolute(ref registers, bus);
            return 3;
        }
    }

    public sealed class JMP_Indirect : JMP
    {
        public JMP_Indirect() : base(3, AddrMode6502.Indirect)
        {
        }

        public override byte Execute(ref Registers6502 registers, Bus6502 bus)
        {
            registers.PC = Indirect(ref registers, bus);
            return 5;
        }
    }
}

