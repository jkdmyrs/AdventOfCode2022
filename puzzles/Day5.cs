internal class Day5 : Puzzle
{
  public Day5(bool part1, bool part2, bool isPractice)
    : base(int.Parse(nameof(Day5).Substring(3)), part1, part2, isPractice)
  {
  }

  public override string Part1() => this.Part1Ans().ToString();

  public (int, int) ParseLine(string line)
  {
    return (0, 0);
  }

  private int Part1Ans()
  {
    var parsed = this.PuzzleInput.Select(ParseLine);
    return 0;
  }

  public override string Part2() => this.Part2Ans().ToString();

  private int Part2Ans()
  {
    return 0;
  }
}