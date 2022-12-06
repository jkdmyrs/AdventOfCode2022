internal class Day6 : Puzzle
{
  public Day6(bool part1, bool part2, bool isPractice)
    : base(int.Parse(nameof(Day6).Substring(3)), part1, part2, isPractice)
  {
  }

  public override string Part1() => this.Part1Ans().ToString();

  private int FindMarker(int length)
  {
    int i = length;
    char[] input = this.PuzzleInput.First().ToCharArray();
    bool cont = true;
    do
    {
      HashSet<char> chars = new HashSet<char>();
      input.Take(length).ToList().ForEach(c => chars.Add(c));
      if (chars.Count == length)
      {
        return i;
      }
      i++;
      input = input.Skip(1).ToArray();
    }
    while (cont);
    throw new Exception("invalid");
  }

  private int Part1Ans()
  {
    return this.FindMarker(4);
  }

  public override string Part2() => this.Part2Ans().ToString();

  private int Part2Ans()
  {
    return this.FindMarker(14);
  }
}