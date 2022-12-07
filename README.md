# jkdmyrs - Advent of Code C#

## Overview

- `AoC.Infra` - Advent of Code puzzle runner, input downloader, stat collector, etc... in C#!
- `AoC` - Puzzle solutions... in C#!

C# might not be the favorite language of many of the "speed hackers" out there, but I find that with LINQ, the `Enumerable` class, tuples, and a few other various C# features, you can find the solution to many AoC problems without too much fussing. 

Solutions in this project are my "first pass" solutions. My goal for each puzzle is to find the correct answers as quickly as possible. I am not concerened with "clean" code in this context.

I aim to eventually finish all puzzles from all previous and future years!

## Session Token

In order to run the puzzles, on the CLI or VisualStudio/VS Code, you must aquire your Advent of Code Session Cookie. You can find this in the browser dev tools... Inspect a network request made to `https://adventofcode.com/` and grab the `session` value from the `cookie` header.

Set this session token as an environment variable: `AOC_SESSION`, or pass it into the program as a CLI arg with the `-s` flag:

## running the puzzles

The repo includes 2 scripts to easily run the puzzles:

- `practice.sh X` - solve the `DayX` puzzle against `input/<year>/practice/X.txt`
- `solve.sh X` - solve the `DayX` puzzle aginst `input/<year>/X.txt`, and create `results/<year>/dayX.txt`
- `uplaod.sh X Y` - solve part `Y` of the `DayX` puzzle aginst `input/<year>/X.txt`, and attempt to submit the answer

### Cli Arguments

If debugging/running with `dotnet` directly, the following `args` can be used:

- `-d X` - **day** - the day of the puzzle to run (required)
- `-y XXXX` - **year** - the year of the puzzle to run (optional - default: current year)
- `-p X` - **part** - `1` or `2` (optional - default: both parts)
- `-a` - **solve** - solve the real puzzle input (optional - default: disabled)
- `-u` - **upload** - must be used with `-p X` - attempt to upload the answer (optional - default: disabled)