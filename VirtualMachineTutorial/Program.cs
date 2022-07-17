using Emulator._6502.CPU;
using Emulator._6502.Devices;

namespace Emulator
{
    internal static class Program
    {
        public static int Main(params string[] args)
        {
            var instruct = new InstructionSet6502();
            var reg = new Registers6502();
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

            instruct[0x00].Execute(reg, bus);


            Console.WriteLine("Successfully to read/write byte & word");
            Console.ReadKey();
            return 0;
        }
    }
}
