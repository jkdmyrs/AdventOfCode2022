namespace AoC.Infra
{
  internal class CliArg
  {
    public string? Arg { get; init; }
    public string? Val { get; init; }
  }

  public static class CliParser
  {
    public static (int year, int day, int? part, bool solve, bool uploadPrompt, string? session) Parse(string[] args)
    {
      var cliArgs = string.Join(' ', args)
        .Split('-')
        .Select(x => x.Trim().GetArg())
        .Skip(1);
      string? session = cliArgs.FirstOrDefault(x => x.Arg == "s")?.Val ?? Environment.GetEnvironmentVariable("AOC_SESSION");
      return (
        cliArgs.FirstOrDefault(x => x.Arg == "y")?.Val,
        cliArgs.First(x => x.Arg == "d").Val,
        cliArgs.FirstOrDefault(x => x.Arg == "p")?.Val,
        cliArgs.Any(x => x.Arg == "a"),
        cliArgs.Any(x => x.Arg == "u"),
        session
      ).ToOutput();
    }
  }
}