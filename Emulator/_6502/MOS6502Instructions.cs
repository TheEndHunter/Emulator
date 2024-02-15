using Emulator._6502.Instructions;

using System.Runtime.CompilerServices;

namespace Emulator._6502
{
    [InlineArray(256)]
    public record struct InstructionSetMOS6502
    {
        public InstructionSetMOS6502()
        {
            /*
             * Fill in the Instruction Set with only the legal opcodes,
             * the rest will be filled with nulls
             */
            this[0x00] = new BRK();
            this[0x01] = new ORA_IndexedIndirect();
            this[0x02] = null;
            this[0x03] = null;
            this[0x04] = null;
            this[0x05] = new ORA_ZeroPage();
            this[0x06] = new ASL_ZeroPage();
            this[0x07] = null;
            this[0x08] = new PHP();
            this[0x09] = new ORA_Immediate();
            this[0x0a] = new ASL_Accumulator();
            this[0x0b] = null;
            this[0x0c] = null;
            this[0x0d] = new ORA_Absolute();
            this[0x0e] = new ASL_Absolute();
            this[0x0f] = null;

            this[0x10] = new BPL();
            this[0x11] = new ORA_IndirectIndexed();
            this[0x12] = null;
            this[0x13] = null;
            this[0x14] = null;
            this[0x15] = new ORA_ZeroPageX();
            this[0x16] = new ASL_ZeroPageX();
            this[0x17] = null;
            this[0x18] = new CLC();
            this[0x19] = new ORA_AbsoluteY();
            this[0x1a] = null;
            this[0x1b] = null;
            this[0x1c] = null;
            this[0x1d] = new ORA_AbsoluteX();
            this[0x1e] = new ASL_AbsoluteX();
            this[0x1f] = null;

            this[0x20] = new JSR();
            this[0x21] = new AND_IndexedIndirect();
            this[0x22] = null;
            this[0x23] = null;
            this[0x24] = new BIT_ZeroPage();
            this[0x25] = new AND_ZeroPage();
            this[0x26] = new ROL_ZeroPage();
            this[0x27] = null;
            this[0x28] = new PLP();
            this[0x29] = new AND_Immediate();
            this[0x2a] = new ROL_Accumulator();
            this[0x2b] = null;
            this[0x2c] = new BIT_Absolute();
            this[0x2d] = new AND_Absolute();
            this[0x2e] = new ROL_Absolute();
            this[0x2f] = null;

            this[0x30] = new BMI();
            this[0x31] = new AND_IndirectIndexed();
            this[0x32] = null;
            this[0x33] = null;
            this[0x34] = null;
            this[0x35] = new AND_ZeroPageX();
            this[0x36] = new ROL_ZeroPageX();
            this[0x37] = null;
            this[0x38] = new SEC();
            this[0x39] = new AND_AbsoluteY();
            this[0x3a] = null;
            this[0x3b] = null;
            this[0x3c] = null;
            this[0x3d] = new AND_AbsoluteX();
            this[0x3e] = new ROL_AbsoluteX();
            this[0x3f] = null;

            this[0x40] = new RTI();
            this[0x41] = new EOR_IndexedIndirect();
            this[0x42] = null;
            this[0x43] = null;
            this[0x44] = null;
            this[0x45] = new EOR_ZeroPage();
            this[0x46] = new LSR_ZeroPage();
            this[0x47] = null;
            this[0x48] = new PHA();
            this[0x49] = new EOR_Immediate();
            this[0x4a] = new LSR_Accumulator();
            this[0x4b] = null;
            this[0x4c] = new JMP_Absolute();
            this[0x4d] = new EOR_Absolute();
            this[0x4e] = new LSR_Absolute();
            this[0x4f] = null;

            this[0x50] = new BVC();
            this[0x51] = new EOR_IndirectIndexed();
            this[0x52] = null;
            this[0x53] = null;
            this[0x54] = null;
            this[0x55] = new EOR_ZeroPageX();
            this[0x56] = new LSR_ZeroPageX();
            this[0x57] = null;
            this[0x58] = new CLI();
            this[0x59] = new EOR_AbsoluteY();
            this[0x5a] = null;
            this[0x5b] = null;
            this[0x5c] = null;
            this[0x5d] = new EOR_AbsoluteX();
            this[0x5e] = new LSR_AbsoluteX();
            this[0x5f] = null;

            this[0x60] = new RTS();
            this[0x61] = new ADC_IndexedIndirect();
            this[0x62] = null;
            this[0x63] = null;
            this[0x64] = null;
            this[0x65] = new ADC_ZeroPage();
            this[0x66] = new ROR_ZeroPage();
            this[0x67] = null;
            this[0x68] = new PLA();
            this[0x69] = new ADC_Immediate();
            this[0x6a] = new ROR_Accumulator();
            this[0x6b] = null;
            this[0x6c] = new JMP_Indirect();
            this[0x6d] = new ADC_Absolute();
            this[0x6e] = new ROR_AbsoluteX();
            this[0x6f] = null;

            this[0x70] = new BVS();
            this[0x71] = new ADC_IndirectIndexed();
            this[0x72] = null;
            this[0x73] = null;
            this[0x74] = null;
            this[0x75] = new ADC_ZeroPageX();
            this[0x76] = new ROR_ZeroPageX();
            this[0x77] = null;
            this[0x78] = new SEI();
            this[0x79] = new ADC_AbsoluteY();
            this[0x7a] = null;
            this[0x7b] = null;
            this[0x7c] = null;
            this[0x7d] = new ADC_AbsoluteX();
            this[0x7e] = new ROR_Absolute();
            this[0x7f] = null;

            this[0x80] = null;
            this[0x81] = new STA_IndexedIndirect();
            this[0x82] = null;
            this[0x83] = null;
            this[0x84] = new STY_ZeroPage();
            this[0x85] = new STA_ZeroPage();
            this[0x86] = new STX_ZeroPage();
            this[0x87] = null;
            this[0x88] = new DEY();
            this[0x89] = null;
            this[0x8a] = new TXA();
            this[0x8b] = null;
            this[0x8c] = new STY_Absolute();
            this[0x8d] = new STA_Absolute();
            this[0x8e] = new STX_Absolute();
            this[0x8f] = null;

            this[0x90] = new BCC();
            this[0x91] = new STA_IndirectIndexed();
            this[0x92] = null;
            this[0x93] = null;
            this[0x94] = new STY_ZeroPageX();
            this[0x95] = new STA_ZeroPageX();
            this[0x96] = new STX_ZeroPageY();
            this[0x97] = null;
            this[0x98] = new TYA();
            this[0x99] = new STA_AbsoluteY();
            this[0x9a] = new TXS();
            this[0x9b] = null;
            this[0x9c] = null;
            this[0x9d] = new STA_AbsoluteX();
            this[0x9e] = null;
            this[0x9f] = null;

            this[0xa0] = new LDY_Immediate();
            this[0xa1] = new LDA_IndexedIndirect();
            this[0xa2] = new LDX_Immediate();
            this[0xa3] = null;
            this[0xa4] = new LDY_ZeroPage();
            this[0xa5] = new LDA_ZeroPage();
            this[0xa6] = new LDX_ZeroPage();
            this[0xa7] = null;
            this[0xa8] = new TAY();
            this[0xa9] = new LDA_Immediate();
            this[0xaa] = new TAX();
            this[0xab] = null;
            this[0xac] = new LDY_Absolute();
            this[0xad] = new LDA_Absolute();
            this[0xae] = new LDX_Absolute();
            this[0xaf] = null;

            this[0xb0] = new BCS();
            this[0xb1] = new LDA_IndirectIndexed();
            this[0xb2] = null;
            this[0xb3] = null;
            this[0xb4] = new LDY_ZeroPageX();
            this[0xb5] = new LDA_ZeroPageX();
            this[0xb6] = new LDX_ZeroPageY();
            this[0xb7] = null;
            this[0xb8] = new CLV();
            this[0xb9] = new LDA_AbsoluteY();
            this[0xba] = new TSX();
            this[0xbb] = null;
            this[0xbc] = new LDY_AbsoluteX();
            this[0xbd] = new LDA_AbsoluteX();
            this[0xbe] = new LDX_AbsoluteY();
            this[0xbf] = null;
            
            this[0xc0] = new CPY_Immediate();
            this[0xc1] = new CMP_IndexedIndirect();
            this[0xc2] = null;
            this[0xc3] = null;
            this[0xc4] = new CPY_ZeroPage();
            this[0xc5] = new CMP_ZeroPage();
            this[0xc6] = new DEC_ZeroPage();
            this[0xc7] = null;
            this[0xc8] = new INY();
            this[0xc9] = new CMP_Immediate();
            this[0xca] = new DEX();
            this[0xcb] = null;
            this[0xcc] = new CPY_Absolute();
            this[0xcd] = new CMP_Absolute();
            this[0xce] = new DEC_Absolute();
            this[0xcf] = null;
            
            this[0xd0] = new BNE();
            this[0xd1] = new CMP_IndirectIndexed();
            this[0xd2] = null;
            this[0xd3] = null;
            this[0xd4] = null;
            this[0xd5] = new CMP_ZeroPageX();
            this[0xd6] = new DEC_ZeroPageX();
            this[0xd7] = null;
            this[0xd8] = new CLD();
            this[0xd9] = new CMP_AbsoluteY();
            this[0xda] = null;
            this[0xdb] = null;
            this[0xdc] = null;
            this[0xdd] = new CMP_AbsoluteX();
            this[0xde] = new DEC_AbsoluteX();
            this[0xdf] = null;
            
            this[0xe0] = new CPX_Immediate();
            this[0xe1] = new SBC_IndexedIndirect();
            this[0xe2] = null;
            this[0xe3] = null;
            this[0xe4] = new CPX_ZeroPage();
            this[0xe5] = new SBC_ZeroPage();
            this[0xe6] = new INC_ZeroPage();
            this[0xe7] = null;
            this[0xe8] = new INX();
            this[0xe9] = new SBC_Immediate();
            this[0xea] = new NOP();
            this[0xec] = new CPX_Absolute();
            this[0xed] = new SBC_Absolute();
            this[0xee] = new INC_Absolute();
            this[0xef] = null;
            
            this[0xf0] = new BEQ();
            this[0xf1] = new SBC_IndirectIndexed();
            this[0xf2] = null;
            this[0xf3] = null;
            this[0xf4] = null;
            this[0xf5] = new SBC_ZeroPageX();
            this[0xf6] = new INC_ZeroPageX();
            this[0xf7] = null;
            this[0xf8] = new SED();
            this[0xf9] = new SBC_AbsoluteY();
            this[0xfa] = null;
            this[0xfb] = null;
            this[0xfc] = null;
            this[0xfd] = new SBC_AbsoluteX();
            this[0xfe] = new INC_AbsoluteX();
            this[0xff] = null;

        }
        private Instruction6502? element0;

        public Instruction6502? this[byte opcode]
        {
            get
            {
                return Unsafe.Add(ref element0, opcode);
            }
            private set
            {
                Unsafe.Add(ref element0, opcode) = value;
            }
        }
    }

    /// <summary>
    /// Instruction Set Decoder class
    /// </summary>
    public class MOS6502Instructions
    {
        private readonly Instruction6502 _unknown;
        //private readonly Dictionary<byte, Instruction6502> _instructions;
        private readonly InstructionSetMOS6502 _instructions;

        /// <summary>
        /// Initializes a new instance of the <see cref="MOS6502Instructions"/> class.
        /// </summary>
        public MOS6502Instructions()
        {
            _unknown = new UnknownInstruction();
            _instructions = new InstructionSetMOS6502();
        }

        public Instruction6502 this[byte opcode]
        {
            get
            {
                Instruction6502? v = _instructions[opcode];

                if (v is null)
                {
#if DEBUG
                    Console.WriteLine($"DEBUG: Unknown Opcode {opcode:X2}({opcode}) attempted to execute");
#else
                    Console.Error.WriteLine($"Unknown Opcode {opcode:X2}({opcode}) attempted to execute");
#endif
                    return _unknown;
                }

                return v;
            }
        }
    }
}
