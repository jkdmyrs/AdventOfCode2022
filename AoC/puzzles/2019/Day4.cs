namespace AOC.Year_2019
{
  internal class Day4 : PuzzleBase
  {
    private static int _year = 2019;

    public Day4(bool part1, bool part2, bool isPractice)
      : base(_year, int.Parse(nameof(Day4).Substring(3)), part1, part2, isPractice)
    {
    }

    public override string Part1() => this.Part1Ans().ToString();

    private int Part1Ans()
    {
      var splitRange = this.PuzzleInput
        .First()
        .Split('-')
        .Select(int.Parse)
        .ToArray();
      var range = Enumerable.Range(splitRange[0], splitRange[1] - splitRange[0]);
      bool Rules(int x)
      {
        char[] chars = x.ToString().ToCharArray();
        bool Increases(char[] c) => c.Zip(c.Skip(1), (a, b) => a <= b).All(x => x);
        bool Adjacent(char[] c) => c.Zip(c.Skip(1), (a, b) => a == b).Any(x => x);
        return Increases(chars) && Adjacent(chars);
      }
      return range.Count(Rules);
    }

    public override string Part2() => this.Part2Ans().ToString();

    private int Part2Ans()
    {
      var splitRange = this.PuzzleInput
        .First()
        .Split('-')
        .Select(int.Parse)
        .ToArray();
      var range = Enumerable.Range(splitRange[0], splitRange[1] - splitRange[0]);
      bool Rules(int x)
      {
        bool Increases(char[] c) => c.Zip(c.Skip(1), (a, b) => a <= b).All(x => x);

        bool Adjacent(char[] c)
        {
          HashSet<char> group2 = new();
          HashSet<char> group3 = new();
          do
          {
            var part2 = c.Take(2);
            if (part2.All(x => x == c[0]) && part2.Count() == 2)
            {
              group2.Add(c[0]);
            }
            var part3 = c.Take(3);
            if (part3.All(x => x == c[0]) && part3.Count() == 3)
            {
              group3.Add(c[0]);
            }
            c = c.Skip(1).ToArray();
          }
          while (c.Any());
          return group2.Except(group3).Any();
        }

        char[] chars = x.ToString().ToCharArray();
        return Increases(chars) && Adjacent(chars);
      }
      return range.Count(Rules);
    }
  }
}