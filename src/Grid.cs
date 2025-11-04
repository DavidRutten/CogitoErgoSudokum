
using System.Data.Common;
using System.IO.Pipes;
using System.Text;

namespace CogitoErgoSudokum
{
  /// <summary>
  /// A 9x9 Sudoku grid filled with <see cref="Digit"/> instances.
  /// </summary>
  public class Grid
  {
    private static readonly Digit[] PluralRow =
    [
      Digit.FullyPlural, Digit.FullyPlural, Digit.FullyPlural,
      Digit.FullyPlural, Digit.FullyPlural, Digit.FullyPlural,
      Digit.FullyPlural, Digit.FullyPlural, Digit.FullyPlural
    ];

    private readonly Digit[][] _rows =
    [
      new Digit[9],
      new Digit[9],
      new Digit[9],
      new Digit[9],
      new Digit[9],
      new Digit[9],
      new Digit[9],
      new Digit[9],
      new Digit[9],
    ];

    /// <summary>
    /// Create a new grid with complete digit plurality.
    /// </summary>
    public Grid() : this(PluralRow, PluralRow, PluralRow,
                         PluralRow, PluralRow, PluralRow,
                         PluralRow, PluralRow, PluralRow)
    { }
    private Grid(params Digit[][] rows)
    {
      _rows = rows;
    }

    /// <summary>
    /// Gets the digit in the given cell.
    /// </summary>
    public Digit this[int row, int column]
    {
      get { return _rows[row - 1][column - 1]; }
    }

    private Grid WithRow(int rowIdentifier, Digit[] digits)
    {
      var array = (Digit[][])_rows.Clone();
      array[rowIdentifier] = digits;
      return new Grid(array);
    }

    /// <summary>
    /// Create a new grid with the given digit in the given location.
    /// </summary>
    public Grid WithDigit(int rowIndentifier, int colIdentifier, Digit digit)
    {
      var r = (Digit[])_rows[rowIndentifier - 1].Clone();
      r[colIdentifier - 1] = digit;
      return WithRow(rowIndentifier, r);
    }
    /// <summary>
    /// Create a new grid with the given digits in the given locations.
    /// </summary>
    public Grid WithDigits(params (int row, int column, Digit digit)[] digits)
    {
      var mask = new bool[9];
      for (int i = 0; i < digits.Length; i++)
        mask[digits[i].row - 1] = true;

      // We only have to clone those rows containing new digits.
      var array = (Digit[][])_rows.Clone();
      for (int i = 0; i < array.Length; i++)
        array[i] = mask[i] ? (Digit[])array[i].Clone() : array[i];

      foreach (var (r, c, d) in digits)
        array[r - 1][c - 1] = d;

      return new Grid(array);
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
  }
}