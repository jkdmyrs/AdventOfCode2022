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
      private int _inspectiionCount = 0;

      public Monkey(List<int> items, Func<int, int> operation, Func<int, bool> test, int ifTrue, int ifFalse)
      {
        Items = items;
        Operation = operation;
        Test = test;
        IfTrue = ifTrue;
        IfFalse = ifFalse;
      }

      public List<int> Items { get; set; }
      public Func<int, int> Operation { get; set; }
      public Func<int, bool> Test { get; set; }
      public int IfTrue { get; set; }
      public int IfFalse { get; set; }
      public int InspectionCount => _inspectiionCount;

      public void BeAMonkey(Monkey[] monkeys, bool part2 = false)
      {
        foreach (var item in this.Items)
        {
          // inspect
          _inspectiionCount++;
          var inspected = this.Operation(item);

          // get bored
          if (!part2)
          {
            inspected = inspected / 3;
          }

          // test
          var test = this.Test(inspected);

          // throw
          if (test)
          {
            monkeys[this.IfTrue].Items.Add(inspected);
          }
          else
          {
            monkeys[this.IfFalse].Items.Add(inspected);
          }
        }
        this.Items = new();
      }
    }

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

    private List<int> GetItems(string items)
    {
      return items.Split(':').Last().Split(',').Select(x => int.Parse(x)).ToList();
    }

    private Func<int, int> GetOperation(string operation)
    {
      var op = operation.Split(':').Last();
      var expression = op.Split('=').Last();
      return ParseExpression(expression);
    }

    private Func<int, int> ParseExpression(string expression)
    {
      var split = expression.Trim().Split(' ');
      var op = split.Skip(1).First();
      var param = Expression.Parameter(typeof(int), "old");
      var left = split.First();
      var right = split.Last();

      switch (op)
      {
        case "+":
          var add = Expression.Add(left == "old" ? param : Expression.Constant(int.Parse(left)), right == "old" ? param : Expression.Constant(int.Parse(right)));
          return Expression.Lambda<Func<int, int>>(add, param).Compile();
        case "*":
          var mul = Expression.Multiply(left == "old" ? param : Expression.Constant(int.Parse(left)), right == "old" ? param : Expression.Constant(int.Parse(right)));
          return Expression.Lambda<Func<int, int>>(mul, param).Compile();
      }
      throw new Exception("unexpeted.");
    }

    private Func<int, bool> GetTest(string test)
    {
      var i = int.Parse(test.Split(':').Last().Split(' ').Last());
      return x => x % i == 0;
    }

    private int GetAction(string action)
    {
      return int.Parse(action.Split(':').Last().Split(' ').Last());
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

    private int Part2Ans()
    {
      var monkeys = ParseMonkeys();

      for (int j = 0; j < 10000; j++)
      {
        for (int i = 0; i < monkeys.Length; i++)
        {
          var monkey = monkeys[i];
          monkey.BeAMonkey(monkeys, true);
        }
      }

      var max = monkeys.Max(x => x.InspectionCount);
      var max2 = monkeys.Where(x => x.InspectionCount != max).Max(x => x.InspectionCount);
      return max * max2;
    }
  }
}