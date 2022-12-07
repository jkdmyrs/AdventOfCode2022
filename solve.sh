YEAR=2022
(dotnet run --project AoC/AoC.csproj -- -s $AOC_SESSION -d $1 -a -y $YEAR) 2>&1 | tee "./results/$YEAR/day$1.txt"