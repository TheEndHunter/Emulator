using System.Diagnostics;

namespace VirtualMachineTutorial
{
    public class InstructionSet6502
    {
        private readonly Dictionary<byte, Instruction6502> OpCodes;
        private readonly Instruction6502 UnknownInstruction;
        public InstructionSet6502()
        {
            UnknownInstruction = new()
            {
                Name = "???",
                AddressMode = AddrMode6502.None,
                Func = NOP,
            };
            OpCodes = new()
            {
                {0x00,new Instruction6502(){Name = "BRK",Func = BRK, AddressMode = AddrMode6502.Implied }},
                {0x01,new Instruction6502(){Name = "ORA",Func = ORA, AddressMode = AddrMode6502.IndexedIndirect ,Flags = Status6502.Zero | Status6502.Negative}},
                {0x05,new Instruction6502(){Name = "ORA",Func = ORA, AddressMode = AddrMode6502.ZeroPage ,Flags = Status6502.Zero | Status6502.Negative}},
                {0x06,new Instruction6502(){Name = "ASL",Func = ASL, AddressMode = AddrMode6502.ZeroPage ,Flags = Status6502.Carry | Status6502.Zero | Status6502.Negative}},
                {0x08,new Instruction6502(){Name = "PHP",Func = PHP, AddressMode = AddrMode6502.Implied }},
                {0x09,new Instruction6502(){Name = "ORA",Func = ORA, AddressMode = AddrMode6502.Immediate ,Flags = Status6502.Zero | Status6502.Negative}},
                {0x0a,new Instruction6502(){Name = "ASL",Func = ASL, AddressMode = AddrMode6502.Accumulator ,Flags = Status6502.Carry | Status6502.Zero | Status6502.Negative}},
                {0x0d,new Instruction6502(){Name = "ORA",Func = ORA, AddressMode = AddrMode6502.Absolute ,Flags = Status6502.Zero | Status6502.Negative}},
                {0x0e,new Instruction6502(){Name = "ASL",Func = ASL, AddressMode = AddrMode6502.Absolute ,Flags = Status6502.Carry | Status6502.Zero | Status6502.Negative}},
                {0x10,new Instruction6502(){Name = "BPL",Func = BPL, AddressMode = AddrMode6502.Relative }},
                {0x11,new Instruction6502(){Name = "ORA",Func = ORA, AddressMode = AddrMode6502.IndirectIndexed ,Flags = Status6502.Zero | Status6502.Negative}},
                {0x15,new Instruction6502(){Name = "ORA",Func = ORA, AddressMode = AddrMode6502.ZeroPageX ,Flags = Status6502.Zero | Status6502.Negative}},
                {0x16,new Instruction6502(){Name = "ASL",Func = ASL, AddressMode = AddrMode6502.ZeroPageX ,Flags = Status6502.Carry | Status6502.Zero | Status6502.Negative}},
                {0x18,new Instruction6502(){Name = "CLC",Func = CLC, AddressMode = AddrMode6502.Implied ,Flags = Status6502.Carry}},
                {0x19,new Instruction6502(){Name = "ORA",Func = ORA, AddressMode = AddrMode6502.AbsoluteY ,Flags = Status6502.Zero | Status6502.Negative}},
                {0x1d,new Instruction6502(){Name = "ORA",Func = ORA, AddressMode = AddrMode6502.AbsoluteX ,Flags = Status6502.Zero | Status6502.Negative}},
                {0x1e,new Instruction6502(){Name = "ASL",Func = ASL, AddressMode = AddrMode6502.AbsoluteX ,Flags = Status6502.Carry | Status6502.Zero | Status6502.Negative}},
                {0x20,new Instruction6502(){Name = "JSR",Func = JSR, AddressMode = AddrMode6502.Absolute }},
                {0x21,new Instruction6502(){Name = "AND",Func = AND, AddressMode = AddrMode6502.IndexedIndirect ,Flags = Status6502.Zero | Status6502.Negative}},
                {0x24,new Instruction6502(){Name = "BIT",Func = BIT, AddressMode = AddrMode6502.ZeroPage ,Flags = Status6502.Zero | Status6502.OVRFLW | Status6502.Negative}},
                {0x25,new Instruction6502(){Name = "AND",Func = AND, AddressMode = AddrMode6502.ZeroPage ,Flags = Status6502.Zero | Status6502.Negative}},
                {0x26,new Instruction6502(){Name = "ROL",Func = ROL, AddressMode = AddrMode6502.ZeroPage ,Flags = Status6502.Carry | Status6502.Zero | Status6502.Negative}},
                {0x28,new Instruction6502(){Name = "PLP",Func = PLP, AddressMode = AddrMode6502.Implied ,Flags = Status6502.Carry | Status6502.Zero | Status6502.IRQDisable | Status6502.DecimalMode | Status6502.BRK | Status6502.OVRFLW | Status6502.Negative}},
                {0x29,new Instruction6502(){Name = "AND",Func = AND, AddressMode = AddrMode6502.Immediate ,Flags = Status6502.Zero | Status6502.Negative}},
                {0x2a,new Instruction6502(){Name = "ROL",Func = ROL, AddressMode = AddrMode6502.Accumulator ,Flags = Status6502.Carry | Status6502.Zero | Status6502.Negative}},
                {0x2c,new Instruction6502(){Name = "BIT",Func = BIT, AddressMode = AddrMode6502.Absolute ,Flags = Status6502.Zero | Status6502.OVRFLW | Status6502.Negative}},
                {0x2d,new Instruction6502(){Name = "AND",Func = AND, AddressMode = AddrMode6502.Absolute ,Flags = Status6502.Zero | Status6502.Negative}},
                {0x2e,new Instruction6502(){Name = "ROL",Func = ROL, AddressMode = AddrMode6502.Absolute ,Flags = Status6502.Carry | Status6502.Zero | Status6502.Negative}},
                {0x30,new Instruction6502(){Name = "BMI",Func = BMI, AddressMode = AddrMode6502.Relative }},
                {0x31,new Instruction6502(){Name = "AND",Func = AND, AddressMode = AddrMode6502.IndirectIndexed ,Flags = Status6502.Zero | Status6502.Negative}},
                {0x35,new Instruction6502(){Name = "AND",Func = AND, AddressMode = AddrMode6502.ZeroPageX ,Flags = Status6502.Zero | Status6502.Negative}},
                {0x36,new Instruction6502(){Name = "ROL",Func = ROL, AddressMode = AddrMode6502.ZeroPageX ,Flags = Status6502.Carry | Status6502.Zero | Status6502.Negative}},
                {0x38,new Instruction6502(){Name = "SEC",Func = SEC, AddressMode = AddrMode6502.Implied ,Flags = Status6502.Carry}},
                {0x39,new Instruction6502(){Name = "AND",Func = AND, AddressMode = AddrMode6502.AbsoluteY ,Flags = Status6502.Zero | Status6502.Negative}},
                {0x3d,new Instruction6502(){Name = "AND",Func = AND, AddressMode = AddrMode6502.AbsoluteX ,Flags = Status6502.Zero | Status6502.Negative}},
                {0x3e,new Instruction6502(){Name = "ROL",Func = ROL, AddressMode = AddrMode6502.AbsoluteX ,Flags = Status6502.Carry | Status6502.Zero | Status6502.Negative}},
                {0x40,new Instruction6502(){Name = "RTI",Func = RTI, AddressMode = AddrMode6502.Implied }},
                {0x41,new Instruction6502(){Name = "EOR",Func = EOR, AddressMode = AddrMode6502.IndexedIndirect ,Flags = Status6502.Zero | Status6502.Negative}},
                {0x45,new Instruction6502(){Name = "EOR",Func = EOR, AddressMode = AddrMode6502.ZeroPage ,Flags = Status6502.Zero | Status6502.Negative}},
                {0x46,new Instruction6502(){Name = "LSR",Func = LSR, AddressMode = AddrMode6502.ZeroPage ,Flags = Status6502.Carry | Status6502.Zero | Status6502.Negative}},
                {0x48,new Instruction6502(){Name = "PHA",Func = PHA, AddressMode = AddrMode6502.Implied }},
                {0x49,new Instruction6502(){Name = "EOR",Func = EOR, AddressMode = AddrMode6502.Immediate ,Flags = Status6502.Zero | Status6502.Negative}},
                {0x4a,new Instruction6502(){Name = "LSR",Func = LSR, AddressMode = AddrMode6502.Accumulator ,Flags = Status6502.Carry | Status6502.Zero | Status6502.Negative}},
                {0x4c,new Instruction6502(){Name = "JMP",Func = JMP, AddressMode = AddrMode6502.Absolute }},
                {0x4d,new Instruction6502(){Name = "EOR",Func = EOR, AddressMode = AddrMode6502.Absolute ,Flags = Status6502.Zero | Status6502.Negative}},
                {0x4e,new Instruction6502(){Name = "LSR",Func = LSR, AddressMode = AddrMode6502.Absolute ,Flags = Status6502.Carry | Status6502.Zero | Status6502.Negative}},
                {0x50,new Instruction6502(){Name = "BVC",Func = BVC, AddressMode = AddrMode6502.Relative }},
                {0x51,new Instruction6502(){Name = "EOR",Func = EOR, AddressMode = AddrMode6502.IndirectIndexed ,Flags = Status6502.Zero | Status6502.Negative}},
                {0x55,new Instruction6502(){Name = "EOR",Func = EOR, AddressMode = AddrMode6502.ZeroPageX ,Flags = Status6502.Zero | Status6502.Negative}},
                {0x56,new Instruction6502(){Name = "LSR",Func = LSR, AddressMode = AddrMode6502.ZeroPageX ,Flags = Status6502.Carry | Status6502.Zero | Status6502.Negative}},
                {0x58,new Instruction6502(){Name = "CLI",Func = CLI, AddressMode = AddrMode6502.Implied ,Flags = Status6502.IRQDisable}},
                {0x59,new Instruction6502(){Name = "EOR",Func = EOR, AddressMode = AddrMode6502.AbsoluteY ,Flags = Status6502.Zero | Status6502.Negative}},
                {0x5d,new Instruction6502(){Name = "EOR",Func = EOR, AddressMode = AddrMode6502.AbsoluteX ,Flags = Status6502.Zero | Status6502.Negative}},
                {0x5e,new Instruction6502(){Name = "LSR",Func = LSR, AddressMode = AddrMode6502.AbsoluteX ,Flags = Status6502.Carry | Status6502.Zero | Status6502.Negative}},
                {0x60,new Instruction6502(){Name = "RTS",Func = RTS, AddressMode = AddrMode6502.Implied }},
                {0x61,new Instruction6502(){Name = "ADC",Func = ADC, AddressMode = AddrMode6502.IndexedIndirect ,Flags = Status6502.Carry | Status6502.Zero | Status6502.OVRFLW | Status6502.Negative}},
                {0x65,new Instruction6502(){Name = "ADC",Func = ADC, AddressMode = AddrMode6502.ZeroPage ,Flags = Status6502.Carry | Status6502.Zero | Status6502.OVRFLW | Status6502.Negative}},
                {0x66,new Instruction6502(){Name = "ROR",Func = ROR, AddressMode = AddrMode6502.ZeroPage ,Flags = Status6502.Carry | Status6502.Zero | Status6502.Negative}},
                {0x68,new Instruction6502(){Name = "PLA",Func = PLA, AddressMode = AddrMode6502.Implied ,Flags = Status6502.Zero | Status6502.Negative}},
                {0x69,new Instruction6502(){Name = "ADC",Func = ADC, AddressMode = AddrMode6502.Immediate ,Flags = Status6502.Carry | Status6502.Zero | Status6502.OVRFLW | Status6502.Negative}},
                {0x6a,new Instruction6502(){Name = "ROR",Func = ROR, AddressMode = AddrMode6502.Accumulator ,Flags = Status6502.Carry | Status6502.Zero | Status6502.Negative}},
                {0x6c,new Instruction6502(){Name = "JMP",Func = JMP, AddressMode = AddrMode6502.Indirect }},
                {0x6d,new Instruction6502(){Name = "ADC",Func = ADC, AddressMode = AddrMode6502.Absolute ,Flags = Status6502.Carry | Status6502.Zero | Status6502.OVRFLW | Status6502.Negative}},
                {0x6e,new Instruction6502(){Name = "ROR",Func = ROR, AddressMode = AddrMode6502.AbsoluteX ,Flags = Status6502.Carry | Status6502.Zero | Status6502.Negative}},
                {0x70,new Instruction6502(){Name = "BVS",Func = BVS, AddressMode = AddrMode6502.Relative }},
                {0x71,new Instruction6502(){Name = "ADC",Func = ADC, AddressMode = AddrMode6502.IndirectIndexed ,Flags = Status6502.Carry | Status6502.Zero | Status6502.OVRFLW | Status6502.Negative}},
                {0x75,new Instruction6502(){Name = "ADC",Func = ADC, AddressMode = AddrMode6502.ZeroPageX ,Flags = Status6502.Carry | Status6502.Zero | Status6502.OVRFLW | Status6502.Negative}},
                {0x76,new Instruction6502(){Name = "ROR",Func = ROR, AddressMode = AddrMode6502.ZeroPageX ,Flags = Status6502.Carry | Status6502.Zero | Status6502.Negative}},
                {0x78,new Instruction6502(){Name = "SEI",Func = SEI, AddressMode = AddrMode6502.Implied ,Flags = Status6502.IRQDisable}},
                {0x79,new Instruction6502(){Name = "ADC",Func = ADC, AddressMode = AddrMode6502.AbsoluteY ,Flags = Status6502.Carry | Status6502.Zero | Status6502.OVRFLW | Status6502.Negative}},
                {0x7d,new Instruction6502(){Name = "ADC",Func = ADC, AddressMode = AddrMode6502.AbsoluteX ,Flags = Status6502.Carry | Status6502.Zero | Status6502.OVRFLW | Status6502.Negative}},
                {0x7e,new Instruction6502(){Name = "ROR",Func = ROR, AddressMode = AddrMode6502.Absolute ,Flags = Status6502.Carry | Status6502.Zero | Status6502.Negative}},
                {0x81,new Instruction6502(){Name = "STA",Func = STA, AddressMode = AddrMode6502.IndexedIndirect }},
                {0x84,new Instruction6502(){Name = "STY",Func = STY, AddressMode = AddrMode6502.ZeroPage }},
                {0x85,new Instruction6502(){Name = "STA",Func = STA, AddressMode = AddrMode6502.ZeroPage }},
                {0x86,new Instruction6502(){Name = "STX",Func = STX, AddressMode = AddrMode6502.ZeroPage }},
                {0x88,new Instruction6502(){Name = "DEY",Func = DEY, AddressMode = AddrMode6502.Implied ,Flags = Status6502.Zero | Status6502.Negative}},
                {0x8a,new Instruction6502(){Name = "TXA",Func = TXA, AddressMode = AddrMode6502.Implied ,Flags = Status6502.Zero | Status6502.Negative}},
                {0x8c,new Instruction6502(){Name = "STY",Func = STY, AddressMode = AddrMode6502.Absolute }},
                {0x8d,new Instruction6502(){Name = "STA",Func = STA, AddressMode = AddrMode6502.Absolute }},
                {0x8e,new Instruction6502(){Name = "STX",Func = STX, AddressMode = AddrMode6502.Absolute }},
                {0x90,new Instruction6502(){Name = "BCC",Func = BCC, AddressMode = AddrMode6502.Relative }},
                {0x91,new Instruction6502(){Name = "STA",Func = STA, AddressMode = AddrMode6502.IndirectIndexed }},
                {0x94,new Instruction6502(){Name = "STY",Func = STY, AddressMode = AddrMode6502.ZeroPageX }},
                {0x95,new Instruction6502(){Name = "STA",Func = STA, AddressMode = AddrMode6502.ZeroPageX }},
                {0x96,new Instruction6502(){Name = "STX",Func = STX, AddressMode = AddrMode6502.ZeroPageY }},
                {0x98,new Instruction6502(){Name = "TYA",Func = TYA, AddressMode = AddrMode6502.Implied ,Flags = Status6502.Zero | Status6502.Negative}},
                {0x99,new Instruction6502(){Name = "STA",Func = STA, AddressMode = AddrMode6502.AbsoluteY }},
                {0x9a,new Instruction6502(){Name = "TXS",Func = TXS, AddressMode = AddrMode6502.Implied }},
                {0x9d,new Instruction6502(){Name = "STA",Func = STA, AddressMode = AddrMode6502.AbsoluteX }},
                {0xa0,new Instruction6502(){Name = "LDY",Func = LDY, AddressMode = AddrMode6502.Immediate ,Flags = Status6502.Zero | Status6502.Negative}},
                {0xa1,new Instruction6502(){Name = "LDA",Func = LDA, AddressMode = AddrMode6502.IndexedIndirect ,Flags = Status6502.Zero | Status6502.Negative}},
                {0xa2,new Instruction6502(){Name = "LDX",Func = LDX, AddressMode = AddrMode6502.Immediate ,Flags = Status6502.Zero | Status6502.Negative}},
                {0xa4,new Instruction6502(){Name = "LDY",Func = LDY, AddressMode = AddrMode6502.ZeroPage ,Flags = Status6502.Zero | Status6502.Negative}},
                {0xa5,new Instruction6502(){Name = "LDA",Func = LDA, AddressMode = AddrMode6502.ZeroPage ,Flags = Status6502.Zero | Status6502.Negative}},
                {0xa6,new Instruction6502(){Name = "LDX",Func = LDX, AddressMode = AddrMode6502.ZeroPage ,Flags = Status6502.Zero | Status6502.Negative}},
                {0xa8,new Instruction6502(){Name = "TAY",Func = TAY, AddressMode = AddrMode6502.Implied ,Flags = Status6502.Zero | Status6502.Negative}},
                {0xa9,new Instruction6502(){Name = "LDA",Func = LDA, AddressMode = AddrMode6502.Immediate ,Flags = Status6502.Zero | Status6502.Negative}},
                {0xaa,new Instruction6502(){Name = "TAX",Func = TAX, AddressMode = AddrMode6502.Implied ,Flags = Status6502.Zero | Status6502.Negative}},
                {0xac,new Instruction6502(){Name = "LDY",Func = LDY, AddressMode = AddrMode6502.Absolute ,Flags = Status6502.Zero | Status6502.Negative}},
                {0xad,new Instruction6502(){Name = "LDA",Func = LDA, AddressMode = AddrMode6502.Absolute ,Flags = Status6502.Zero | Status6502.Negative}},
                {0xae,new Instruction6502(){Name = "LDX",Func = LDX, AddressMode = AddrMode6502.Absolute ,Flags = Status6502.Zero | Status6502.Negative}},
                {0xb0,new Instruction6502(){Name = "BCS",Func = BCS, AddressMode = AddrMode6502.Relative }},
                {0xb1,new Instruction6502(){Name = "LDA",Func = LDA, AddressMode = AddrMode6502.IndirectIndexed ,Flags = Status6502.Zero | Status6502.Negative}},
                {0xb4,new Instruction6502(){Name = "LDY",Func = LDY, AddressMode = AddrMode6502.ZeroPageX ,Flags = Status6502.Zero | Status6502.Negative}},
                {0xb5,new Instruction6502(){Name = "LDA",Func = LDA, AddressMode = AddrMode6502.ZeroPageX ,Flags = Status6502.Zero | Status6502.Negative}},
                {0xb6,new Instruction6502(){Name = "LDX",Func = LDX, AddressMode = AddrMode6502.ZeroPageY ,Flags = Status6502.Zero | Status6502.Negative}},
                {0xb8,new Instruction6502(){Name = "CLV",Func = CLV, AddressMode = AddrMode6502.Implied ,Flags = Status6502.OVRFLW}},
                {0xb9,new Instruction6502(){Name = "LDA",Func = LDA, AddressMode = AddrMode6502.AbsoluteY ,Flags = Status6502.Zero | Status6502.Negative}},
                {0xba,new Instruction6502(){Name = "TSX",Func = TSX, AddressMode = AddrMode6502.Implied ,Flags = Status6502.Zero | Status6502.Negative}},
                {0xbc,new Instruction6502(){Name = "LDY",Func = LDY, AddressMode = AddrMode6502.AbsoluteX ,Flags = Status6502.Zero | Status6502.Negative}},
                {0xbd,new Instruction6502(){Name = "LDA",Func = LDA, AddressMode = AddrMode6502.AbsoluteX ,Flags = Status6502.Zero | Status6502.Negative}},
                {0xbe,new Instruction6502(){Name = "LDX",Func = LDX, AddressMode = AddrMode6502.AbsoluteY ,Flags = Status6502.Zero | Status6502.Negative}},
                {0xc0,new Instruction6502(){Name = "CPY",Func = CPY, AddressMode = AddrMode6502.Immediate ,Flags = Status6502.Carry | Status6502.Zero | Status6502.Negative}},
                {0xc1,new Instruction6502(){Name = "CMP",Func = CMP, AddressMode = AddrMode6502.IndexedIndirect ,Flags = Status6502.Carry | Status6502.Zero | Status6502.Negative}},
                {0xc4,new Instruction6502(){Name = "CPY",Func = CPY, AddressMode = AddrMode6502.ZeroPage ,Flags = Status6502.Carry | Status6502.Zero | Status6502.Negative}},
                {0xc5,new Instruction6502(){Name = "CMP",Func = CMP, AddressMode = AddrMode6502.ZeroPage ,Flags = Status6502.Carry | Status6502.Zero | Status6502.Negative}},
                {0xc6,new Instruction6502(){Name = "DEC",Func = DEC, AddressMode = AddrMode6502.ZeroPage ,Flags = Status6502.Zero | Status6502.Negative}},
                {0xc8,new Instruction6502(){Name = "INY",Func = INY, AddressMode = AddrMode6502.Implied ,Flags = Status6502.Zero | Status6502.Negative}},
                {0xc9,new Instruction6502(){Name = "CMP",Func = CMP, AddressMode = AddrMode6502.Immediate ,Flags = Status6502.Carry | Status6502.Zero | Status6502.Negative}},
                {0xca,new Instruction6502(){Name = "DEX",Func = DEX, AddressMode = AddrMode6502.Implied ,Flags = Status6502.Zero | Status6502.Negative}},
                {0xcc,new Instruction6502(){Name = "CPY",Func = CPY, AddressMode = AddrMode6502.Absolute ,Flags = Status6502.Carry | Status6502.Zero | Status6502.Negative}},
                {0xcd,new Instruction6502(){Name = "CMP",Func = CMP, AddressMode = AddrMode6502.Absolute ,Flags = Status6502.Carry | Status6502.Zero | Status6502.Negative}},
                {0xce,new Instruction6502(){Name = "DEC",Func = DEC, AddressMode = AddrMode6502.Absolute ,Flags = Status6502.Zero | Status6502.Negative}},
                {0xd0,new Instruction6502(){Name = "BNE",Func = BNE, AddressMode = AddrMode6502.Relative }},
                {0xd1,new Instruction6502(){Name = "CMP",Func = CMP, AddressMode = AddrMode6502.IndirectIndexed ,Flags = Status6502.Carry | Status6502.Zero | Status6502.Negative}},
                {0xd5,new Instruction6502(){Name = "CMP",Func = CMP, AddressMode = AddrMode6502.ZeroPageX ,Flags = Status6502.Carry | Status6502.Zero | Status6502.Negative}},
                {0xd6,new Instruction6502(){Name = "DEC",Func = DEC, AddressMode = AddrMode6502.ZeroPageX ,Flags = Status6502.Zero | Status6502.Negative}},
                {0xd8,new Instruction6502(){Name = "CLD",Func = CLD, AddressMode = AddrMode6502.Implied ,Flags = Status6502.DecimalMode}},
                {0xd9,new Instruction6502(){Name = "CMP",Func = CMP, AddressMode = AddrMode6502.AbsoluteY ,Flags = Status6502.Carry | Status6502.Zero | Status6502.Negative}},
                {0xdd,new Instruction6502(){Name = "CMP",Func = CMP, AddressMode = AddrMode6502.AbsoluteX ,Flags = Status6502.Carry | Status6502.Zero | Status6502.Negative}},
                {0xde,new Instruction6502(){Name = "DEC",Func = DEC, AddressMode = AddrMode6502.AbsoluteX ,Flags = Status6502.Zero | Status6502.Negative}},
                {0xe0,new Instruction6502(){Name = "CPX",Func = CPX, AddressMode = AddrMode6502.Immediate ,Flags = Status6502.Carry | Status6502.Zero | Status6502.Negative}},
                {0xe1,new Instruction6502(){Name = "SBC",Func = SBC, AddressMode = AddrMode6502.IndexedIndirect ,Flags = Status6502.Carry | Status6502.Zero | Status6502.OVRFLW | Status6502.Negative}},
                {0xe4,new Instruction6502(){Name = "CPX",Func = CPX, AddressMode = AddrMode6502.ZeroPage ,Flags = Status6502.Carry | Status6502.Zero | Status6502.Negative}},
                {0xe5,new Instruction6502(){Name = "SBC",Func = SBC, AddressMode = AddrMode6502.ZeroPage ,Flags = Status6502.Carry | Status6502.Zero | Status6502.OVRFLW | Status6502.Negative}},
                {0xe6,new Instruction6502(){Name = "INC",Func = INC, AddressMode = AddrMode6502.ZeroPage ,Flags = Status6502.Zero | Status6502.Negative}},
                {0xe8,new Instruction6502(){Name = "INX",Func = INX, AddressMode = AddrMode6502.Implied ,Flags = Status6502.Zero | Status6502.Negative}},
                {0xe9,new Instruction6502(){Name = "SBC",Func = SBC, AddressMode = AddrMode6502.Immediate ,Flags = Status6502.Carry | Status6502.Zero | Status6502.OVRFLW | Status6502.Negative}},
                {0xea,new Instruction6502(){Name = "NOP",Func = NOP, AddressMode = AddrMode6502.Implied }},
                {0xec,new Instruction6502(){Name = "CPX",Func = CPX, AddressMode = AddrMode6502.Absolute ,Flags = Status6502.Carry | Status6502.Zero | Status6502.Negative}},
                {0xed,new Instruction6502(){Name = "SBC",Func = SBC, AddressMode = AddrMode6502.Absolute ,Flags = Status6502.Carry | Status6502.Zero | Status6502.OVRFLW | Status6502.Negative}},
                {0xee,new Instruction6502(){Name = "INC",Func = INC, AddressMode = AddrMode6502.Absolute ,Flags = Status6502.Zero | Status6502.Negative}},
                {0xf0,new Instruction6502(){Name = "BEQ",Func = BEQ, AddressMode = AddrMode6502.Relative }},
                {0xf1,new Instruction6502(){Name = "SBC",Func = SBC, AddressMode = AddrMode6502.IndirectIndexed ,Flags = Status6502.Carry | Status6502.Zero | Status6502.OVRFLW | Status6502.Negative}},
                {0xf5,new Instruction6502(){Name = "SBC",Func = SBC, AddressMode = AddrMode6502.ZeroPageX ,Flags = Status6502.Carry | Status6502.Zero | Status6502.OVRFLW | Status6502.Negative}},
                {0xf6,new Instruction6502(){Name = "INC",Func = INC, AddressMode = AddrMode6502.ZeroPageX ,Flags = Status6502.Zero | Status6502.Negative}},
                {0xf8,new Instruction6502(){Name = "SED",Func = SED, AddressMode = AddrMode6502.Implied ,Flags = Status6502.DecimalMode}},
                {0xf9,new Instruction6502(){Name = "SBC",Func = SBC, AddressMode = AddrMode6502.AbsoluteY ,Flags = Status6502.Carry | Status6502.Zero | Status6502.OVRFLW | Status6502.Negative}},
                {0xfd,new Instruction6502(){Name = "SBC",Func = SBC, AddressMode = AddrMode6502.AbsoluteX ,Flags = Status6502.Carry | Status6502.Zero | Status6502.OVRFLW | Status6502.Negative}},
                {0xfe,new Instruction6502(){Name = "INC",Func = INC, AddressMode = AddrMode6502.AbsoluteX ,Flags = Status6502.Zero | Status6502.Negative}},
            };
        }

