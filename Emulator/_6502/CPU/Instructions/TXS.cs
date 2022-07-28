﻿using Emulator._6502.Devices;

namespace Emulator._6502.CPU.Instructions
{
    public sealed class TXS : Instruction6502
    {
        public TXS() : base("TXS", 1, AddrMode6502.Implied, Status6502.None)
        {

        }

        public override byte Execute(Registers6502 registers, Bus6502 bus)
        {
            registers.STKP = registers.X;
            return 2;
        }
    }
}
