using System.Runtime.InteropServices;

namespace CogitoErgoSudokum
{
  class Program
  {
    static void Main(string[] args)
    {
      Console.Clear();
      Console.WriteLine("Cogito Ergo Sudokum, regulae Sudoku communes applicantur.");
      Console.WriteLine($"Running on {GetOperatingSystem()} & .NET {Environment.Version}");

      if (args.Length > 0)
      {
        Console.WriteLine($"\nArguments passed: {string.Join(", ", args)}");
      }

      // Key menu
      // N: new puzzle
      // O: open puzzle
      // S: save puzzle (as)
      // R: edit rulesets
      // F: find solution(s)
      // U: undo?
      // Q: quit

      var grid = new Grid();
      grid = grid.WithDigits(
        (1, 1, Digit.CreateAssigned(3)),
        (1, 4, Digit.CreateAssigned(4)),
        (2, 3, Digit.CreateAssigned(9)),
        (4, 2, Digit.CreateAssigned(7)),
        (4, 9, Digit.CreateAssigned(5)),
        (5, 5, Digit.CreateAssigned(1)),
        (7, 6, Digit.CreateAssigned(2)),
        (8, 6, Digit.CreateAssigned(8))
      );

      Console.Write(grid.ToString());

      Console.WriteLine("Press any key to stop.");
      Console.ReadKey();
    }

    static string GetOperatingSystem()
    {
      if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        return "OSX";
      if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        return "Windows";
      if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        return "Linux";
      return "Unknown";
    }
  }
}