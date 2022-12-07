namespace AoC
{
  internal static class InternalExtensions
  {
    public static (int day, int? part, bool solve, bool uploadPrompt, string? session) ParseAndSetupEnv(this string[] args)
    {
      var parsed = CliParser.Parse(args);
      var (year, day, part, solve, upload, session) = parsed;

      Console.WriteLine($"AOC 2022");
      var display1 = part.HasValue ? $" {part.Value}" : "s 1+2";
      var display2 = solve ? "Real Input" : "Practice Input";
      Console.WriteLine($"Day {day}, Part{display1} -  {display2}");

      StaticSettings.ProjectBasePath = Path.Combine(Environment.CurrentDirectory, "AoC");
      StaticSettings.Year = year;
      return (day, part, solve, upload, session);
    }
  }
}