internal static class PuzzleRunner
{
  public static void Run((int day, int? part, bool answer) aocArgs)
  {
    var puzzles = AppDomain.
      CurrentDomain
      .GetAssemblies()
      .SelectMany(x => x.GetTypes())
      .Where(t => t.IsSubclassOf(typeof(Puzzle)))
      .Select(puzzle => (Puzzle?)puzzle?.GetConstructor(new[] { typeof(bool), typeof(bool), typeof(bool) })
        ?.Invoke(new object[] {
          !aocArgs.part.HasValue || aocArgs.part.Value == 1,
          !aocArgs.part.HasValue || aocArgs.part.Value == 2,
          !aocArgs.answer } ) ?? throw new ArgumentNullException(nameof(puzzle)));
    Console.WriteLine(puzzles.First(x => x.Day == aocArgs.day));
  }
}

internal static class CliParser
{
  internal class CliArg
  {
    public string? Arg { get; init; }
    public string? Val { get; init; }
  }

  public static (int day, int? part, bool answer) Parse(string[] args)
  {
    var cliArgs = string.Join(' ', args)
      .Split('-')
      .Select(x => x.Trim().GetArg())
      .Skip(1);
    return (
      cliArgs.First(x => x.Arg == "d").Val,
      cliArgs.FirstOrDefault(x => x.Arg == "p")?.Val,
      cliArgs.Any(x => x.Arg == "a")
    ).ToOutput();
  }
}

internal abstract class Puzzle
{
  public readonly Lazy<IEnumerable<string>> PuzzleInput;
  public abstract string Part1();
  public abstract string Part2();
  public readonly int Day;
  public readonly bool ExecutePart1;
  public readonly bool ExecutePart2;
  private bool _ioError = false;

  public Puzzle(int day, bool part1, bool part2, bool practice)
  {
    Day = day;
    ExecutePart1 = part1;
    ExecutePart2 = part2;
    PuzzleInput = new Lazy<IEnumerable<string>>(() =>
    {
      var timer = System.Diagnostics.Stopwatch.StartNew();
      List<string>? lines = null;
      try
      {
        lines = File
          .ReadAllLines(@$"input/{day.ToString() + (practice ? "_practice" : string.Empty)}.txt")
          .ToList();
      }
      catch
      {
        _ioError = true;
        Console.WriteLine("@I/O ERROR: file not found".Replace("@", Environment.NewLine));
      }
      timer.Stop();
      Console.WriteLine($"I/O time: {timer.ElapsedMilliseconds}ms@".Replace("@", Environment.NewLine));
      return lines ?? new List<string>();
    });
  }

  private void Load()
  {
    _ = PuzzleInput.Value;
  }

  public override string ToString()
  {
    this.Load();
    string part1 = string.Empty;
    string part2 = string.Empty;
    if (_ioError)
      return string.Empty;
    if (this.ExecutePart1)
    {
      System.Diagnostics.Stopwatch timer = System.Diagnostics.Stopwatch.StartNew();
      part1 = $"@Part 1 Answer: {this.Part1()}";
      timer.Stop();
      Console.WriteLine($"Part 1 Duration: {timer.ElapsedMilliseconds}ms");
    }

    if (this.ExecutePart2)
    {
      System.Diagnostics.Stopwatch timer = System.Diagnostics.Stopwatch.StartNew();
      part2 = $"@Part 2 Asnwer: {this.Part2()}";
      timer.Stop();
      Console.WriteLine($"Part 2 Duration: {timer.ElapsedMilliseconds}ms");
    }
    return $"{part1}{part2}".Replace("@", Environment.NewLine);
  }
}

internal static class InternalExtensions
{
  public static CliParser.CliArg GetArg(this string strArg)
  {
    var split = strArg.Split(' ');
    if (split[0] == "a")
    {
      return new CliParser.CliArg
      {
        Arg = "a"
      };
    }
    return new CliParser.CliArg
    {
      Arg = split[0],
      Val = split[1]
    };
  }

  public static (int day, int? part, bool answer) ToOutput(this (string? x, string? y, bool z) val)
  {
    return (int.Parse(val.x ?? throw new ArgumentNullException(nameof(val))), int.TryParse(val.y, out int part) ? part : null, val.z);
  }
}