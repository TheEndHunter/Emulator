﻿using Emulator._6502.Instructions;

using System.Collections.Frozen;

namespace Emulator._6502
{
    /// <summary>
    /// Instruction Set Decoder class
    /// </summary>
    public class InstructionSet6502
    {
        private readonly Instruction6502 m_Unknown;
        //private readonly Dictionary<byte, Instruction6502> m_Instructions;
        private readonly FrozenDictionary<byte, Instruction6502> m_Instructions;

        /// <summary>
        /// Initializes a new instance of the <see cref="InstructionSet6502"/> class.
        /// </summary>
        public InstructionSet6502()
        {
            m_Unknown = new UnknownInstruction();
            m_Instructions = new Dictionary<byte, Instruction6502>()
            {
                {0x00, new BRK()},
                {0x01, new ORA_IndexedIndirect()},
                {0x05, new ORA_ZeroPage()},
                {0x06, new ASL_ZeroPage()},
                {0x08, new PHP()},
                {0x09, new ORA_Immediate()},
                {0x0a, new ASL_Acumulator()},
                {0x0d, new ORA_Absolute()},
                {0x0e, new ASL_Absolute()},
                {0x10, new BPL()},
                {0x11, new ORA_IndirectIndexed()},
                {0x15, new ORA_ZeroPageX()},
                {0x16, new ASL_ZeroPageX()},
                {0x18, new CLC()},
                {0x19, new ORA_AbsoluteY()},
                {0x1d, new ORA_AbsoluteX()},
                {0x1e, new ASL_AbsoluteX()},
                {0x20, new JSR()},
                {0x21, new AND_IndexedIndirect()},
                {0x24, new BIT_ZeroPage()},
                {0x25, new AND_ZeroPage()},
                {0x26, new ROL_ZeroPage()},
                {0x28, new PLP()},
                {0x29, new AND_Immediate()},
                {0x2a, new ROL_Accumulator()},
                {0x2c, new BIT_Absolute()},
                {0x2d, new AND_Absolute()},
                {0x2e, new ROL_Absolute()},
                {0x30, new BMI()},
                {0x31, new AND_IndirectIndexed()},
                {0x35, new AND_ZeroPageX()},
                {0x36, new ROL_ZeroPageX()},
                {0x38, new SEC()},
                {0x39, new AND_AbsoluteY()},
                {0x3d, new AND_AbsoluteX()},
                {0x3e, new ROL_AbsoluteX()},
                {0x40, new RTI()},
                {0x41, new EOR_IndexedIndirect()},
                {0x45, new EOR_ZeroPage()},
                {0x46, new LSR_ZeroPage()},
                {0x48, new PHA()},
                {0x49, new EOR_Immediate()},
                {0x4a, new LSR_Accumulator()},
                {0x4c, new JMP_Absolute()},
                {0x4d, new EOR_Absolute()},
                {0x4e, new LSR_Absolute()},
                {0x50, new BVC()},
                {0x51, new EOR_IndirectIndexed()},
                {0x55, new EOR_ZeroPageX()},
                {0x56, new LSR_ZeroPageX()},
                {0x58, new CLI()},
                {0x59, new EOR_AbsoluteY()},
                {0x5d, new EOR_AbsoluteX()},
                {0x5e, new LSR_AbsoluteX()},
                {0x60, new RTS()},
                {0x61, new ADC_IndexedIndirect()},
                {0x65, new ADC_ZeroPage()},
                {0x66, new ROR_ZeroPage()},
                {0x68, new PLA()},
                {0x69, new ADC_Immediate()},
                {0x6a, new ROR_Accumulator()},
                {0x6c, new JMP_Indirect()},
                {0x6d, new ADC_Absolute()},
                {0x6e, new ROR_AbsoluteX()},
                {0x70, new BVS()},
                {0x71, new ADC_IndirectIndexed()},
                {0x75, new ADC_ZeroPageX()},
                {0x76, new ROR_ZeroPageX()},
                {0x78, new SEI()},
                {0x79, new ADC_AbsoluteY()},
                {0x7d, new ADC_AbsoluteX()},
                {0x7e, new ROR_Absolute()},
                {0x81, new STA_IndexedIndirect()},
                {0x84, new STY_ZeroPage()},
                {0x85, new STA_ZeroPage()},
                {0x86, new STX_ZeroPage()},
                {0x88, new DEY()},
                {0x8a, new TXA()},
                {0x8c, new STY_Absolute()},
                {0x8d, new STA_Absolute()},
                {0x8e, new STX_Absolute()},
                {0x90, new BCC()},
                {0x91, new STA_IndirectIndexed()},
                {0x94, new STY_ZeroPageX()},
                {0x95, new STA_ZeroPageX()},
                {0x96, new STX_ZeroPageY()},
                {0x98, new TYA()},
                {0x99, new STA_AbsoluteY()},
                {0x9a, new TXS()},
                {0x9d, new STA_AbsoluteX()},
                {0xa0, new LDY_Immediate()},
                {0xa1, new LDA_IndexedIndirect()},
                {0xa2, new LDX_Immediate()},
                {0xa4, new LDY_ZeroPage()},
                {0xa5, new LDA_ZeroPage()},
                {0xa6, new LDX_ZeroPage()},
                {0xa8, new TAY()},
                {0xa9, new LDA_Immediate()},
                {0xaa, new TAX()},
                {0xac, new LDY_Absolute()},
                {0xad, new LDA_Absolute()},
                {0xae, new LDX_Absolute()},
                {0xb0, new BCS()},
                {0xb1, new LDA_IndirectIndexed()},
                {0xb4, new LDY_ZeroPageX()},
                {0xb5, new LDA_ZeroPageX()},
                {0xb6, new LDX_ZeroPageY()},
                {0xb8, new CLV()},
                {0xb9, new LDA_AbsoluteY()},
                {0xba, new TSX()},
                {0xbc, new LDY_AbsoluteX()},
                {0xbd, new LDA_AbsoluteX()},
                {0xbe, new LDX_AbsoluteY()},
                {0xc0, new CPY_Immediate()},
                {0xc1, new CMP_IndexedIndirect()},
                {0xc4, new CPY_ZeroPage()},
                {0xc5, new CMP_ZeroPage()},
                {0xc6, new DEC_ZeroPage()},
                {0xc8, new INY()},
                {0xc9, new CMP_Immediate()},
                {0xca, new DEX()},
                {0xcc, new CPY_Absolute()},
                {0xcd, new CMP_Absolute()},
                {0xce, new DEC_Absolute()},
                {0xd0, new BNE()},
                {0xd1, new CMP_IndirectIndexed()},
                {0xd5, new CMP_ZeroPageX()},
                {0xd6, new DEC_ZeroPageX()},
                {0xd8, new CLD()},
                {0xd9, new CMP_AbsoluteY()},
                {0xdd, new CMP_AbsoluteX()},
                {0xde, new DEC_AbsoluteX()},
                {0xe0, new CPX_Immediate()},
                {0xe1, new SBC_IndexedIndirect()},
                {0xe4, new CPX_ZeroPage()},
                {0xe5, new SBC_ZeroPage()},
                {0xe6, new INC_ZeroPage()},
                {0xe8, new INX()},
                {0xe9, new SBC_Immediate()},
                {0xea, new NOP()},
                {0xec, new CPX_Absolute()},
                {0xed, new SBC_Absolute()},
                {0xee, new INC_Absolute()},
                {0xf0, new BEQ()},
                {0xf1, new SBC_IndirectIndexed()},
                {0xf5, new SBC_ZeroPageX()},
                {0xf6, new INC_ZeroPageX()},
                {0xf8, new SED()},
                {0xf9, new SBC_AbsoluteY()},
                {0xfd, new SBC_AbsoluteX()},
                {0xfe, new INC_AbsoluteX()},
            }.ToFrozenDictionary();
        }

        public Instruction6502 this[byte opcode]
        {
            get
            {
                if (m_Instructions.ContainsKey(opcode))
                {
                    return m_Instructions[opcode];
                }
                Console.WriteLine($"Unknown Opcode {opcode:X2} attempted to execute");
                return m_Unknown;
            }
        }
    }
}
