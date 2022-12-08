namespace AOC.Year_2022
{
  internal class Day8 : PuzzleBase
  {
    private static int _year = 2022;

    public Day8(bool part1, bool part2, bool isPractice)
      : base(_year, int.Parse(nameof(Day8).Substring(3)), part1, part2, isPractice)
    {
    }

    public override string Part1() => this.Part1Ans().ToString();

    private int[][] ParseGrid()
    {
      var grid = new int[this.PuzzleInput.Count][];
      for (int i = 0; i < this.PuzzleInput.Count; i++)
      {
        grid[i] = this.PuzzleInput[i].ToString().ToCharArray().Select(x => int.Parse(x.ToString())).ToArray();
      }
      return grid;
    }

    private int Part1Ans()
    {
      HashSet<(int, int)> visited = new();
      var grid = this.ParseGrid();

      for (int i = 0; i < grid.Length; i++)
      {
        for (int j = 0; j < grid[i].Length; j++)
        {
          if (i == 0 || i == grid.Length - 1 || j == 0 || j == grid[i].Length - 1)
          {
            visited.Add((i, j));
            continue;
          }

          var row = grid[i];
          var tree = row[j];
          var left = row.Take(j).ToList();
          var right = row.Skip(j + 1).ToList();
          if (!left.Any(x => x >= tree) || !right.Any(x => x >= tree))
          {
            visited.Add((i, j));
            continue;
          }

          var column = grid.Select(x => x[j]).ToList();
          var above = column.Take(i).ToList();
          var below = column.Skip(i + 1).ToList();
          if (!above.Any(x => x >= tree) || !below.Any(x => x >= tree))
          {
            visited.Add((i, j));
            continue;
          }
        }
      }

      return visited.Count;
    }

    public override string Part2() => this.Part2Ans().ToString();

    private int Part2Ans()
    {
      var grid = this.ParseGrid();
      var viewScore = 0;

      for (int i = 0; i < grid.Length; i++)
      {
        for (int j = 0; j < grid[i].Length; j++)
        {
          if (i == 0 || i == grid.Length - 1 || j == 0 || j == grid[i].Length - 1)
          {
            continue;
          }

          int temp = 0;
          int score = 0;

          var row = grid[i];
          var tree = row[j];
          var left = row.Take(j).ToList();
          left.Reverse();
          for (int k = 0; k < left.Count; k++)
          {
            if (left[k] == tree)
            {
              temp++;
              break;
            }
            if (left[k] > tree)
            {
              temp++;
              break;
            }
            temp++;
          }
          score = temp;
          temp = 0;
          var right = row.Skip(j + 1).ToList();
          for (int k = 0; k < right.Count; k++)
          {
            if (right[k] == tree)
            {
              temp++;
              break;
            }
            if (right[k] > tree)
            {
              temp++;
              break;
            }
            temp++;
          }
          score = score * temp;

          temp = 0;
          var column = grid.Select(x => x[j]).ToList();
          var above = column.Take(i).ToList();
          above.Reverse();
          for (int k = 0; k < above.Count; k++)
          {
            if (above[k] == tree)
            {
              temp++;
              break;
            }
            if (above[k] > tree)
            {
              temp++;
              break;
            }
            temp++;
          }
          score = score * temp;
          temp = 0;
          var below = column.Skip(i + 1).ToList();
          for (int k = 0; k < below.Count; k++)
          {
            if (below[k] == tree)
            {
              temp++;
              break;
            }
            if (below[k] > tree)
            {
              temp++;
              break;
            }
            temp++;
          }
          score = score * temp;

          viewScore = Math.Max(viewScore, score);
        }
      }

      return viewScore;
    }
  }
}