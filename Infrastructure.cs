// main
var aocArgs = CliParser.Parse(args);

Console.WriteLine($"AOC 2022");
string display1 = aocArgs.part.HasValue ? $" {aocArgs.part.Value}" : "s 1+2";
string display2 = aocArgs.answer ? "Real Input" : "Practice Input";
Console.WriteLine($"Day {aocArgs.day}, Part{display1} -  {display2}");

List<Puzzle> puzzles = new();
var puzzleClasses = AppDomain.
  CurrentDomain
  .GetAssemblies()
  .SelectMany(x => x.GetTypes())
  .Where(t => t.IsSubclassOf(typeof(Puzzle)))
  .ToList();
puzzleClasses.ForEach(puzzle => puzzles.Add(
  (Puzzle)puzzle.GetConstructor(new[] {typeof(bool), typeof(bool), typeof(bool)}).Invoke(new object[] { !aocArgs.part.HasValue || aocArgs.part.Value == 1, !aocArgs.part.HasValue || aocArgs.part.Value == 2, !aocArgs.answer } )));

Console.WriteLine(puzzles.First(x => x.Day == aocArgs.day));

// infra
internal static class CliParser
{
  public static (int day, int? part, bool answer) Parse(string[] args)
  {
    var cliArgs = string.Join(' ', args).Split('-').Select(x => x.Trim().GetArg()).Skip(1);
    return (cliArgs.First(x => x.Arg == "d").Val, cliArgs.FirstOrDefault(x => x.Arg == "p")?.Val, cliArgs.Any(x => x.Arg == "a")).ToOutput();
  }
}

internal class CliArg
{
  public string Arg { get; init; }
  public string Val { get; init; }
}

internal abstract class Puzzle
{
  public readonly Lazy<List<string>> PuzzleInput;
  public abstract string Part1();
  public abstract string Part2();
  public readonly int Day;
  public readonly bool ExecutePart1;
  public readonly bool ExecutePart2;

  public Puzzle(int day, bool part1, bool part2, bool practice)
  {
    Day = day;
    ExecutePart1 = part1;
    ExecutePart2 = part2;
    PuzzleInput = new Lazy<List<string>>(() => System.IO.File.ReadAllLines(@$"Input/{day.ToString() + (practice ? "_practice" : string.Empty)}.txt").ToList());
  }

  public override string ToString()
  {
    return this.ToAnswerString();
  }
}

internal static class InternalExtensions
{
  public static CliArg GetArg(this string strArg)
  {
    var split = strArg.Split(' ');
    if (split[0] == "a")
    {
      return new CliArg
      {
        Arg = "a"
      };
    }
    return new CliArg
    {
      Arg = split[0],
      Val = split[1]
    };
  }

  public static (int day, int? part, bool answer) ToOutput(this (string x, string y, bool z) val)
  {
    return (int.Parse(val.x), int.TryParse(val.y, out int part) ? part : null, val.z);
  }

  public static string ToAnswerString(this Puzzle puzzle)
  {
    string part1 = string.Empty;
    string part2 = string.Empty;
    if (puzzle.ExecutePart1)
    {
      part1 = $"@Part 1 Answer: {puzzle.Part1()}";
    }

    if (puzzle.ExecutePart2)
    {
      part2 = $"@Part 2 Asnwer: {puzzle.Part2()}";
    }
    return $"{part1}{part2}".Replace("@", System.Environment.NewLine);
  }
}