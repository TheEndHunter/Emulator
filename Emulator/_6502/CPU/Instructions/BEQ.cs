using Emulator._6502.Devices;

namespace Emulator._6502.CPU.Instructions
{
    public sealed class BEQ : Instruction6502
    {
        public BEQ() : base("BEQ", 2, AddrMode6502.Relative, Status6502.None)
        {
        }

        public override byte Execute(Registers6502 registers, Bus6502 bus)
        {
            var addr = Relative(registers, bus);
            var jaddr = bus.ReadWord(addr);
            byte clocks = 2;
            if (registers.GetFlag(Status6502.Zero))
            {
                clocks++;
                registers.PC = jaddr;
                if ((addr & 0xFF00) != (jaddr & 0xFF00))
                {
                    clocks++;
                }
            }
            return clocks;
        }
    }
}
