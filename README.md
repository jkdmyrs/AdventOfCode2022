# jkdmyrs - Advent of Code C#

## Overview
Advent of Code puzzle runner, input downloader, stat collector... in C#!

C# might not be the favorite language of many of the "speed hackers" out there, but I find that with LINQ, the `Enumerable` class, tuples, and a few other various C# features, you can find the solution to many AoC problems without too much fussing. 

This repo provides me with a Puzzle base class, with which I implement a Puzzle class for each day; the implementation includes methods which return the answers to parts 1 and 2. The repo also provides a mechanism with which to run the puzzles, and a mechanism to interface with the program via CLI args (documented below).

## Advent of Code Session Token

In order to run the puzzles, on the CLI or VisualStudio/VS Code, you must aquire your Advent of Code Session Cookie. You can find this in the browser dev tools... Inspect a network request made to `https://adventofcode.com/` and grab the `session` value from the `cookie` header.

Set this session token as an environment variable: `AOC_SESSION`, or pass it into the program as a CLI arg with the `-s` flag:

```
-s TOKEN_VALUE_HERE
```

## running the puzzles

The repo includes 2 scripts:

- `practice.sh X` - solve the `DayX` puzzle against `input/practice/X.txt`
- `solve.sh X` - solve the `DayX` puzzle aginst `input/X.txt`, and create `results/dayX.txt`