namespace AoC.Infra
{
  public abstract class PuzzleBase
  {
    private bool _isPractice;
    private string? _session = null;
    private bool _ioError = false;

    public readonly bool ExecutePart1;
    public readonly bool ExecutePart2;
    public abstract string Part1();
    public abstract string Part2();
    public readonly int Year;
    public readonly int Day;
    public List<string> PuzzleInput = Array.Empty<string>().ToList();

    public PuzzleBase(int year, int day, bool part1, bool part2, bool practice)
    {
      Year = year;
      Day = day;
      ExecutePart1 = part1;
      ExecutePart2 = part2;
      _isPractice = practice;
    }

    public async Task Load(string? session)
    {
      _session = session;
      var timer = System.Diagnostics.Stopwatch.StartNew();
      string filePath = Path.Combine(StaticSettings.ProjectBasePath, "input", Year.ToString(), _isPractice ? Path.Combine("practice", $"{Day}.txt") : $"{Day}.txt");
      // we might have a double "AoC" in the path depending on OS
      // remove it
      filePath = filePath.Replace(Path.Combine("AoC","AoC"), "AOC");
      List<string>? lines = null;
      try
      {
        lines = File.ReadAllLines(filePath).ToList();
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
        try
        {
          Console.WriteLine("Attempting to fetch input");
          lines = (await new HttpClient().GetPuzzleInput(Year, Day, _session ?? string.Empty).ConfigureAwait(false)).ToList();
          if (lines.Any())
          {
            try
            {
              await File.WriteAllTextAsync(filePath, string.Join(Environment.NewLine, lines)).ConfigureAwait(false);
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
      this.PuzzleInput = lines ?? new List<string>();
    }

    public (string? ans1, string? ans2) Solve()
    {
      string? ans1 = null;
      string? ans2 = null;
      try
      {
        if (_ioError && !PuzzleInput.Any())
        {
          Console.WriteLine($"Unable to solve due to IO error.");
          return (null, null);
        }

        if (this.ExecutePart1)
        {
          System.Diagnostics.Stopwatch timer = System.Diagnostics.Stopwatch.StartNew();
          ans1 = Part1();
          timer.Stop();
          Console.WriteLine($"Part 1 Duration: {timer.ElapsedMilliseconds}ms");
        }

        if (this.ExecutePart2)
        {
          System.Diagnostics.Stopwatch timer = System.Diagnostics.Stopwatch.StartNew();
          ans2 = Part2();
          timer.Stop();
          Console.WriteLine($"Part 2 Duration: {timer.ElapsedMilliseconds}ms");
        }

        return (ans1, ans2);
      }
      catch (Exception e)
      {
        Console.WriteLine($"Unable to solve due to exception: {e.Message}");
        throw;
      }
    }
  }
}