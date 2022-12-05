internal class Day5 : Puzzle
{
  public Day5(bool part1, bool part2, bool isPractice)
    : base(int.Parse(nameof(Day5).Substring(3)), part1, part2, isPractice)
  {
  }

  public override string Part1() => this.Part1Ans().ToString();

  private (int cols, int initRows) Setup()
  {
    int i = 0;
    var lines = new List<string>(this.PuzzleInput);
    do
    {
      string line = lines.First();
      lines = lines.Skip(1).ToList();
      i++;
    } while (lines.Any(x => x.Contains('[')));
    var cols = lines.First().Split(' ').Where(x => !string.IsNullOrWhiteSpace(x)).Select(int.Parse).Max();
    return (cols, i);
  }

  private bool TryParseLine(string line, int col, out char val)
  {
    int i = col * 4;
    var value = line.Skip(i + 1).Take(1).First();
    if (value != ' ')
    {
      val = value;
      return true;
    }
    val = ' ';
    return false;
  }

  private (int count, int from, int to) ParseMove(string line)
  {
    var split = line.Split(' ');
    return (int.Parse(split[1]), int.Parse(split[3]) - 1, int.Parse(split[5]) - 1);
  }

  private Dictionary<int, Stack<char>> ParseCrates(int cols, int rows)
  {
    Dictionary<int, Stack<char>> results = new();
    Dictionary<int, Queue<char>> crates = new();
    for (int i = 0; i < cols; i++)
    {
      crates[i] = new Queue<char>();
      results[i] = new Stack<char>();

      this.PuzzleInput.Take(rows).ToList().ForEach(x =>
      {
        if (TryParseLine(x, i, out char val))
        {
          crates[i].Enqueue(val);
        }
      });
    }
    crates.Keys.ToList().ForEach(x =>
    {
      var cratesQ = crates[x].Reverse();
      while (cratesQ.Any())
      {
        results[x].Push(cratesQ.First());
        cratesQ = cratesQ.Skip(1);
      }
    });
    return results;
  }

  private void HandleMove((int, int, int) move, Dictionary<int, Stack<char>> crates)
  {
    var (count, from, to) = move;
    for (int i = 0; i < count; i++)
    {
      var pop = crates[from].Pop();
      crates[to].Push(pop);
    }
  }

  private string Part1Ans()
  {
    var (cols, rows) = Setup();
    var crates = ParseCrates(cols, rows);
    var moves = this.PuzzleInput.Skip(rows + 2).Select(ParseMove).ToList();

    moves.ForEach(move =>
    {
      HandleMove(move, crates);
    });

    using StringWriter stringWriter = new();
    crates.Keys.ToList().ForEach(key =>
    {
      stringWriter.Write(crates[key].Pop());
    });
    return stringWriter.ToString();
  }

  public override string Part2() => this.Part2Ans().ToString();

  private void HandleMove2((int, int, int) move, Dictionary<int, Stack<char>> crates)
  {
    var (count, from, to) = move;
    var temp = new List<char>();
    for (int i = 0; i < count; i++)
    {
      var pop = crates[from].Pop();
      temp.Add(pop);
    }
    temp.Reverse();
    temp.ForEach(x =>
    {
      crates[to].Push(x);
    });
  }

  private string Part2Ans()
  {
    var (cols, rows) = Setup();
    var crates = ParseCrates(cols, rows);
    var moves = this.PuzzleInput.Skip(rows + 2).Select(ParseMove).ToList();

    moves.ForEach(move =>
    {
      HandleMove2(move, crates);
    });

    using StringWriter stringWriter = new();
    crates.Keys.ToList().ForEach(key =>
    {
      stringWriter.Write(crates[key].Pop());
    });
    return stringWriter.ToString();
  }
}