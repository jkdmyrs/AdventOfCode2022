namespace AOC.Year_2022
{
  internal class Day12 : PuzzleBase
  {
    private static int _year = 2022;
    public Day12(bool part1, bool part2, bool isPractice)
      : base(_year, int.Parse(nameof(Day12).Substring(3)), part1, part2, isPractice) { }

    public override string Part1() => this.Part1Ans().ToString();
    public override string Part2() => this.Part2Ans().ToString();

    private (int, int) _start;
    private (int, int) _end;
    private List<(int, int)> _starts = new();

    private char[][] ParseGrid()
    {
      int i = 0;
      var grid = new List<char[]>();
      foreach (var line in this.PuzzleInput)
      {
        char InterceptStartAndEnd(char c, int i, int j)
        {
          switch (c)
          {
            case 'S':
              _start = (i, j);
              return 'a';
            case 'E':
              _end = (i, j);
              return 'z';
            default:
              return c;
          }
        }
        grid.Add(line.Select((x, j) => InterceptStartAndEnd(x, i, j)).ToArray());
        i++;
      }
      return grid.ToArray();
    }

    private char[][] ParseGridPart2()
    {
      int i = 0;
      var grid = new List<char[]>();
      foreach (var line in this.PuzzleInput)
      {
        char InterceptPositions(char c, int i, int j)
        {
          switch (c)
          {
            case 'a':
            case 'S':
              _starts.Add((i, j));
              return 'a';
            case 'E':
              _end = (i, j);
              return 'z';
            default:
              return c;
          }
        }
        grid.Add(line.Select((x, j) => InterceptPositions(x, i, j)).ToArray());
        i++;
      }
      return grid.ToArray();
    }

    // use BFS to find the shortest path
    private int FindShortestPath(char[][] grid, (int, int) start, (int, int) end)
    {
      var visited = new HashSet<(int, int)>();
      var queue = new Queue<(int, int)>();
      queue.Enqueue(start);
      visited.Add(start);
      var steps = 0;
      while (queue.Count > 0)
      {
        var count = queue.Count;
        for (int i = 0; i < count; i++)
        {
          var (x, y) = queue.Dequeue();
          var current = grid[x][y];
          if (x == end.Item1 && y == end.Item2)
            return steps;
          if (x > 0 && grid[x - 1][y] - current <= 1 && !visited.Contains((x - 1, y)))
          {
            queue.Enqueue((x - 1, y));
            visited.Add((x - 1, y));
          }
          if (x < grid.Length - 1 && grid[x + 1][y] - current <= 1 && !visited.Contains((x + 1, y)))
          {
            queue.Enqueue((x + 1, y));
            visited.Add((x + 1, y));
          }
          if (y > 0 && grid[x][y - 1] - current <= 1 && !visited.Contains((x, y - 1)))
          {
            queue.Enqueue((x, y - 1));
            visited.Add((x, y - 1));
          }
          if (y < grid[0].Length - 1 && grid[x][y + 1] - current <= 1 && !visited.Contains((x, y + 1)))
          {
            queue.Enqueue((x, y + 1));
            visited.Add((x, y + 1));
          }
        }
        steps++;
      }
      return -1;
    }

    private int Part1Ans()
    {
      var grid = ParseGrid();
      return FindShortestPath(grid, _start, _end);
    }

    private int Part2Ans()
    {
      var grid = ParseGridPart2();
      return _starts.Select(x => FindShortestPath(grid, x, _end)).Where(x => x != -1).Min();
    }
  }
}