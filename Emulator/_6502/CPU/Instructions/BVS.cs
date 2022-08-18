using Emulator._6502.Devices;

namespace Emulator._6502.CPU.Instructions
{
    public sealed class BVS : Instruction6502
    {
        public BVS() : base("BVS", 2, AddrMode6502.Relative, Status6502.None)
        {
        }

        public override byte Execute(ref Registers6502 registers, Bus6502 bus)
        {
            var addr = Relative(ref registers, bus);
            var jaddr = bus.ReadWord(addr);
            byte clocks = 2;
            if (registers.GetFlag(Status6502.OverFlow))
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
