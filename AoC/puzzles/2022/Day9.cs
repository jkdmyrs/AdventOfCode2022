namespace AOC.Year_2022
{
  internal class Day9 : PuzzleBase
  {
    private static int _year = 2022;

    public Day9(bool part1, bool part2, bool isPractice)
      : base(_year, int.Parse(nameof(Day9).Substring(3)), part1, part2, isPractice)
    {
    }

    public override string Part1() => this.Part1Ans().ToString();

    private bool TouchingOrOverlapping((int, int) head, (int, int) tail)
    {
      // thanks to github copilot for this
      var (hx, hy) = head;
      var (tx, ty) = tail;
      return (hx == tx && hy == ty)
      || (hx == tx && hy == ty + 1)
      || (hx == tx && hy == ty - 1)
      || (hx == tx + 1 && hy == ty)
      || (hx == tx - 1 && hy == ty)
      || (hx == tx + 1 && hy == ty + 1)
      || (hx == tx - 1 && hy == ty - 1)
      || (hx == tx + 1 && hy == ty - 1)
      || (hx == tx - 1 && hy == ty + 1);
    }

    private (int, int) Move((int, int) head, char direction)
    {
      var (x, y) = head;

      switch (direction)
      {
        case 'U':
          y += 1;
          break;
        case 'D':
          y -= 1;
          break;
        case 'R':
          x += 1;
          break;
        case 'L':
          x -= 1;
          break;
      }

      return (x, y);
    }

    private (int, int) Follow((int, int) head, (int, int) tail)
    {
      var (hx, hy) = head;
      var (tx, ty) = tail;
      if (!TouchingOrOverlapping(head, tail))
      {
        var dx = hx - tx == 0 ? 0 : (hx - tx) / Math.Abs(hx - tx);
        var dy = hy - ty == 0 ? 0 : (hy - ty) / Math.Abs(hy - ty);

        tail = (tx + (1 * dx), ty + (1 * dy));
      }
      return tail;
    }

    private int Part1Ans()
    {
      var visited = new HashSet<(int, int)>();
      visited.Add((0, 0));
      var moves = this.PuzzleInput.Select(x => x.ToArray()).Select(x => (x[0], int.Parse(string.Join(string.Empty, x.Skip(1)))));
      var head = (0, 0);
      var tail = (0, 0);

      foreach (var (direction, distance) in moves)
      {
        for (int i = 0; i < distance; i++)
        {
          head = Move(head, direction);
          tail = Follow(head, tail);
          visited.Add(tail);
        }
      }
      return visited.Count;
    }

    public override string Part2() => this.Part2Ans().ToString();

    private int Part2Ans()
    {
      var visited = new HashSet<(int, int)>();
      visited.Add((0, 0));
      var moves = this.PuzzleInput.Select(x => x.ToArray()).Select(x => (x[0], int.Parse(string.Join(string.Empty, x.Skip(1)))));
      var knots = Enumerable.Repeat((0, 0), 10).ToArray();

      foreach (var (direction, distance) in moves)
      {
        for (int i = 0; i < distance; i++)
        {
          knots[0] = Move(knots[0], direction);
          for (int k = 1; k < knots.Length; k++)
          {
            knots[k] = Follow(knots[k - 1], knots[k]);
          }
          visited.Add(knots.Last());
        }
      }
      return visited.Count;
    }
  }
}