internal class Day1 : Puzzle
{
  public Day1(bool part1, bool part2, bool isPractice)
    : base(int.Parse(nameof(Day1).Substring(3)), part1, part2, isPractice)
  {
  }

  public override string Part1() => this.Part1Ans().ToString();

  private int Part1Ans()
  {
    List<int> sums = new();
    IEnumerable<string> lines = this.PuzzleInput;
    do
    {
      var emptyIndex = lines.ToList().IndexOf(string.Empty);
      sums.Add((emptyIndex == -1 ? lines : lines.Take(emptyIndex)).Select(int.Parse).Sum());
      lines = lines.Skip((emptyIndex == -1 ? 0 : emptyIndex) + 1);
    }
    while(lines.Any());
    return sums.Max();
  }

  public override string Part2() => this.Part2Ans().ToString();

  private int Part2Ans()
  {
    List<int> sums = new();
    IEnumerable<string> lines = this.PuzzleInput;
    do
    {
      var emptyIndex = lines.ToList().IndexOf(string.Empty);
      sums.Add((emptyIndex == -1 ? lines : lines.Take(emptyIndex)).Select(int.Parse).Sum());
      lines = lines.Skip((emptyIndex == -1 ? 0 : emptyIndex) + 1);
    }
    while(lines.Any());
    return sums.OrderByDescending(x => x).Take(3).Sum();
  }
}