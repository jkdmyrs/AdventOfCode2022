(dotnet run --project AoC/AoC.csproj -- -s $AOC_SESSION -d $1 -a -y 2022) 2>&1 | tee "./results/2022/day$1.txt"