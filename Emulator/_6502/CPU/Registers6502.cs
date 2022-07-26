using System.Diagnostics;

namespace Emulator._6502.CPU
{
    [DebuggerDisplay($"{{{nameof(GetDebuggerDisplay)}(),nq}}")]
    public struct Registers6502
    {
        public byte A { get; set; }
        public byte X { get; set; }
        public byte Y { get; set; }
        public byte STKP { get; set; }
        public ushort PC { get; set; }
        public Status6502 Status { get; set; }

        public void SetFlag(Status6502 f, bool v)
        {
            if (v)
            {
                Status |= f;
            }
            else
            {
                Status &= ~f;
            }

        }

        public bool GetFlag(Status6502 f)
        {
            return (Status & f) > 0;
        }

        public string GetDebuggerDisplay()
        {
            return $"Registers: A:0x{A.ToString("X2")}, X:0x{X.ToString("X2")}, Y:0x{Y.ToString("X2")}, STKP:0x{STKP.ToString("X2")}, PC:0x{PC.ToString("X4")}, Status:{Status}";
        }
    }

}
