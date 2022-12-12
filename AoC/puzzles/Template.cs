namespace AOC.Year_20XX
{
  internal class Day0 : PuzzleBase
  {
    private static int _year = 2022;
    public Day0(bool part1, bool part2, bool isPractice)
      : base(_year, int.Parse(nameof(Day0).Substring(3)), part1, part2, isPractice) { }

    public override string Part1() => this.Part1Ans().ToString();
    public override string Part2() => this.Part2Ans().ToString();

    private int Part1Ans()
    {
      return 0;
    }

    private int Part2Ans()
    {
      return 0;
    }
  }
}