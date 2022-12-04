internal class Day4 : Puzzle
{
  public Day4(bool part1, bool part2, bool isPractice)
    : base(int.Parse(nameof(Day4).Substring(3)), part1, part2, isPractice)
  {
  }

  public override string Part1() => this.Part1Ans().ToString();

  internal class Range
  {
    public Range(string[] elf)
    {
      this.Lower = int.Parse(elf[0]);
      this.Upper = int.Parse(elf[1]);
    }
    public int Upper { get; set; }
    public int Lower { get; set; }
  }

  private (Range elf1, Range elf2) ParseLine(string line)
  {
    var split = line.Split(',');
    
    var elf1Split = split[0].Split('-');
    var elf2Split = split[1].Split('-');

    return (new Range(elf1Split), new Range(elf2Split));
  }

  private bool FullyContains((Range, Range) input)
  {
    (var elf1, var elf2) = input;
    if (elf2.Contains(elf1)) {
      return true;
    }
    if (elf1.Contains(elf2)) {
      return true;
    }
    return false;
  }

  private int Part1Ans()
  {
    var elfPairs = this.PuzzleInput.Select(ParseLine);
    var containsResults = elfPairs.Select(FullyContains);
    return containsResults.Where(x => x).Count();
  }

  public override string Part2() => this.Part2Ans().ToString();

  private int Part2Ans()
  {
    var elfPairs = this.PuzzleInput.Select(ParseLine);

    int count = 0;

    elfPairs.ToList().ForEach(x => 
    {
      (var elf1, var elf2) = x;
      var elf1Enumerable = Enumerable.Range(elf1.Lower, elf1.Upper - elf1.Lower + 1);
      var elf2Enumerable = Enumerable.Range(elf2.Lower, elf2.Upper - elf2.Lower + 1);
      if (elf1Enumerable.Intersect(elf2Enumerable).Any()) {
        count++;
      }
    });
    return count;
  }
}

internal static class RangeExtensions
{
  public static bool Contains(this Day4.Range range, Day4.Range range2)
  {
    return range2.Lower >= range.Lower && range2.Upper <= range.Upper;
  }
}