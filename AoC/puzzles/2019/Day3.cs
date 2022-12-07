namespace AOC.Year_2019
{
  internal class Day3 : PuzzleBase
  {
    private static int _year = 2019;

    public Day3(bool part1, bool part2, bool isPractice)
      : base(_year, int.Parse(nameof(Day3).Substring(3)), part1, part2, isPractice)
    {
    }

    public override string Part1() => Part1Ans().ToString();

    public override string Part2() => this.Part2Ans().ToString();

    private int Part1Ans()
    {
      List<(char dir, int dist)> moves1 = new();
      List<(char dir, int dist)> moves2 = new();
      moves1 = this.PuzzleInput.First().Split(',').Select(x => (x[0], int.Parse(x[1..]))).ToList();
      moves2 = this.PuzzleInput.Skip(1).First().Split(',').Select(x => (x[0], int.Parse(x[1..]))).ToList();

      List<(int x, int y)> w1 = new();
      (int x, int y) current = (0, 0);
      moves1.ForEach(move =>
      {
        var newLoc = GetNewLocation(move, current);

        (int x, int y) diff = (newLoc.x - current.x, newLoc.y - current.y);
        if (diff.x == 0)
        {
          // vertical
          for (int i = 0; i < Math.Abs(diff.y); i++)
          {
            w1.Add((current.x, current.y + ((i + 1) * (Math.Abs(diff.y) / diff.y))));
          }
        }
        else if (diff.y == 0)
        {
          // horizontal
          for (int i = 0; i < Math.Abs(diff.x); i++)
          {
            w1.Add(((current.x + ((i + 1) * (Math.Abs(diff.x) / diff.x))), current.y));
          }
        }
        else
          throw new Exception("unexpected");
        current = newLoc;
      });

      List<(int x, int y)> w2 = new();
      current = (0, 0);
      moves2.ForEach(move =>
      {
        var newLoc = GetNewLocation(move, current);

        (int x, int y) diff = (newLoc.x - current.x, newLoc.y - current.y);
        if (diff.x == 0)
        {
          // vertical
          for (int i = 0; i < Math.Abs(diff.y); i++)
          {
            w2.Add((current.x, current.y + ((i + 1) * (Math.Abs(diff.y) / diff.y))));
          }
        }
        else if (diff.y == 0)
        {
          // horizontal
          for (int i = 0; i < Math.Abs(diff.x); i++)
          {
            w2.Add(((current.x + ((i + 1) * (Math.Abs(diff.x) / diff.x))), current.y));
          }
        }
        else
          throw new Exception("unexpected");
        current = newLoc;
      });

      var intersect = w1.Intersect(w2);
      return intersect.Min(x => Math.Abs(x.x) + Math.Abs(x.y));
    }

    public (int x, int y) GetNewLocation((char dir, int dist) move, (int x, int y) loc)
    {
      (int x, int y) newLoc;
      switch (move.dir)
      {
        case 'R':
          newLoc = (loc.x + move.dist, loc.y);
          break;
        case 'U':
          newLoc = (loc.x, loc.y + move.dist);
          break;
        case 'L':
          newLoc = (loc.x - move.dist, loc.y);
          break;
        case 'D':
          newLoc = (loc.x, loc.y - move.dist);
          break;
        default:
          throw new Exception("invalid direction");
      }
      return newLoc;
    }

    private int Part2Ans()
    {
      List<(char dir, int dist)> moves1 = new();
      List<(char dir, int dist)> moves2 = new();
      moves1 = this.PuzzleInput.First().Split(',').Select(x => (x[0], int.Parse(x[1..]))).ToList();
      moves2 = this.PuzzleInput.Skip(1).First().Split(',').Select(x => (x[0], int.Parse(x[1..]))).ToList();

      List<((int x, int y) point, int steps)> w1 = new();
      (int x, int y) current = (0, 0);
      int steps = 0;
      moves1.ForEach(move =>
      {
        var newLoc = GetNewLocation(move, current);

        (int x, int y) diff = (newLoc.x - current.x, newLoc.y - current.y);
        if (diff.x == 0)
        {
          // vertical
          for (int i = 0; i < Math.Abs(diff.y); i++)
          {
            steps++;
            w1.Add((((current.x, current.y + ((i + 1) * (Math.Abs(diff.y) / diff.y)))), steps));
          }
        }
        else if (diff.y == 0)
        {
          // horizontal
          for (int i = 0; i < Math.Abs(diff.x); i++)
          {
            steps++;
            w1.Add((((current.x + ((i + 1) * (Math.Abs(diff.x) / diff.x))), current.y), steps));
          }
        }
        else
          throw new Exception("unexpected");
        current = newLoc;
      });

      List<((int x, int y) point, int steps)> w2 = new();
      current = (0, 0);
      steps = 0;
      moves2.ForEach(move =>
      {
        var newLoc = GetNewLocation(move, current);

        (int x, int y) diff = (newLoc.x - current.x, newLoc.y - current.y);
        if (diff.x == 0)
        {
          // vertical
          for (int i = 0; i < Math.Abs(diff.y); i++)
          {
            steps++;
            w2.Add(((current.x, current.y + ((i + 1) * (Math.Abs(diff.y) / diff.y))), steps));
          }
        }
        else if (diff.y == 0)
        {
          // horizontal
          for (int i = 0; i < Math.Abs(diff.x); i++)
          {
            steps++;
            w2.Add(((((current.x + ((i + 1) * (Math.Abs(diff.x) / diff.x))), current.y)), steps));
          }
        }
        else
          throw new Exception("unexpected");
        current = newLoc;
      });

      var intersects = w1.Select(x => x.point).Intersect(w2.Select(x => x.point));

      List<int> results = new();
      foreach (var point in intersects)
      {
        results.Add(w1.First(x => x.point == point).steps + w2.First(x => x.point == point).steps);
      }
      return results.Min();
    }
  }
}