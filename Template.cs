internal class TEMPLATE_DAY : Puzzle
{
  public TEMPLATE_DAY(bool part1, bool part2, bool isPractice)
    : base(int.Parse(nameof(TEMPLATE_DAY).Substring(3)), part1, part2, isPractice)
  {
  }

  public override string Part1() => this.Part1Ans().ToString();

  private int Part1Ans()
  {
    return 0;
  }

  public override string Part2() => this.Part2Ans().ToString();

  private int Part2Ans()
  {
    return 0;
  }
}