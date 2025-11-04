
namespace CogitoErgoSudokum
{
  /// <summary>
  /// A single digit value or plurality in a Sudoku grid cell.
  /// </summary>
  public readonly struct Digit
  {
    private static readonly int[] _flags = new[] {
      1 << 0, // Blank
      1 << 1, // Value 1
      1 << 2, // Value 2
      1 << 3, // Value 3
      1 << 4, // Value 4
      1 << 5, // Value 5
      1 << 6, // Value 6
      1 << 7, // Value 7
      1 << 8, // Value 8
      1 << 9, // Value 9
    };

    private readonly int _state;

    public static Digit FullyPlural
    {
      get { return new Digit(false, 0b1111111110); }
    }

    /// <summary>
    /// Create an assigned digit.
    /// </summary>
    public static Digit CreateAssigned(int value)
    {
      if (value < 1 || value > 9)
        throw new ArgumentException("Only valid Sudoku digits may be assigned.");
      return new Digit(true, _flags[value]);
    }
    /// <summary>
    /// Create an unassigned digit.
    /// </summary>
    public static Digit CreateUnassigned(params int[] plurality)
    {
      int state = 0;
      foreach (var v in plurality)
        if (v < 1 || v > 9)
          throw new ArgumentException("Only valid Sudoku digits may be assigned.");
        else
          state |= _flags[v];

      return new Digit(true, state);
    }
    private Digit(bool assigned, int state)
    {
      _state = state;
      Assigned = assigned;

      Plurality = 0;
      for (int i = 1; i <= 9; i++)
        if ((state & _flags[i]) != 0)
          Plurality++;
    }

    /// <summary>
    /// Test whether this digit is able to be the given value.
    /// </summary>
    /// <param name="value">Sudoku value, must be in the range 1-9 inclusive.</param>
    public readonly bool Has(int value)
    {
      return (_state & _flags[value]) != 0;
    }
    /// <summary>
    /// Test whether this digit is the given value.
    /// </summary>
    /// <param name="value">Sudoku value, must be in the range 1-9 inclusive.</param>
    public readonly bool Is(int value)
    {
      return (_state & _flags[value]) == _flags[value];
    }

    /// <summary>
    /// Iterate over all possible values of this digit.
    /// </summary>
    public IEnumerable<int> Values
    {
      get
      {
        for (int v = 1; v <= 9; v++)
          if (Has(v))
            yield return v;
      }
    }

    /// <summary>
    /// Gets whether this digit was assigned by the user.
    /// </summary>
    public bool Assigned { get; }
    /// <summary>
    /// Gets the number of values this digit can be.
    /// </summary>
    public int Plurality { get; }

    /// <summary>
    /// Create a digit similar to this one, but without the given value.
    /// </summary>
    public readonly Digit Without(int value)
    {
      return new Digit(false, _state & ~_flags[value]);
    }
    /// <summary>
    /// Create a digit similar to this one, but with the given value.
    /// </summary>
    public readonly Digit With(int value)
    {
      return new Digit(false, _state | _flags[value]);
    }

    public override readonly string ToString()
    {
      if (Plurality == 1)
      {
        for (int i = 1; i <= 9; i++)
          if (Is(i))
            return i.ToString();
        return "?";
      }
      else
        return "{" + Plurality + "}";
    }
  }
}