        public Instruction6502 Decode(byte opCode)
        {
            var res = OpCodes.Where(x => x.Key == opCode);
            if (res.Any()) return res.ElementAt(0).Value;
            return UnknownInstruction;
        }

        internal static byte NOP(Registers6502 registers, AddrMode6502 addressMode, Bus6502 memory)
        {
            return 0;
        }
        internal static byte BRK(Registers6502 registers, AddrMode6502 addressMode, Bus6502 memory)
        {
            if (Debugger.IsAttached)
            {
                Debugger.Break();
            }
            return 0;
        }
        internal static byte ORA(Registers6502 registers, AddrMode6502 addressMode, Bus6502 memory)
        {
            return 0;
        }
        internal static byte ASL(Registers6502 registers, AddrMode6502 addressMode, Bus6502 memory)
        {
            return 0;
        }
        internal static byte PHP(Registers6502 registers, AddrMode6502 addressMode, Bus6502 memory)
        {
            return 0;
        }
        internal static byte BPL(Registers6502 registers, AddrMode6502 addressMode, Bus6502 memory)
        {
            return 0;
        }
        internal static byte INC(Registers6502 registers, AddrMode6502 addressMode, Bus6502 memory)
        {
            return 0;
        }
        internal static byte SBC(Registers6502 registers, AddrMode6502 addressMode, Bus6502 memory)
        {
            return 0;
        }
        internal static byte SED(Registers6502 registers, AddrMode6502 addressMode, Bus6502 memory)
        {
            return 0;
        }
        internal static byte BEQ(Registers6502 registers, AddrMode6502 addressMode, Bus6502 memory)
        {
            return 0;
        }
        internal static byte CPX(Registers6502 registers, AddrMode6502 addressMode, Bus6502 memory)
        {
            return 0;
        }
        internal static byte INX(Registers6502 registers, AddrMode6502 addressMode, Bus6502 memory)
        {
            return 0;
        }
        internal static byte DEC(Registers6502 registers, AddrMode6502 addressMode, Bus6502 memory)
        {
            return 0;
        }
        internal static byte CMP(Registers6502 registers, AddrMode6502 addressMode, Bus6502 memory)
        {
            return 0;
        }
        internal static byte ADC(Registers6502 registers, AddrMode6502 addressMode, Bus6502 memory)
        {
            return 0;
        }
        internal static byte AND(Registers6502 registers, AddrMode6502 addressMode, Bus6502 memory)
        {
            return 0;
        }
        internal static byte BCC(Registers6502 registers, AddrMode6502 addressMode, Bus6502 memory)
        {
            return 0;
        }
        internal static byte TYA(Registers6502 registers, AddrMode6502 addressMode, Bus6502 memory)
        {
            return 0;
        }
        internal static byte TXS(Registers6502 registers, AddrMode6502 addressMode, Bus6502 memory)
        {
            return 0;
        }
        internal static byte TXA(Registers6502 registers, AddrMode6502 addressMode, Bus6502 memory)
        {
            return 0;
        }
        internal static byte TSX(Registers6502 registers, AddrMode6502 addressMode, Bus6502 memory)
        {
            return 0;
        }
        internal static byte TAY(Registers6502 registers, AddrMode6502 addressMode, Bus6502 memory)
        {
            return 0;
        }
        internal static byte TAX(Registers6502 registers, AddrMode6502 addressMode, Bus6502 memory)
        {
            return 0;
        }
        internal static byte STY(Registers6502 registers, AddrMode6502 addressMode, Bus6502 memory)
        {
            return 0;
        }
        internal static byte STX(Registers6502 registers, AddrMode6502 addressMode, Bus6502 memory)
        {
            return 0;
        }
        internal static byte STA(Registers6502 registers, AddrMode6502 addressMode, Bus6502 memory)
        {
            return 0;
        }
        internal static byte SEI(Registers6502 registers, AddrMode6502 addressMode, Bus6502 memory)
        {
            return 0;
        }
        internal static byte SEC(Registers6502 registers, AddrMode6502 addressMode, Bus6502 memory)
        {
            return 0;
        }
        internal static byte RTS(Registers6502 registers, AddrMode6502 addressMode, Bus6502 memory)
        {
            return 0;
        }
        internal static byte RTI(Registers6502 registers, AddrMode6502 addressMode, Bus6502 memory)
        {
            return 0;
        }
        internal static byte ROR(Registers6502 registers, AddrMode6502 addressMode, Bus6502 memory)
        {
            return 0;
        }
        internal static byte ROL(Registers6502 registers, AddrMode6502 addressMode, Bus6502 memory)
        {
            return 0;
        }
        internal static byte PLP(Registers6502 registers, AddrMode6502 addressMode, Bus6502 memory)
        {
            return 0;
        }
        internal static byte PLA(Registers6502 registers, AddrMode6502 addressMode, Bus6502 memory)
        {
            return 0;
        }
        internal static byte PHA(Registers6502 registers, AddrMode6502 addressMode, Bus6502 memory)
        {
            return 0;
        }
        internal static byte LSR(Registers6502 registers, AddrMode6502 addressMode, Bus6502 memory)
        {
            return 0;
        }
        internal static byte LDY(Registers6502 registers, AddrMode6502 addressMode, Bus6502 memory)
        {
            return 0;
        }
        internal static byte LDX(Registers6502 registers, AddrMode6502 addressMode, Bus6502 memory)
        {
            return 0;
        }
        internal static byte LDA(Registers6502 registers, AddrMode6502 addressMode, Bus6502 memory)
        {
            return 0;
        }

