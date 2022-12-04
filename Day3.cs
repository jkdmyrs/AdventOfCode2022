internal class Day3 : Puzzle
{
  public Day3(bool part1, bool part2, bool isPractice)
    : base(int.Parse(nameof(Day3).Substring(3)), part1, part2, isPractice)
  {
  }

  public override string Part1() => this.Part1Ans().ToString();

  private (char[], char[]) ParseLine(string line)
  {
    var half = line.Length/2;
    return (line.Substring(0, half).ToCharArray(), line.Substring(half, half).ToCharArray());
  }

  private char FindIntersect((char[], char[]) line)
  {
    return line.Item1.Intersect(line.Item2).First();
  }

  private int CalcPriority(char a)
  {
    int aInt = (int)a;
    if (aInt >= 97)
    {
      return aInt - 96;
    }
    return aInt - 38;
  }

  private int Part1Ans()
  {
    return this.PuzzleInput
      .Select(ParseLine)
      .Select(FindIntersect)
      .Select(CalcPriority)
      .Sum();
  }

  public override string Part2() => this.Part2Ans().ToString();

  private IEnumerable<((char[], char[]), (char[], char[]), (char[], char[]))> ParseGroups(IEnumerable<(char[], char[])> lines)
  {
    List<((char[], char[]), (char[], char[]), (char[], char[]))> groups = new();
    do
    {
      groups.Add((lines.Take(1).First(), lines.Skip(1).Take(1).First(), lines.Skip(2).Take(1).First()));
      lines = lines.Skip(3);
    }
    while(lines.Any());
    return groups;
  }

  private char FindGroupIntersect(((char[], char[]), (char[], char[]), (char[], char[])) group)
  {
    (char[], char[], char[]) concat = (group.Item1.Item1.Concat(group.Item1.Item2).ToArray(), group.Item2.Item1.Concat(group.Item2.Item2).ToArray(), group.Item3.Item1.Concat(group.Item3.Item2).ToArray());
    return concat.Item1.Intersect(concat.Item2).Intersect(concat.Item3).First();
  }

  private int Part2Ans()
  {
    var lines = this.PuzzleInput.Select(ParseLine);
    var groups = ParseGroups(lines);
    return groups.Select(FindGroupIntersect).Select(CalcPriority).Sum();
  }
}