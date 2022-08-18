using System.Diagnostics;
using System.Text;

namespace Emulator._6502.CPU
{
    [DebuggerDisplay($"{{{nameof(GetDebuggerDisplay)}(),nq}}")]
    public struct Registers6502
    {
        public Registers6502()
        {
            A = 0;
            X = 0;
            Y = 0;
            STKP = 0;
            PC = 0;
            Status = Status6502.None;
        }
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
            StringBuilder sb = new();
            sb.AppendLine($"\tA: {A:X2}");
            sb.AppendLine($"\tX: {X:X2}");
            sb.AppendLine($"\tY: {Y:X2}");
            sb.AppendLine($"\tSTKP: {STKP:X4}");
            sb.AppendLine($"\tPC: {PC:X4}");

            return sb.ToString();
        }
    }

}
