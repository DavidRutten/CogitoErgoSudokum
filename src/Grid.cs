using System.Text;
using System.Collections.Immutable;

using Array = System.Collections.Immutable.ImmutableArray<CogitoErgoSudokum.Cell>;
using System.Linq.Expressions;
using System.Data;

namespace CogitoErgoSudokum
{
  /// <summary>
  /// A 9x9 Sudoku grid filled with <see cref="Digit"/> instances.
  /// </summary>
  public class Grid
  {
    private readonly Array _grid;

    /// <summary>
    /// Create a new grid with complete digit plurality.
    /// </summary>
    public Grid() : this(InitialGrid()) { }
    private static Array InitialGrid()
    {
      var array = ImmutableArray.CreateBuilder<Cell>(9 * 9);
      for (int r = 1; r <= 9; r++)
        for (int c = 1; c <= 9; c++)
          array.Add(new Cell(r, c, Digit.FullyPlural));
      return array.DrainToImmutable();
    }
    private Grid(Array grid)
    {
      _grid = grid;
    }

    /// <summary>
    /// Gets the digit in the given cell.
    /// </summary>
    public Cell this[int row, int column]
    {
      get { return _grid[ToLinear(row, column)]; }
    }

    /// <summary>
    /// Create a new grid with the given digit in the given location.
    /// </summary>
    public Grid WithCell(Cell cell)
    {
      var index = ToLinear(cell.Row, cell.Col);
      return new Grid(_grid.SetItem(index, cell));
    }
    /// <summary>
    /// Create a new grid with the given digit in the given location.
    /// </summary>
    public Grid WithCell(int row, int column, Digit digit)
    {
      var cell = new Cell(row, column, digit);
      return WithCell(cell);
    }

    /// <summary>
    /// Create a new grid with the given digits in the given locations.
    /// </summary>
    public Grid WithCells(params Cell[] cells)
    {
      if (cells is null || cells.Length == 0)
        return this;

      if (cells.Length == 1)
        return WithCell(cells[0]);

      var grid = _grid.ToBuilder();
      foreach (var cell in cells)
      {
        var index = ToLinear(cell.Row, cell.Col);
        grid[index] = cell;
      }
      return new Grid(grid.DrainToImmutable());
    }

    /// <summary>
    /// Remove the values in all plural cells due to hard digits in resolved cells.
    /// </summary>
    public Grid RemoveValuesDueToSingles()
    {

      var grid = _grid.ToBuilder();
      foreach (var cell in _grid)
      {
        var value = cell.Digit.Value;
        if (value > 0)
          for (int i = 0; i < grid.Count; i++)
          {
            var other = grid[i];
            if (cell.Row != other.Row || cell.Col != other.Col)
          }
      }
      return new Grid(grid.DrainToImmutable());
    }

