global using AoC;
global using AoC.Infra;

var timer = System.Diagnostics.Stopwatch.StartNew();
await PuzzleRunner.Run(args.ParseAndSetupEnv()).ConfigureAwait(false);
timer.Stop();

Console.WriteLine($"@Total Time: {timer.ElapsedMilliseconds}ms".Replace("@", Environment.NewLine));

