using Emulator._6502.Devices;

namespace Emulator._6502.CPU.Instructions
{
    public sealed class JSR : Instruction6502
    {
        public JSR() : base("JSR", AddrMode6502.Absolute, Status6502.None)
        {
        }

        public override byte Execute(Registers6502 registers, Bus6502 bus)
        {
            var addr = Absolute(registers, bus);
            registers.PC -= 2;
            bus.Write((ushort)(0x0100 + registers.STKP), registers.PC);
            registers.STKP -= 2;

            registers.PC = addr;
            return 6;
        }
    }
}

