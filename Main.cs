var timer = System.Diagnostics.Stopwatch.StartNew();
var _a = CliParser.Parse(args);
var (day, part, answer) = _a;

Console.WriteLine($"AOC 2022");
var display1 = part.HasValue ? $" {part.Value}" : "s 1+2";
var display2 = answer ? "Real Input" : "Practice Input";
Console.WriteLine($"Day {day}, Part{display1} -  {display2}");

PuzzleRunner.Run(_a);
timer.Stop();
Console.WriteLine($"@Total Time: {timer.ElapsedMilliseconds}ms".Replace("@", Environment.NewLine));