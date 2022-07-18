﻿using Emulator._6502.Devices;

namespace Emulator._6502.CPU.Instructions
{
    public sealed class RTS : Instruction6502
    {
        public RTS() : base("RTS", AddrMode6502.Implied, Status6502.None)
        {

        }

        public override byte Execute(Registers6502 registers, Bus6502 bus)
        {
            return 0;
        }
    }
}
