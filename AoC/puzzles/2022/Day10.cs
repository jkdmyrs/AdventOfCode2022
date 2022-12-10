namespace AOC.Year_2022
{
  using Instruction = Tuple<string, int>;

  internal class Day10 : PuzzleBase
  {
    private static int _year = 2022;

    public Day10(bool part1, bool part2, bool isPractice)
      : base(_year, int.Parse(nameof(Day10).Substring(3)), part1, part2, isPractice)
    {
    }

    public override string Part1() => this.Part1Ans().ToString();

    private int Part1Ans()
    {
      var instructions = this.PuzzleInput.Select(x =>
      {
        var split = x.Split(' ');
        return new Instruction(split[0], split.Length == 2 ? int.Parse(split[1]) : 0);
      }).GetEnumerator();
      instructions.MoveNext();

      int clock = 0;
      int register = 1;
      var cur = instructions.Current;
      List<int> checks = new() { 20, 60, 100, 140, 180, 220 };
      List<int> strengths = new();
      var executeAddx = false;
      bool moveNext = true;
      do
      {
        clock++;

        if (checks.Contains(clock))
        {
          strengths.Add(register * clock);
        }

        var (op, val) = cur;
        switch (op)
        {
          case "noop":
            moveNext = instructions.MoveNext();
            break;
          case "addx":
            if (executeAddx)
            {
              register += val;
              moveNext = instructions.MoveNext();
            }
            executeAddx = !executeAddx;
            break;
        }

        cur = instructions.Current;
      }
      while (clock < 220 && moveNext);
      return strengths.Sum();
    }

    public override string Part2() => this.Part2Ans().ToString();

    private string Part2Ans()
    {
      var instructions = this.PuzzleInput.Select(x =>
      {
        var split = x.Split(' ');
        return new Instruction(split[0], split.Length == 2 ? int.Parse(split[1]) : 0);
      }).GetEnumerator();
      instructions.MoveNext();

      var crt = new int[6][];
      for (int i = 0; i < crt.Length; i++)
      {
        crt[i] = new int[40];
      }

      int clock = 0;
      int register = 1;
      var cur = instructions.Current;
      var executeAddx = false;
      bool moveNext = true;
      do
      {
        clock++;
        int crtRow = (clock) / 40;
        IEnumerable<int> sprite;
        if (register == 0)
        {
          sprite = Enumerable.Range(0, 3);
        }
        else if (register == 39)
        {
          sprite = Enumerable.Range(37, 3);
        }
        else
        {
          sprite = Enumerable.Range(register - 1, 3);
        }

        var pos = (clock - 1) % 40;
        if (sprite.Contains(pos))
        {
          crt[crtRow][pos] = 1; // #
        }
        else
        {
          crt[crtRow][pos] = 0; // .
        }

        var (op, val) = cur;
        switch (op)
        {
          case "noop":
            moveNext = instructions.MoveNext();
            break;
          case "addx":
            if (executeAddx)
            {
              register += val;
              moveNext = instructions.MoveNext();
            }
            executeAddx = !executeAddx;
            break;
        }

        cur = instructions.Current;
      }
      while (clock < 239 && moveNext);
      using StringWriter sw = new();
      sw.WriteLine();
      foreach (var row in crt)
      {
        sw.WriteLine(string.Join("", row.Select(x => x == 1 ? "#" : ".")));
      }
      return sw.ToString();
    }
  }
}