        internal static byte JSR(Registers6502 registers, AddrMode6502 addressMode, Bus6502 memory)
        {
            return 0;
        }
        internal static byte JMP(Registers6502 registers, AddrMode6502 addressMode, Bus6502 memory)
        {
            return 0;
        }
        internal static byte INY(Registers6502 registers, AddrMode6502 addressMode, Bus6502 memory)
        {
            return 0;
        }
        internal static byte EOR(Registers6502 registers, AddrMode6502 addressMode, Bus6502 memory)
        {
            return 0;
        }
        internal static byte DEY(Registers6502 registers, AddrMode6502 addressMode, Bus6502 memory)
        {
            return 0;
        }
        internal static byte DEX(Registers6502 registers, AddrMode6502 addressMode, Bus6502 memory)
        {
            return 0;
        }
        internal static byte CPY(Registers6502 registers, AddrMode6502 addressMode, Bus6502 memory)
        {
            return 0;
        }
        internal static byte CLV(Registers6502 registers, AddrMode6502 addressMode, Bus6502 memory)
        {
            return 0;
        }
        internal static byte CLI(Registers6502 registers, AddrMode6502 addressMode, Bus6502 memory)
        {
            return 0;
        }
        internal static byte CLD(Registers6502 registers, AddrMode6502 addressMode, Bus6502 memory)
        {
            return 0;
        }
        internal static byte CLC(Registers6502 registers, AddrMode6502 addressMode, Bus6502 memory)
        {
            return 0;
        }
        internal static byte BVS(Registers6502 registers, AddrMode6502 addressMode, Bus6502 memory)
        {
            return 0;
        }
        internal static byte BVC(Registers6502 registers, AddrMode6502 addressMode, Bus6502 memory)
        {
            return 0;
        }
        internal static byte BNE(Registers6502 registers, AddrMode6502 addressMode, Bus6502 memory)
        {
            return 0;
        }
        internal static byte BMI(Registers6502 registers, AddrMode6502 addressMode, Bus6502 memory)
        {
            return 0;
        }
        internal static byte BIT(Registers6502 registers, AddrMode6502 addressMode, Bus6502 memory)
        {
            return 0;
        }
        internal static byte BCS(Registers6502 registers, AddrMode6502 addressMode, Bus6502 memory)
        {
            return 0;
        }
    }
}
