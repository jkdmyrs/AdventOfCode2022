internal class Day2 : Puzzle
{
  private const int win = 6;
  private const int tie = 3;
  private const int rock = 1;
  private const int paper = 2;
  private const int scissors = 3;

  public Day2(bool part1, bool part2, bool isPractice)
    : base(int.Parse(nameof(Day2).Substring(3)), part1, part2, isPractice)
  {
  }

  public override string Part1() => this.Part1Ans().ToString();

  private int Part1Ans()
  {
    int ScoreRound(string round)
    {
      int ToScore(string choice)
      {
        switch(choice) {
          default:
            case "A":
            case "X":
              return 1;
            case "B":
            case "Y":
              return 2;
            case "C": 
            case "Z":
              return 3;
            throw new NotImplementedException();
        }
      }
      var split = round.Split(' ');
      var opp = ToScore(split[0]);
      var me = ToScore(split[1]);
      if (me == opp) {
        return me + tie;
      }
      if ((me == rock && opp == scissors)
        || (me == paper && opp == rock)
        || (me == scissors && opp == paper)) {
        return me + win;
      }
      return me;
    }
    return this.PuzzleInput.Select(ScoreRound).Sum();
  }

  public override string Part2() => this.Part2Ans().ToString();

  private int Part2Ans()
  {
    int ScoreRound(string round)
    {
      int ToScore(string choice)
      {
        switch(choice) {
          default:
            case "A":
            case "X":
              return 1;
            case "B":
            case "Y":
              return 2;
            case "C": 
            case "Z":
              return 3;
            throw new NotImplementedException();
        }
      }
      int ConvertChoice(int rule, int opp)
      {
        return rule switch
        {
          1 => GetLosingMove(opp),
          2 => opp,
          3 => GetWinningMove(opp),
          _ => throw new NotImplementedException()
        };
      }
      int GetWinningMove(int choice)
      {
        return choice switch
        {
          rock => paper,
          paper => scissors,
          scissors => rock,
          _ => throw new NotImplementedException()
        };
      }
      int GetLosingMove(int choice)
      {
        return choice switch
        {
          rock => scissors,
          paper => rock,
          scissors => paper,
          _ => throw new NotImplementedException()
        };
      }
      var split = round.Split(' ');
      var opp = ToScore(split[0]);
      var me = ToScore(split[1]);
      me = ConvertChoice(me, opp);
      if (me == opp) {
        return me + tie;
      }
      if ((me == rock && opp == scissors)
        || (me == paper && opp == rock)
        || (me == scissors && opp == paper)) {
        return me + win;
      }
      return me;
    }
    return this.PuzzleInput.Select(ScoreRound).Sum();
  }
}