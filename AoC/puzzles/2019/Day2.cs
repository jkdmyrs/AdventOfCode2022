namespace AOC.Year_2019
{
  internal class Day2 : PuzzleBase
  {
    private static int _year = 2019;

    public Day2(bool part1, bool part2, bool isPractice)
      : base(_year, int.Parse(nameof(Day2).Substring(3)), part1, part2, isPractice)
    {
      _program = new int[0];
    }

    public override string Part1() => Part1Ans().ToString();

    public override string Part2()
    {
      (int noun, int verb) = this.Part2Ans();
      return (100 * noun + verb).ToString();
    }

    private int[] _program;

    private int RunProgram()
    {
      int i = 0;
      while (_program[i] != 99)
      {
        switch (_program[i])
        {
          case 1:
            _program[_program[i + 3]] = _program[_program[i + 1]] + _program[_program[i + 2]];
            break;
          case 2:
            _program[_program[i + 3]] = _program[_program[i + 1]] * _program[_program[i + 2]];
            break;
          default:
            throw new Exception("invalid opcode");
        }
        i += 4;
      }
      return _program[0];
    }

    private void ResetProgram()
    {
      _program = PuzzleInput.First().Split(',').Select(int.Parse).ToArray();
    }

    private int Part1Ans()
    {
      ResetProgram();

      _program[1] = 12;
      _program[2] = 2;

      return RunProgram();
    }

    private (int x, int y) Part2Ans()
    {
      for (int i = 0; i < 100; i++)
      {
        for (int j = 0; j < 100; j++)
        {
          ResetProgram();
          _program[1] = i;
          _program[2] = j;
          if (RunProgram() == 19690720)
          {
            return (i, j);
          }
        }
      }
      throw new Exception("no answer");
    }
  }
}