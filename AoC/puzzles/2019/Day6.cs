namespace AOC.Year_2019
{
  internal class Day6 : PuzzleBase
  {
    private static int _year = 2019;

    public Day6(bool part1, bool part2, bool isPractice)
      : base(_year, int.Parse(nameof(Day6).Substring(3)), part1, part2, isPractice)
    {
    }

    public override string Part1() => this.Part1Ans().ToString();

    private class Orbit
    {
      public Orbit(string name, string? parentName)
      {
        Name = name;
        ParentName = parentName;
      }

      public string Name { get; set; } 
      public string? ParentName { get; set; }

      public Orbit? GetParent(IEnumerable<Orbit> orbits)
      {
        return ParentName is null ? null : orbits.First(x => x.Name == this.ParentName);
      }

      public int Size(IEnumerable<Orbit> orbits)
      {
        if (GetParent(orbits) is null)
          return 0;
        return 1 + GetParent(orbits)!.Size(orbits);
      }
    }

    private int Part1Ans()
    {
      var orbits = this.PuzzleInput.Select(x => {
        var split = x.Split(')');
        return new Orbit(split[1], split[0]);
      }).ToList();
      orbits.Add(new Orbit("COM", null));
      return orbits.Sum(x => x.Size(orbits));
    }

    public override string Part2() => this.Part2Ans().ToString();

    private int Part2Ans()
    {
      var orbits = this.PuzzleInput.Select(x => {
        var split = x.Split(')');
        return new Orbit(split[1], split[0]);
      }).ToList();
      orbits.Add(new Orbit("COM", null));

      var you = orbits.First(x => x.Name == "YOU");
      var san = orbits.First(x => x.Name == "SAN");

      var youParents = new List<Orbit>();
      var sanParents = new List<Orbit>();

      while (you is not null && you.ParentName is not null)
      {
        you = you.GetParent(orbits);
        if (you is not null)
        {
          youParents.Add(you);
        }
      }

      while (san is not null && san.ParentName is not null)
      {
        san = san.GetParent(orbits);
        if (san is not null)
        {
        sanParents.Add(san);
        }
      }

      var intersect = youParents.Intersect(sanParents).First();

      return youParents.IndexOf(intersect) + sanParents.IndexOf(intersect);
    }
  }
}