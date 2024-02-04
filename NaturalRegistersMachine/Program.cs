using System.Diagnostics;
using System.Numerics;

internal class Program
{
    static readonly string[] AllowedCommands = { "help", "test", "run" };
    static readonly string[] AllowedExpressions = { "S", "T", "Z", "J" };
    static void Main(string[] args)
    {
        if (args.Length < 1 || !AllowedCommands.Contains(args[0].ToLower()))
        {
            Console.WriteLine("Unrecognized option. Please, use help command to see the list of supported commands");
            Environment.Exit(0);
        }

        HandleCommand(args);
    }

    private static void HandleCommand(string[] args)
    {
        string command = args[0].ToLower();
        if (command == "help")
        {
            Help();
            Environment.Exit(0);
        }

        if (args.Length < 2 || args.Length > 3)
        {
            Console.WriteLine("Wrong arguments count, please use 'help' command to view the usage");
            Environment.Exit(0);
        }
        var path = args[1];
        uint limit = 1000;
        if (args.Length == 3)
        {
            try
            {
                limit = ushort.Parse(args[2]);
            }
            catch (FormatException)
            {
                Console.WriteLine($"Invalid limit parameter: {args[2]}");
                Environment.Exit(0);
            }
        }
        if (!Path.Exists(path))
        {
            Console.WriteLine($"File\"{path}\" does not exist");
            Environment.Exit(0);
        }
        var shouldPrintDebug = command == "test";
        Console.WriteLine(Proceed(path, limit, shouldPrintDebug));

    }

    private static void Help()
    {
        Console.WriteLine(@"List of supported commands:

help - displays this message

run {filepath} {limit=1000} - prints the result of running code from file or -1 if limit is reached

test {filepath} {limit=1000} - do the same action as run command, but also prints all the registers at each step");
    }

    private static int Proceed(string filepath, uint limit, bool shouldPrintDebug)
    {
        var lines = File.ReadLines(filepath).ToList();
        var registers = (from el in lines.ElementAt(0).Split(" ") select (uint)BigInteger.Parse(el)).ToList();
        var index = 1;
        uint counter = 0;
        while (index < lines.Count && counter <= limit)
        {
            if (shouldPrintDebug)
            {
                Console.WriteLine(String.Join(" ", registers));
            }

            var line = lines.ElementAt(index).Trim();
            index = HandleLine(index, line, registers);
            counter++;
        }

        if (shouldPrintDebug)
        {
            Console.WriteLine(String.Join(" ", registers));
        }

        if (counter > limit)
        {
            return -1;
        }

        return (int)registers.ElementAt(0);
    }

    private static int HandleLine(int lineNumber, string line, IList<uint> registers)
    {
        var command = line.ElementAt(0).ToString().ToUpper();

        if (!AllowedExpressions.Contains(command))
        {
            System.Console.WriteLine($"Exiting due to unrecognized code on line {lineNumber}: {line}");
            Environment.Exit(0);
        }

        line = line.Replace(" ", "").Remove(0, 2);
        line = line.Remove(line.Length - 1, 1);

        var indexes = (from el in line.Split(',') select int.Parse(el)).ToList();

        switch (command)
        {
            case "Z":
                registers[indexes[0]] = 0;
                break;

            case "S":
                registers[indexes[0]] += 1;
                break;

            case "T":
                registers[indexes[1]] = registers[indexes[0]];
                break;

            case "J":
                if (registers[indexes[0]] == registers[indexes[1]])
                {
                    return indexes[2];
                }
                break;

        }

        return lineNumber + 1;
    }
}