namespace AOC.Year_2022
{
  // some aliases to keep things readable
  using Rock = Tuple<int, int>;
  using Spot = Tuple<int, int>;

  internal class Day14 : PuzzleBase
  {
    private static int _year = 2022;
    public Day14(bool part1, bool part2, bool isPractice)
      : base(_year, int.Parse(nameof(Day14).Substring(3)), part1, part2, isPractice) { }

    public override string Part1() => this.Part1Ans().ToString();
    public override string Part2() => this.Part2Ans().ToString();

    private int FindMinX()
    {
      int min = int.MaxValue;
      this.PuzzleInput.ForEach(x =>
      {
        var spilt = x.Split("->");
        spilt.ToList().ForEach(y =>
        {
          var split2 = y.Split(',');
          var temp = int.Parse(split2[0]);
          min = Math.Min(min, temp);
        });
      });
      return min;
    }

    private int FindMaxX()
    {
      int max = int.MinValue;
      this.PuzzleInput.ForEach(x =>
      {
        var spilt = x.Split("->");
        spilt.ToList().ForEach(y =>
        {
          var split2 = y.Split(',');
          var temp = int.Parse(split2[0]);
          max = Math.Max(max, temp);
        });
      });
      return max;
    }

    private int FindMaxY()
    {
      int max = int.MinValue;
      this.PuzzleInput.ForEach(x =>
      {
        var spilt = x.Split("->");
        spilt.ToList().ForEach(y =>
        {
          var split2 = y.Split(',');
          var temp = int.Parse(split2[1]);
          max = Math.Max(max, temp);
        });
      });
      return max;
    }

    private List<Rock[]> NormalizeRanges(int min)
    {
      var rocks = new List<Rock[]>();
      this.PuzzleInput.ForEach(x =>
      {
        var spilt = x.Split("->");
        var rock = new Rock[spilt.Length];
        for (int i = 0; i < spilt.Length; i++)
        {
          var split2 = spilt[i].Split(',');
          var temp = int.Parse(split2[0]);
          var temp2 = int.Parse(split2[1]);
          rock[i] = new Rock(temp - min, int.Parse(split2[1]));
        }
        rocks.Add(rock);
      });
      return rocks;
    }

    private List<Rock> ExpandRocks(List<Rock[]> rocks)
    {
      var expandedRocks = new HashSet<Rock>();
      rocks.ForEach(rockList =>
      {
        expandedRocks.Add(rockList[0]);
        for (int i = 0; i < rockList.Length - 1; i++)
        {
          var (x1, y1) = rockList[i];
          var (x2, y2) = rockList[i + 1];

          if (x1 == x2)
          {
            var smaller = Math.Min(y1, y2);
            var bigger = Math.Max(y1, y2);
            // Vertical
            for (int j = smaller; j < bigger + 1; j++)
            {
              expandedRocks.Add(new Rock(x1, j));
            }
          }
          else
          {
            // Horizontal
            var smaller = Math.Min(x1, x2);
            var bigger = Math.Max(x1, x2);
            for (int j = smaller; j < bigger + 1; j++)
            {
              expandedRocks.Add(new Rock(j, y1));
            }
          }
        }
      });
      return expandedRocks.ToList();
    }

    private Dictionary<Spot, char> BuildGrid(List<Rock> rocks, int minX, int maxX, int maxY)
    {
      Dictionary<Spot, char> grid = new();
      rocks.ForEach(rock =>
      {
        var (x, y) = rock;
        grid.Add(new Spot(y, x), '#');
      });
      return grid;
    }

    private int Part1Ans()
    {
      var minX = this.FindMinX();
      var maxX = this.FindMaxX();
      var maxY = this.FindMaxY();
      var rocks = this.NormalizeRanges(minX);
      var expandedRocks = this.ExpandRocks(rocks);
      var grid = this.BuildGrid(expandedRocks, minX, maxX, maxY);
      bool createNew = true;
      Spot currentSpot = new Spot(0, 500 - minX);
      int count = 0;
      do
      {
        if (createNew)
        {
          currentSpot = new Spot(0, 500 - minX);
          createNew = false;
        }

        var (cy, cx) = currentSpot;
        if (cy + 1 > maxY)
        {
          break;
        }
        var down = grid.TryGetValue(new Spot(cy + 1, cx), out char below) ? below : '.';
        if (down == '.')
        {
          // empty space below
          // move down
          currentSpot = new Spot(cy + 1, cx);
        }
        else
        {
          // empty diagnal left
          if (cx - 1 < 0 || cx + 1 > maxX)
          {
            break;
          }
          var dLeft = grid.TryGetValue(new Spot(cy + 1, cx - 1), out char diagLeft) ? diagLeft : '.';
          var dRight = grid.TryGetValue(new Spot(cy + 1, cx + 1), out char diagRight) ? diagRight : '.';
          if (dLeft == '.')
          {
            // move diagnal left
            currentSpot = new Spot(cy + 1, cx - 1);
          }
          // empty diagnal right
          else if (dRight == '.')
          {
            // move diagnal right
            currentSpot = new Spot(cy + 1, cx + 1);
          }
          else
          {
            grid.Add(currentSpot, 'O');
            count++;
            createNew = true;
          }
        }
      }
      while (true);

      //this.PrintGrid(grid);
      return count;
    }

    private int Part2Ans()
    {
      // refactor to use a dictionary of points instead of a grid

      return 0;
    }
  }
}