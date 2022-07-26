using Emulator._6502.Devices;

namespace Emulator._6502.CPU.Instructions
{
    public abstract class JMP : Instruction6502
    {
        protected JMP(AddrMode6502 mode) : base("JMP", mode, Status6502.None)
        {
        }
    }

    public sealed class JMP_Absolute : JMP
    {
        public JMP_Absolute() : base(AddrMode6502.Absolute)
        {
        }

        public override byte Execute(Registers6502 registers, Bus6502 bus)
        {
            registers.PC = Absolute(registers, bus);
            return 3;
        }
    }

    public sealed class JMP_Indirect : JMP
    {
        public JMP_Indirect() : base(AddrMode6502.Indirect)
        {
        }

        public override byte Execute(Registers6502 registers, Bus6502 bus)
        {
            registers.PC = Indirect(registers, bus);
            return 5;
        }
    }
}

