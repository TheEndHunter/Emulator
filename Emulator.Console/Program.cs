using Emulator._6502;

using Microsoft.Win32.SafeHandles;

using System.ComponentModel.Design;

namespace Emulator.App
{
    /// <summary>
    /// The program.
    /// </summary>
    internal static class Program
    {
        private static CancellationTokenSource _TokenSource;
        public static async Task<int> Main()
        {
            _TokenSource = new CancellationTokenSource();
            Cpu6502 cpu = new();

            Ram6502 ram = new();

            cpu.RegisterDevice(ram);

            var folderMonitor = new RomManager();

            bool exit = false;
            do
            {
                Console.BackgroundColor = ConsoleColor.Cyan;
                Console.ForegroundColor = ConsoleColor.White;

                Console.Clear();
                Console.WriteLine("Please Select a Rom Image to run:");
                Console.Write(folderMonitor.GetMenu());
                Console.Write($"Enter Value from 0 to {folderMonitor.ExitIndex}: ");
                int result;

                do
                {
                    Console.Clear();
                    Console.WriteLine("Please Select a Rom Image to run:");
                    Console.Write(folderMonitor.GetMenu());
                    Console.Write($"Enter Value from 0 to {folderMonitor.ExitIndex}: ");

                    if (int.TryParse(Console.ReadLine() ?? "-1", out result))
                    {
                        if (result < 0 || result > folderMonitor.ExitIndex)
                        {
                            continue;
                        }
                        break;
                    }
                }
                while (true);

                Console.Clear();
                if (result == folderMonitor.ExitIndex)
                {
                    exit = true;
                    continue;
                }

                (string Name, string FullPath) = folderMonitor.GetFileByIndex(result) ?? throw new ArgumentException(nameof(result), "Index not valid file");

                Console.BackgroundColor = ConsoleColor.DarkBlue;
                Console.ForegroundColor = ConsoleColor.White;
                Console.Clear();

                ushort bytes = ram.LoadData(FullPath);
                cpu.Reset();
                Console.WriteLine("Number of bytes read:{0}", bytes);
                var dis = cpu.DecompileAddrRange(0, bytes);
                foreach (var i in dis)
                {
                    Console.Write("{0}{1}", i, Environment.NewLine);
                }
                cpu.Reset();
                Console.WriteLine(cpu.GetDebuggerDisplay());
                cpu.Step((ushort)dis.Count);

                Console.WriteLine(string.Empty);
                Console.Write("Would you like to save the disassembly?(y/n): ");

                var t = Console.GetCursorPosition();

                ConsoleKeyInfo keyinfo;
                bool validKey = true;
                do
                {
                    keyinfo = Console.ReadKey();

                    if (keyinfo.Key == ConsoleKey.N)
                    {
                        break;
                    }
                    else if (keyinfo.Key == ConsoleKey.Y)
                    {
                        var path = Path.Combine(folderMonitor.DisassemblyDir, Name + ".txt");
                        if (!Directory.Exists(folderMonitor.DisassemblyDir))
                        {
                            Directory.CreateDirectory(folderMonitor.DisassemblyDir);
                        }

                        if (!Path.Exists(path))
                        {
                            File.AppendAllLines(path, dis);
                            break;
                        }
                        else
                        {
                            File.WriteAllLines(path, dis);
                            break;
                        }
                    }
                    else
                    {
                        Console.SetCursorPosition(t.Left, t.Top);
                    }
                }
                while (true);

                Console.WriteLine(cpu.GetDebuggerDisplay());
                Console.Clear();

            }
            while (!exit);

            Console.ResetColor();
            Console.Clear();

            return 0;

        }
    }
}