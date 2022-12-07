namespace AOC.Year_2019
{
  internal class Day1 : PuzzleBase
  {
    private static int _year = 2019;

    public Day1(bool part1, bool part2, bool isPractice)
      : base(_year, int.Parse(nameof(Day1).Substring(3)), part1, part2, isPractice)
    {
    }

    public override string Part1() => this.Part1Ans().ToString();

    private int Part1Ans()
    {
      return this.PuzzleInput
        .Select(int.Parse)
        .Select(x => x / 3 - 2)
        .Sum();
    }

    public override string Part2() => this.Part2Ans().ToString();

    private int Part2Ans()
    {
      int sum = 0;
      var masses = this.PuzzleInput
        .Select(int.Parse);
      do
      {
        masses = masses
          .Select(x => x / 3 - 2)
          .Where(x => x > 0);
        sum += masses.Sum();
      }
      while (masses.Any());
      return sum;
    }
  }
}