using CommandLine;

using Emulator._6502;

namespace Emulator.App
{
    /// <summary>
    /// The prog_ram.
    /// </summary>
    internal static class Program
    {
        private static readonly Cpu6502 _cpu = new();
        private static readonly Ram6502 _ram = new();
        private static readonly RomManager _folderMon = new();
        public struct CommandOptions
        {
            [Option('s', "Source", Default = "", FlagCounter = false, HelpText = "File path to source bin", Hidden = false, Required = false)]
            public FileInfo File { get; set; }
            [Option('d', "Disassemble", Default = true, FlagCounter = false, HelpText = "save disassembly to same directory as source", Hidden = false, Required = false)]
            public bool Disassemble { get; set; }
        }

        public static int Main(string[] args)
        {
            _cpu.RegisterDevice(_ram);
            var res = Parser.Default.ParseArguments<CommandOptions>(args).MapResult(ParsedCommand, UnParsedCommand);
            Console.ResetColor();
            Console.Clear();
            return res;
        }

        private static int ParsedCommand(CommandOptions a)
        {
            if (a.File?.Exists != true)
            {
                return UnParsedCommand(null);
            }

            _ram.Clear();
            ushort bytes = _ram.LoadData(a.File.FullName);
            _cpu.Reset();
            var dis = _cpu.DecompileAddrRange(0, bytes);
            _cpu.Reset();
            _cpu.Step((ushort)dis.Length);

            Console.WriteLine();
            if (!Directory.Exists(_folderMon.DisassemblyDir))
            {
                Directory.CreateDirectory(_folderMon.DisassemblyDir);
            }

            if (a.Disassemble)
            {
                var path = $"{_folderMon.DisassemblyDir}\\{a.File.Name}_Disassembly.txt";

                if (!Path.Exists(path))
                {
                    File.Delete(path);
                }
                using StreamWriter text = new(path);
                for (int i = 0; i < dis.Length; i++)
                {
                    text.WriteLineAsync(dis[i]);
                }
                text.Close();
            }
            return 0;
        }

        private static int UnParsedCommand(IEnumerable<Error>? errors = null)
        {
            bool exit = false;
            do
            {
                Console.BackgroundColor = ConsoleColor.Cyan;
                Console.ForegroundColor = ConsoleColor.White;
                int result;
                do
                {
                    Console.Clear();
                    Console.WriteLine("Please Select a Rom Image to run:");
                    Console.Write(_folderMon.GetMenu());
                    Console.Write($"Enter Value from 0 to {_folderMon.ExitIndex}: ");

                    if (int.TryParse(Console.ReadLine() ?? "-1", out result))
                    {
                        if (result < 0 || result > _folderMon.ExitIndex)
                        {
                            continue;
                        }
                        break;
                    }
                }
                while (true);

                Console.Clear();
                if (result == _folderMon.ExitIndex)
                {
                    exit = true;
                    continue;
                }
                var s = _folderMon.GetFileByIndex(result);
                ArgumentNullException.ThrowIfNull(s, nameof(result));
                (string Name, string FullPath) = s;

                Console.BackgroundColor = ConsoleColor.DarkBlue;
                Console.ForegroundColor = ConsoleColor.White;
                Console.Clear();

                _ram.Clear();
                ushort bytes = _ram.LoadData(FullPath);
                _cpu.Reset();
                Console.WriteLine("Number of bytes read:{0}", bytes);
                var dis = _cpu.DecompileAddrRange(0, bytes);
                int c = dis.Length;
                for (int i = 0; i < c; i++)
                {
                    Console.WriteLine(dis[i]);
                }
                _cpu.Reset();
                Console.WriteLine(_cpu.GetDebuggerDisplay());

                _cpu.Step((ushort)dis.Length);

                Console.WriteLine();
                Console.Write("Would you like to save the disassembly?(y/n): ");

                var (Left, Top) = Console.GetCursorPosition();

                ConsoleKeyInfo keyinfo;
                if (!Directory.Exists(_folderMon.DisassemblyDir))
                {
                    Directory.CreateDirectory(_folderMon.DisassemblyDir);
                }
                do
                {
                    keyinfo = Console.ReadKey();

                    if (keyinfo.Key == ConsoleKey.N)
                    {
                        break;
                    }
                    else if (keyinfo.Key == ConsoleKey.Y)
                    {
                        var path = $"{_folderMon.DisassemblyDir}\\{Name}.txt";

                        if (!Path.Exists(path))
                        {
                            File.Delete(path);
                        }
                        using StreamWriter text = new(path);
                        for (int i = 0; i < dis.Length; i++)
                        {
                            text.WriteLineAsync(dis[i]);
                        }
                        text.Close();
                    }
                    else
                    {
                        Console.SetCursorPosition(Left, Top);
                    }
                }
                while (true);

                Console.WriteLine(_cpu.GetDebuggerDisplay());
                Console.Clear();

            }
            while (!exit);
            return 0;
        }
    }
}