    public override string ToString()
    {
      return ToString5();
    }
    /// <summary>
    /// Create a string representation of this grid with a column
    /// width of 1 character.
    /// </summary>
    public string ToString1()
    {
      throw new NotImplementedException();
    }
    /// <summary>
    /// Create a string representation of this grid with a column
    /// width of 3 characters.
    /// </summary>
    public string ToString3()
    {
      var upper = "┏━━━┯━━━┯━━━┳━━━┯━━━┯━━━┳━━━┯━━━┯━━━┓";
      var inner = "┣━━━┿━━━┿━━━╋━━━┿━━━┿━━━╋━━━┿━━━┿━━━┫";
      var intra = "┠───┼───┼───╂───┼───┼───╂───┼───┼───┨";
      var lower = "┗━━━┷━━━┷━━━┻━━━┷━━━┷━━━┻━━━┷━━━┷━━━┛";

      var sb = new StringBuilder();
      void AppendDigit(Digit d)
      {
        if (d.Plurality == 0)
          sb.Append(" ❌ ");
        else if (d.Plurality == 1)
          sb.Append(" " + d + " ");
        else
          sb.Append("{" + d.Plurality + "}");
      }
      void AppendRow(int index)
      {
        sb.Append('┃');
        {
          AppendDigit(_rows[index][0]);
          sb.Append('│');
          AppendDigit(_rows[index][1]);
          sb.Append('│');
          AppendDigit(_rows[index][2]);
        }
        sb.Append('┃');
        {
          AppendDigit(_rows[index][3]);
          sb.Append('│');
          AppendDigit(_rows[index][4]);
          sb.Append('│');
          AppendDigit(_rows[index][5]);
        }
        sb.Append('┃');
        {
          AppendDigit(_rows[index][6]);
          sb.Append('│');
          AppendDigit(_rows[index][7]);
          sb.Append('│');
          AppendDigit(_rows[index][8]);
        }
        sb.Append('┃');
        sb.AppendLine();
      }

      sb.AppendLine(upper);
      AppendRow(0);
      sb.AppendLine(intra);
      AppendRow(1);
      sb.AppendLine(intra);
      AppendRow(2);
      sb.AppendLine(inner);
      AppendRow(3);
      sb.AppendLine(intra);
      AppendRow(4);
      sb.AppendLine(intra);
      AppendRow(5);
      sb.AppendLine(inner);
      AppendRow(6);
      sb.AppendLine(intra);
      AppendRow(7);
      sb.AppendLine(intra);
      AppendRow(8);
      sb.AppendLine(lower);

      return sb.ToString();

      /*
      ┏━━━┯━━━┯━━━┳━━━┯━━━┯━━━┳━━━┯━━━┯━━━┓
      ┃ 1 │ 2 │ 3 ┃ 4 │ 5 │ 6 ┃ 7 │ 8 │ 9 ┃
      ┠───┼───┼───╂───┼───┼───╂───┼───┼───┨
      ┃156│2 6│   ┃(4)│[9]│{4}┃ ? │   │   ┃
      ┠───┼───┼───╂───┼───┼───╂───┼───┼───┨
      ┃   │   │   ┃   │   │   ┃   │   │   ┃
      ┣━━━┿━━━┿━━━╋━━━┿━━━┿━━━╋━━━┿━━━┿━━━┫
      ┃   │   │   ┃   │   │   ┃   │   │   ┃
      ┠───┼───┼───╂───┼───┼───╂───┼───┼───┨
      ┃   │   │   ┃   │   │   ┃   │   │   ┃
      ┠───┼───┼───╂───┼───┼───╂───┼───┼───┨
      ┃   │   │   ┃   │   │   ┃   │   │   ┃
      ┣━━━┿━━━┿━━━╋━━━┿━━━┿━━━╋━━━┿━━━┿━━━┫
      ┃   │   │   ┃   │   │   ┃   │   │   ┃
      ┠───┼───┼───╂───┼───┼───╂───┼───┼───┨
      ┃   │   │   ┃   │   │   ┃   │   │   ┃
      ┠───┼───┼───╂───┼───┼───╂───┼───┼───┨
      ┃   │   │   ┃   │   │   ┃   │   │   ┃
      ┗━━━┷━━━┷━━━┻━━━┷━━━┷━━━┻━━━┷━━━┷━━━┛
      */
    }
    /// <summary>
    /// Create a string representation of this grid with a column
    /// width of 5 characters.
    /// </summary>
    public string ToString5()
    {
      var upper = "┏━━━━━┯━━━━━┯━━━━━┳━━━━━┯━━━━━┯━━━━━┳━━━━━┯━━━━━┯━━━━━┓";
      var inner = "┣━━━━━┿━━━━━┿━━━━━╋━━━━━┿━━━━━┿━━━━━╋━━━━━┿━━━━━┿━━━━━┫";
      var intra = "┠─────┼─────┼─────╂─────┼─────┼─────╂─────┼─────┼─────┨";
      var lower = "┗━━━━━┷━━━━━┷━━━━━┻━━━━━┷━━━━━┷━━━━━┻━━━━━┷━━━━━┷━━━━━┛";

      var sb = new StringBuilder();
      void AppendDigit(Digit d)
      {
        switch (d.Plurality)
        {
          case 0: sb.Append("  ❌  "); break;
          case 1: sb.Append("  " + d + "  "); break;
          case 2: sb.Append(" " + string.Join(string.Empty, d.Values) + "  "); break;
          case 3: sb.Append(" " + string.Join(string.Empty, d.Values) + " "); break;
          case 4: sb.Append("" + string.Join(string.Empty, d.Values) + " "); break;
          case 5: sb.Append("" + string.Join(string.Empty, d.Values) + ""); break;
          default: sb.Append(" {" + d.Plurality + "} "); break;
        }
      }
      void AppendRow(int index)
      {
        sb.Append('┃');
        {
          AppendDigit(_rows[index][0]);
          sb.Append('│');
          AppendDigit(_rows[index][1]);
          sb.Append('│');
          AppendDigit(_rows[index][2]);
        }
        sb.Append('┃');
        {
          AppendDigit(_rows[index][3]);
          sb.Append('│');
          AppendDigit(_rows[index][4]);
          sb.Append('│');
          AppendDigit(_rows[index][5]);
        }
        sb.Append('┃');
        {
          AppendDigit(_rows[index][6]);
          sb.Append('│');
          AppendDigit(_rows[index][7]);
          sb.Append('│');
          AppendDigit(_rows[index][8]);
        }
        sb.Append('┃');
        sb.AppendLine();
      }

      sb.AppendLine(upper);
      AppendRow(0);
      sb.AppendLine(intra);
      AppendRow(1);
      sb.AppendLine(intra);
      AppendRow(2);
      sb.AppendLine(inner);
      AppendRow(3);
      sb.AppendLine(intra);
      AppendRow(4);
      sb.AppendLine(intra);
      AppendRow(5);
      sb.AppendLine(inner);
      AppendRow(6);
      sb.AppendLine(intra);
      AppendRow(7);
      sb.AppendLine(intra);
      AppendRow(8);
      sb.AppendLine(lower);

      return sb.ToString();

      /*
      ┏━━━━━┯━━━━━┯━━━━━┳━━━━━┯━━━━━┯━━━━━┳━━━━━┯━━━━━┯━━━━━┓
      ┃  1  │  2  │  3  ┃  4  │  5  │  6  ┃  7  │  8  │  9  ┃
      ┠─────┼─────┼─────╂─────┼─────┼─────╂─────┼─────┼─────┨
      ┃  1  │  2  │  3  ┃  4  │  5  │  6  ┃  7  │  8  │  9  ┃
      ┠─────┼─────┼─────╂─────┼─────┼─────╂─────┼─────┼─────┨
      ┃  1  │  2  │  3  ┃  4  │  5  │  6  ┃  7  │  8  │  9  ┃
      ┣━━━━━┿━━━━━┿━━━━━╋━━━━━┿━━━━━┿━━━━━╋━━━━━┿━━━━━┿━━━━━┫
      ┃  1  │  2  │  3  ┃  4  │  5  │  6  ┃  7  │  8  │  9  ┃
      ┠─────┼─────┼─────╂─────┼─────┼─────╂─────┼─────┼─────┨
      ┃  1  │  2  │  3  ┃  4  │  5  │  6  ┃  7  │  8  │  9  ┃
      ┠─────┼─────┼─────╂─────┼─────┼─────╂─────┼─────┼─────┨
      ┃  1  │  2  │  3  ┃  4  │  5  │  6  ┃  7  │  8  │  9  ┃
      ┣━━━━━┿━━━━━┿━━━━━╋━━━━━┿━━━━━┿━━━━━╋━━━━━┿━━━━━┿━━━━━┫
      ┃  1  │  2  │  3  ┃  4  │  5  │  6  ┃  7  │  8  │  9  ┃
      ┠─────┼─────┼─────╂─────┼─────┼─────╂─────┼─────┼─────┨
      ┃  1  │  2  │  3  ┃  4  │  5  │  6  ┃  7  │  8  │  9  ┃
      ┠─────┼─────┼─────╂─────┼─────┼─────╂─────┼─────┼─────┨
      ┃  1  │  2  │  3  ┃  4  │  5  │  6  ┃  7  │  8  │  9  ┃
      ┗━━━━━┷━━━━━┷━━━━━┻━━━━━┷━━━━━┷━━━━━┻━━━━━┷━━━━━┷━━━━━┛
      */
    }

