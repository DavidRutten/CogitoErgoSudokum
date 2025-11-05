
namespace CogitoErgoSudokum
{
  public readonly struct Cell
  {
    public Cell(int row, int column) : this(row, column, Digit.FullyPlural) { }
    public Cell(int row, int column, Digit digit) : this(row, column, BoxIndex(row, column), LinearIndex(row, column), digit) { }
    private Cell(int row, int column, int box, int lin, Digit contents)
    {
      Row = row;
      Col = column;
      Box = box;
      Lin = lin;
      Digit = contents;
    }

    /// <summary>
    /// Given row and column indices, compute the box index.
    /// </summary>
    /// <exception cref="IndexOutOfRangeException"></exception>
    public static int BoxIndex(int row, int col)
    {
      var r = (row - 1) / 3;
      var c = (col - 1) / 3;
      if (r == 0)
      {
        if (c == 0) return 1;
        if (c == 1) return 2;
        if (c == 2) return 3;
      }
      else if (r == 1)
      {
        if (c == 0) return 4;
        if (c == 1) return 5;
        if (c == 2) return 6;
      }
      else if (r == 2)
      {
        if (c == 0) return 7;
        if (c == 1) return 8;
        if (c == 2) return 9;
      }

      throw new IndexOutOfRangeException($"Invalid sudoky grid coordinate [{row},{col}].");
    }
    /// <summary>
    /// Given row and column indices, compute the linear index.
    /// </summary>
    /// <exception cref="IndexOutOfRangeException"></exception>
    public static int LinearIndex(int row, int col)
    {
      row -= 1;
      col -= 1;
      return 9 * row + col;
    }

    /// <summary>
    /// Gets the row index (as an integer from 1 to 9 inclusive).
    /// </summary>
    public int Row { get; }
    /// <summary>
    /// Gets the column index (as an integer from 1 to 9 inclusive).
    /// </summary>
    public int Col { get; }
    /// <summary>
    /// Gets the box index (as an integer from 1 to 9 inclusive).
    /// </summary>
    public int Box { get; }
    /// <summary>
    /// Gets the linear index within the array (as an integer from 0 to 80 inclusive).
    /// </summary>
    public int Lin { get; }

    public Digit Digit { get; }
    public Cell WithDigit(Digit digit)
    {
      return new Cell(Row, Col, Box, Lin, digit);
    }
    public Cell WithDigit(int digitValue)
    {
      return WithDigit(Digit.With(digitValue));
    }
    public Cell WithoutDigit(int digitValue)
    {
      return WithDigit(Digit.Without(digitValue));
    }
  }
}