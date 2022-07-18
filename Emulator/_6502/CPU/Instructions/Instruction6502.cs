using Emulator._6502.Devices;

using System.Diagnostics;

namespace Emulator._6502.CPU.Instructions
{
    /// <summary>
    /// 6502 instruction
    /// </summary>
    [DebuggerDisplay($"{{{nameof(GetDebuggerDisplay)}(),nq}}")]
    public abstract class Instruction6502
    {
        /// <summary>
        /// Default Constructor
        /// </summary>
        /// <param name="name"></param>
        /// <param name="addressMode"></param>
        /// <param name="flags"></param>
        protected Instruction6502(string name, AddrMode6502 addressMode, Status6502 flags)
        {
            Name = name;
            AddressMode = addressMode;
            Flags = flags;
        }
        public string Name { get; }
        public AddrMode6502 AddressMode { get; }
        public Status6502 Flags { get; }
        public abstract byte Execute(Registers6502 registers, Bus6502 bus);

        public string GetDebuggerDisplay()
        {
            return $"{Name}[A:{AddressMode}][F:{Flags}]";
        }
    }
}
