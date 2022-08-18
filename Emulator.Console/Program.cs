using Emulator._6502.CPU;
using Emulator._6502.Devices;

namespace Emulator.App
{
    /// <summary>
    /// The program.
    /// </summary>
    internal static class Program
    {
        /// <summary>
        /// Main Entry Point
        /// </summary>
        /// <param name="args">Program Startup Arguments</param>
        /// <returns>Exit Code</returns>
        public static int Main(params string[] args)
        {
            var ram = new Ram6502();

            var bus = new Bus6502();
            bus.RegisterDevice(new AddressRange6502() { StartAddress = 0x0000, EndAddress = 0xFFFF }, ram);

            byte value = 0x78;
            bus.Write(0xBEEF, value);

            var res = bus.ReadByte(0xBEEF);

            if (res != value)
            {
                Console.WriteLine("Failed to read/write byte");
                Console.ReadKey();
                return 1;
            }

            ushort value2 = 0xBEEF;
            bus.Write(0xDEAD, value2);

            var res2 = bus.ReadWord(0xDEAD);
            if (res2 != value2)
            {
                Console.WriteLine("Failed to read/write word");
                Console.ReadKey();
                return 1;
            }
            string path = Path.Combine("Roms", "MaxValueTest.bin");
            var data = File.ReadAllBytes(path);
            ram.LoadData(data);
            var cpu = new Cpu6502(ref bus);
            cpu.Reset();
            var dis = cpu.DecompileOpcodes(0, 19);
            foreach (var i in dis)
            {
                Console.Write(i);
            }

            cpu.Reset();
            Console.WriteLine(cpu.GetDebuggerDisplay());
            cpu.Execute();
            Console.WriteLine(cpu.GetDebuggerDisplay());

            Console.ReadKey();



            return 0;
        }
    }
}
