namespace AOC.Year_2019
{
  internal class Day5 : PuzzleBase
  {
    private static int _year = 2019;

    public Day5(bool part1, bool part2, bool isPractice)
      : base(_year, int.Parse(nameof(Day5).Substring(3)), part1, part2, isPractice)
    {
      _program = new int[0];
    }

    public override string Part1() => Part1Ans().ToString();

    public override string Part2() => Part2Ans().ToString();

    private int[] _program;

    int GetValue(char mode, int value)
    {
      return mode == '0' ? _program[value] : value;
    }

    private string RunProgram(int input, bool part2 = false)
    {
      int pc = 0;
      string outputBuffer = string.Empty;
      while (_program[pc] != 99)
      {
        var opCode = _program[pc].ToString().PadLeft(5, '0');
        var modes = string.Join(string.Empty, opCode.Substring(0, opCode.Length - 2).ToArray().Reverse()).ToArray();
        int left, right, dest, param;
        switch (int.Parse(opCode.Substring(opCode.Length - 2)))
        {
          case 1:
            left = GetValue(modes[0], _program[pc + 1]);
            right = GetValue(modes[1], _program[pc + 2]);
            dest = _program[pc + 3];
            _program[dest] = left + right;
            pc+=4;
            break;
          case 2:
            left = GetValue(modes[0], _program[pc + 1]);
            right = GetValue(modes[1], _program[pc + 2]);
            dest = _program[pc + 3];
            _program[dest] = left * right;
            pc+=4;
            break;
          case 3:
            param = _program[pc + 1];
            _program[param] = input;
            pc += 2;
            break;
          case 4:
            param = GetValue(modes[0], _program[pc + 1]);
            Console.WriteLine(param);
            outputBuffer = param.ToString();
            pc += 2;
            break;
          case 5:
            if (part2)
            {
              left = GetValue(modes[0], _program[pc + 1]);
              right = GetValue(modes[1], _program[pc + 2]);
              if (left != 0)
              {
                pc = right;
              }
              else
              {
                pc += 3;
              }
            }
            break;
          case 6:
            if (part2)
            {
              left = GetValue(modes[0], _program[pc + 1]);
              right = GetValue(modes[1], _program[pc + 2]);
              if (left == 0)
              {
                pc = right;
              }
              else
              {
                pc += 3;
              }
            }
            break;
          case 7:
            if (part2)
            {
              left = GetValue(modes[0], _program[pc + 1]);
              right = GetValue(modes[1], _program[pc + 2]);
              dest = _program[pc + 3];
              _program[dest] = left < right ? 1 : 0;
              pc += 4;
            }
            break;
          case 8:
            if (part2)
            {
              left = GetValue(modes[0], _program[pc + 1]);
              right = GetValue(modes[1], _program[pc + 2]);
              dest = _program[pc + 3];
              _program[dest] = left == right ? 1 : 0;
              pc += 4;
            }
            break;
          default:
            throw new Exception("invalid opcode");
        }
      }
      return outputBuffer;
    }

    private void ResetProgram()
    {
      _program = PuzzleInput.First().Split(',').Select(int.Parse).ToArray();
    }

    private string Part1Ans()
    {
      ResetProgram();

      return RunProgram(1);
    }

    private string Part2Ans()
    {
      ResetProgram();

      return RunProgram(5, true);
    }
  }
}