    /// <summary>
    /// Dump the grid to the console.
    /// </summary>
    public void ToConsole5()
    {
      var upper = "┏━━━━━┯━━━━━┯━━━━━┳━━━━━┯━━━━━┯━━━━━┳━━━━━┯━━━━━┯━━━━━┓";
      var inner = "┣━━━━━┿━━━━━┿━━━━━╋━━━━━┿━━━━━┿━━━━━╋━━━━━┿━━━━━┿━━━━━┫";
      var intra = "┠─────┼─────┼─────╂─────┼─────┼─────╂─────┼─────┼─────┨";
      var lower = "┗━━━━━┷━━━━━┷━━━━━┻━━━━━┷━━━━━┷━━━━━┻━━━━━┷━━━━━┷━━━━━┛";

      void WriteRow(int index)
      {
        for (int i = 0; i < 9; i++)
        {
          if (i % 3 == 0)
            Console.Write('┃');
          else
            Console.Write('│');

          Console.Write(_rows[index][i].Format(5));
        }
        Console.WriteLine('┃');
      }

      Console.WriteLine(upper);
      WriteRow(0);
      Console.WriteLine(intra);
      WriteRow(1);
      Console.WriteLine(intra);
      WriteRow(2);
      Console.WriteLine(inner);
      WriteRow(3);
      Console.WriteLine(intra);
      WriteRow(4);
      Console.WriteLine(intra);
      WriteRow(5);
      Console.WriteLine(inner);
      WriteRow(6);
      Console.WriteLine(intra);
      WriteRow(7);
      Console.WriteLine(intra);
      WriteRow(8);
      Console.WriteLine(lower);

      /*
      ┏━━━━━┯━━━━━┯━━━━━┳━━━━━┯━━━━━┯━━━━━┳━━━━━┯━━━━━┯━━━━━┓
      ┃  1  │  2  │  3  ┃  4  │  5  │  6  ┃  7  │  8  │  9  ┃
      ┠─────┼─────┼─────╂─────┼─────┼─────╂─────┼─────┼─────┨
      ┃  1  │  2  │  3  ┃  4  │  5  │  6  ┃  7  │  8  │  9  ┃
      ┠─────┼─────┼─────╂─────┼─────┼─────╂─────┼─────┼─────┨
      ┃  1  │  2  │  3  ┃  4  │  5  │  6  ┃  7  │  8  │  9  ┃
      ┣━━━━━┿━━━━━┿━━━━━╋━━━━━┿━━━━━┿━━━━━╋━━━━━┿━━━━━┿━━━━━┫
      ┃  1  │  2  │  3  ┃  4  │  5  │  6  ┃  7  │  8  │  9  ┃
      ┠─────┼─────┼─────╂─────┼─────┼─────╂─────┼─────┼─────┨
      ┃  1  │  2  │  3  ┃  4  │  5  │  6  ┃  7  │  8  │  9  ┃
      ┠─────┼─────┼─────╂─────┼─────┼─────╂─────┼─────┼─────┨
      ┃  1  │  2  │  3  ┃  4  │  5  │  6  ┃  7  │  8  │  9  ┃
      ┣━━━━━┿━━━━━┿━━━━━╋━━━━━┿━━━━━┿━━━━━╋━━━━━┿━━━━━┿━━━━━┫
      ┃  1  │  2  │  3  ┃  4  │  5  │  6  ┃  7  │  8  │  9  ┃
      ┠─────┼─────┼─────╂─────┼─────┼─────╂─────┼─────┼─────┨
      ┃  1  │  2  │  3  ┃  4  │  5  │  6  ┃  7  │  8  │  9  ┃
      ┠─────┼─────┼─────╂─────┼─────┼─────╂─────┼─────┼─────┨
      ┃  1  │  2  │  3  ┃  4  │  5  │  6  ┃  7  │  8  │  9  ┃
      ┗━━━━━┷━━━━━┷━━━━━┻━━━━━┷━━━━━┷━━━━━┻━━━━━┷━━━━━┷━━━━━┛
      */
    }
  }
}