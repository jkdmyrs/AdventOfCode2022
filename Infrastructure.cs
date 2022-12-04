internal static class PuzzleRunner
{
  public static async Task Run((int day, int? part, bool answer, string? session) aocArgs)
  {
    var timer = System.Diagnostics.Stopwatch.StartNew();
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

    async Task<string> Solve(Puzzle p, string? session)
    {
      await p.Load(session).ConfigureAwait(false);
      var solveTimer = System.Diagnostics.Stopwatch.StartNew();
      string solve = p.ToString();
      solveTimer.Stop();
      Console.WriteLine($"@Solve Time: {solveTimer.ElapsedMilliseconds}ms".Replace("@", Environment.NewLine));
      return solve;
    }
    
    timer.Stop();
    Console.WriteLine($"@Puzzle Bootstrap Time: {timer.ElapsedMilliseconds}ms".Replace("@", Environment.NewLine));
    var answer = await Solve(puzzles.First(x => x.Day == aocArgs.day), aocArgs.session).ConfigureAwait(false);
    Console.WriteLine(answer);
  }
}

internal static class CliParser
{
  internal class CliArg
  {
    public string? Arg { get; init; }
    public string? Val { get; init; }
  }

  public static (int day, int? part, bool answer, string? session) Parse(string[] args)
  {
    var cliArgs = string.Join(' ', args)
      .Split('-')
      .Select(x => x.Trim().GetArg())
      .Skip(1);
    string? session = cliArgs.FirstOrDefault(x => x.Arg == "s")?.Val ?? Environment.GetEnvironmentVariable("AOC_SESSION");
    return (
      cliArgs.First(x => x.Arg == "d").Val,
      cliArgs.FirstOrDefault(x => x.Arg == "p")?.Val,
      cliArgs.Any(x => x.Arg == "a"),
      session
    ).ToOutput();
  }
}

public class AsyncLazy<T> : Lazy<Task<T>>
{
  public AsyncLazy(Func<T> valueFactory) :
    base(() => Task.Factory.StartNew(valueFactory)) { }

  public AsyncLazy(Func<Task<T>> taskFactory) :
    base(() => Task.Factory.StartNew(() => taskFactory()).Unwrap()) { }
}

internal abstract class Puzzle
{
  private readonly AsyncLazy<IEnumerable<string>> _puzzleInput;
  public abstract string Part1();
  public abstract string Part2();
  public readonly int Day;
  public readonly bool ExecutePart1;
  public readonly bool ExecutePart2;
  private bool _ioError = false;
  private bool _isPractice;
  public List<string> PuzzleInput = Array.Empty<string>().ToList();
  private string? _session = null;

  public Puzzle(int day, bool part1, bool part2, bool practice)
  {
    Day = day;
    ExecutePart1 = part1;
    ExecutePart2 = part2;
    _isPractice = practice;
    _puzzleInput = new AsyncLazy<IEnumerable<string>>(async () =>
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
        Console.WriteLine("I/O ERROR: file not found");
      }
    
      // fallback to AOC API for our puzzle input
      // non-practice only
      if (!_isPractice && (!lines?.Any() ?? true))
      {
        Console.WriteLine("Attempting to fetch input");
        var request = new HttpRequestMessage();
        request.Method = HttpMethod.Get;
        request.Headers.TryAddWithoutValidation("User-Agent", "https://github.com/jkdmyrs/advent-of-code-csharp by jk@dmyrs.com");
        request.RequestUri = new Uri($"https://adventofcode.com/2022/day/{Day}/input");
        request.Headers.Add("cookie", $"session={this._session ?? string.Empty}");
        var response = await new HttpClient().SendAsync(request).ConfigureAwait(false);
        try
        {
          response.EnsureSuccessStatusCode();
          var temp = (await response.Content.ReadAsStringAsync().ConfigureAwait(false)).Split(Environment.NewLine);
          lines = temp.Take(temp.Count()-1).ToList();
          if (lines.Any())
          {
            try
            {
              await File.WriteAllTextAsync(@$"input/{Day.ToString()}.txt", string.Join(Environment.NewLine, lines)).ConfigureAwait(false);
            }
            catch
            {
              _ioError = true;
              Console.WriteLine("I/O ERROR: failed to write input file");
            }
          }
          else
          {
            _ioError = true;
            Console.WriteLine("I/O ERROR: downloaded file was empty");
          }
        }
        catch 
        {
          _ioError = true;
          Console.WriteLine("I/O ERROR: failed to download input file");
        }
      }

      timer.Stop();
      Console.WriteLine($"I/O time: {timer.ElapsedMilliseconds}ms@".Replace("@", Environment.NewLine));
      return lines ?? new List<string>();
    });
  }

  public async Task Load(string? session)
  {
    _session = session;
    // force the lazy loaded IO to happen
    PuzzleInput = (await _puzzleInput.Value.ConfigureAwait(false)).ToList();
  }

  public override string ToString()
  {
    string part1 = string.Empty;
    string part2 = string.Empty;
    if (_ioError && !PuzzleInput.Any())
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

  public static (int day, int? part, bool answer, string? session) ToOutput(this (string? x, string? y, bool z, string? session) val)
  {
    return (int.Parse(val.x ?? throw new ArgumentNullException(nameof(val))), int.TryParse(val.y, out int part) ? part : null, val.z, val.session);
  }
}