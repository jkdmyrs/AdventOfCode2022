var timer = System.Diagnostics.Stopwatch.StartNew();
var (day, part, solve, session) = CliParser.Parse(args);

Console.WriteLine($"AOC 2022");
var display1 = part.HasValue ? $" {part.Value}" : "s 1+2";
var display2 = solve ? "Real Input" : "Practice Input";
Console.WriteLine($"Day {day}, Part{display1} -  {display2}");

await PuzzleRunner.Run((day, part, solve, session)).ConfigureAwait(false);
timer.Stop();
Console.WriteLine($"@Total Time: {timer.ElapsedMilliseconds}ms".Replace("@", Environment.NewLine));