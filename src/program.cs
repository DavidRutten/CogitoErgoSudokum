using System;
using System.Runtime.InteropServices;

namespace MyApp
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

      // Create a SudokuGrid class. Each cell has a bitmask showing which digits are stil available
      // and whether or not the singular digit has been manually set.

      // how do I signal plurality per cell?
      // Don't seem to be good dot patterns for 3x3.
      /*
          ┏━━━┯━━━┯━━━┳━━━┯━━━┯━━━┳━━━┯━━━┯━━━┓
          ┃ 1 │ 2 │ 3 ┃ 4 │ 5 │ 6 ┃ 7 │ 8 │ 9 ┃
          ┠───┼───┼───╂───┼───┼───╂───┼───┼───┨
          ┃ 1 │ 2 │ 3 ┃ 4 │ 5 │ 6 ┃ 7 │ 8 │ 9 ┃
          ┠───┼───┼───╂───┼───┼───╂───┼───┼───┨
          ┃ 1 │ 2 │ 3 ┃ 4 │ 5 │ 6 ┃ 7 │ 8 │ 9 ┃
          ┣━━━┿━━━┿━━━╋━━━┿━━━┿━━━╋━━━┿━━━┿━━━┫
          ┃ 1 │ 2 │ 3 ┃ 4 │ 5 │ 6 ┃ 7 │ 8 │ 9 ┃
          ┠───┼───┼───╂───┼───┼───╂───┼───┼───┨
          ┃ 1 │ 2 │ 3 ┃ 4 │ 5 │ 6 ┃ 7 │ 8 │ 9 ┃
          ┠───┼───┼───╂───┼───┼───╂───┼───┼───┨
          ┃ 1 │ 2 │ 3 ┃ 4 │ 5 │ 6 ┃ 7 │ 8 │ 9 ┃
          ┣━━━┿━━━┿━━━╋━━━┿━━━┿━━━╋━━━┿━━━┿━━━┫
          ┃ 1 │ 2 │ 3 ┃ 4 │ 5 │ 6 ┃ 7 │ 8 │ 9 ┃
          ┠───┼───┼───╂───┼───┼───╂───┼───┼───┨
          ┃ 1 │ 2 │ 3 ┃ 4 │ 5 │ 6 ┃ 7 │ 8 │ 9 ┃
          ┠───┼───┼───╂───┼───┼───╂───┼───┼───┨
          ┃ 1 │ 2 │ 3 ┃ 4 │ 5 │ 6 ┃ 7 │ 8 │ 9 ┃
          ┗━━━┷━━━┷━━━┻━━━┷━━━┷━━━┻━━━┷━━━┷━━━┛

      */
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