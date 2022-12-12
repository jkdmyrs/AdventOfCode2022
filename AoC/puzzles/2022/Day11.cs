using System.Linq.Expressions;

namespace AOC.Year_2022
{
  internal class Day11 : PuzzleBase
  {
    private static int _year = 2022;

    public Day11(bool part1, bool part2, bool isPractice)
      : base(_year, int.Parse(nameof(Day11).Substring(3)), part1, part2, isPractice)
    {
    }

    public override string Part1() => this.Part1Ans().ToString();

    private class Monkey
    {
      private int _inspectionCount = 0;

      public Monkey(List<long> items, Func<long, long> operation, Func<long, bool> test, int ifTrue, int ifFalse)
      {
        Items = items;
        Operation = operation;
        Test = test;
        IfTrue = ifTrue;
        IfFalse = ifFalse;
      }

      public List<long> Items { get; set; }
      public Func<long, long> Operation { get; set; }
      public Func<long, bool> Test { get; set; }
      public int IfTrue { get; set; }
      public int IfFalse { get; set; }
      public int InspectionCount => _inspectionCount;

      public void BeAMonkey(Monkey[] monkeys, bool part2 = false, int supermodulus = 0)
      {
        foreach (var item in this.Items)
        {
          _inspectionCount++;
          var inspected = this.Operation(item);

          if (part2)
          {
            inspected = inspected % supermodulus;
          }
          else
          {
            inspected = inspected / 3;
          }

          if (this.Test(inspected))
          {
            monkeys[this.IfTrue].Items.Add(inspected);
          }
          else
          {
            monkeys[this.IfFalse].Items.Add(inspected);
          }
        }
        this.Items.Clear();
      }
    }

    private int Part1Ans()
    {
      var monkeys = ParseMonkeys();

      for (int j = 0; j < 20; j++)
      {
        for (int i = 0; i < monkeys.Length; i++)
        {
          var monkey = monkeys[i];
          monkey.BeAMonkey(monkeys);
        }
      }

      var max = monkeys.Max(x => x.InspectionCount);
      var max2 = monkeys.Where(x => x.InspectionCount != max).Max(x => x.InspectionCount);
      return max * max2;
    }

    public override string Part2() => this.Part2Ans().ToString();

    /// <summary>
    /// Was really not a fan of this puzzle. I had to look at the reddit
    /// thread to get the idea of how to solve it... 
    /// 
    /// It seems that most people hard coded and/or visually inspected the input 
    /// to notice that all the divisors were co-prime, which based on my limited
    /// understanding, is required for the supermodulus trick. 
    /// 
    /// I tend to ALWAYS write a parser for the input, and almost never visually
    /// inspect it; I feel that any puzzle where this is necessary is an inherently
    /// poorly designed puzzle.
    /// </summary>
    private long Part2Ans()
    {
      var monkeys = ParseMonkeys();

      var supermoduluslist = this.PuzzleInput.Where(x => x.Contains("divisible by")).Select(x => int.Parse(x.Split(' ').Last()));
      var supermodulus = supermoduluslist.Aggregate((x, y) => x * y);

      for (int j = 0; j < 10000; j++)
      {
        for (int i = 0; i < monkeys.Length; i++)
        {
          var monkey = monkeys[i];
          monkey.BeAMonkey(monkeys, true, supermodulus);
        }
      }

      var max = monkeys.Max(x => x.InspectionCount);
      var max2 = monkeys.Where(x => x.InspectionCount != max).Max(x => x.InspectionCount);
      return (long)max * (long)max2;
    }

    #region ParseMonkeys
    private Monkey[] ParseMonkeys()
    {
      var monkeyCount = this.PuzzleInput.Where(x => x.Contains("Monkey")).Count();
      var monkeys = new Monkey[monkeyCount];

      var input = new List<string>(this.PuzzleInput);
      for (int i = 0; i < monkeyCount; i++)
      {
        var details = input.Skip(1).Take(5).ToArray();
        monkeys[i] = new Monkey(
          this.GetItems(details[0]),
          this.GetOperation(details[1]),
          this.GetTest(details[2]),
          this.GetAction(details[3]),
          this.GetAction(details[4])
        );
        input = input.Skip(7).ToList();
      }
      return monkeys;
    }

    private List<long> GetItems(string items)
    {
      return items.Split(':').Last().Split(',').Select(x => long.Parse(x)).ToList();
    }

    private Func<long, long> GetOperation(string operation)
    {
      var op = operation.Split(':').Last();
      var expression = op.Split('=').Last();
      return ParseExpression(expression);
    }

    private Func<long, long> ParseExpression(string expression)
    {
      var split = expression.Trim().Split(' ');
      var op = split.Skip(1).First();
      var param = Expression.Parameter(typeof(long), "old");
      var left = split.First();
      var right = split.Last();

      switch (op)
      {
        case "+":
          var add = Expression.Add(left == "old" ? param : Expression.Constant(long.Parse(left)), right == "old" ? param : Expression.Constant(long.Parse(right)));
          return Expression.Lambda<Func<long, long>>(add, param).Compile();
        case "*":
          var mul = Expression.Multiply(left == "old" ? param : Expression.Constant(long.Parse(left)), right == "old" ? param : Expression.Constant(long.Parse(right)));
          return Expression.Lambda<Func<long, long>>(mul, param).Compile();
      }
      throw new Exception("unexpeted.");
    }

    private Func<long, bool> GetTest(string test)
    {
      var i = long.Parse(test.Split(':').Last().Split(' ').Last());
      return x => x % i == 0;
    }

    private int GetAction(string action)
    {
      return int.Parse(action.Split(':').Last().Split(' ').Last());
    }
    #endregion
  }
}