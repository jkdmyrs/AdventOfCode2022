namespace AoC.Infra
{
  public static class PuzzleRunner
  {
    public static async Task Run((int, int?, bool, bool, string?) args)
    {
      var (day, part, solve, uploadPrompt, session) = args;
      var timer = System.Diagnostics.Stopwatch.StartNew();
      var puzzles = AppDomain.
        CurrentDomain
        .GetAssemblies()
        .SelectMany(x => x.GetTypes())
        .Where(t => t.IsSubclassOf(typeof(PuzzleBase)))
        .Select(puzzle => (PuzzleBase?)puzzle?.GetConstructor(new[] { typeof(bool), typeof(bool), typeof(bool) })
          ?.Invoke(new object[] {
            !part.HasValue || part.Value == 1,
            !part.HasValue || part.Value == 2,
            !solve }) ?? throw new ArgumentNullException(nameof(puzzle)));

      timer.Stop();
      Console.WriteLine($"@Puzzle Bootstrap Time: {timer.ElapsedMilliseconds}ms".Replace("@", Environment.NewLine));

      async Task<(string ans1, string ans2)> Solve(PuzzleBase p, string? session)
      {
        await p.Load(session).ConfigureAwait(false);
        var solveTimer = System.Diagnostics.Stopwatch.StartNew();
        var (ans1, ans2) = p.Solve();
        solveTimer.Stop();
        Console.WriteLine($"@Solve Time: {solveTimer.ElapsedMilliseconds}ms".Replace("@", Environment.NewLine));

        string ansFmt = "@Part {0} Answer: {1}";
        var part1 = p.ExecutePart1 ? string.Format(ansFmt, 1, ans1) : string.Empty;
        var part2 = p.ExecutePart2 ? string.Format(ansFmt, 2, ans2) : string.Empty;

        Console.WriteLine($"{part1}{part2}".Replace("@", Environment.NewLine));

        return (ans1 ?? string.Empty, ans2 ?? string.Empty);
      }

      async Task Upload(PuzzleBase p, string? session, int part, string ans)
      {
        try
        {
          Console.WriteLine("@Attempting to upload answer...".Replace("@", Environment.NewLine));
          var (uploadSuccess, highOrLow) = await new HttpClient().AnswerPuzzle(
            p.Year,
            p.Day,
            part,
            ans,
            session ?? string.Empty).ConfigureAwait(false);
          if (uploadSuccess)
            Console.WriteLine("Upload result: Correct");
          else
            Console.WriteLine($"Upload result: Incorrect... too {highOrLow}");
        }
        catch (Exception e)
        {
          Console.WriteLine("Upload failed: " + e.Message);
        }
      }

      var puzzle = puzzles.FirstOrDefault(x => x.Day == day && x.Year == StaticSettings.Year);
      if (puzzle is null)
      {
        Console.WriteLine($"@No puzzle found for day {day}, year {StaticSettings.Year}".Replace("@", Environment.NewLine));
        return;
      }
      var (ans1, ans2) = await Solve(puzzle, session).ConfigureAwait(false);
      if (uploadPrompt && solve && part is not null)
      {
        await Upload(puzzle, session, (int)part, part == 1 ? ans1 : ans2).ConfigureAwait(false);
      }
    }
  }
}