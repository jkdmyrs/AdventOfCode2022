namespace AOC.Year_2022
{
  using Point = Tuple<int, int>;
  using Corner = Tuple<Tuple<int, int>, Tuple<int, int>, Tuple<int, int>, Tuple<int, int>>;

  internal class Day15 : PuzzleBase
  {
    private static int _year = 2022;
    public Day15(bool part1, bool part2, bool isPractice)
      : base(_year, int.Parse(nameof(Day15).Substring(3)), part1, part2, isPractice)
    {
      this._scanDepth = isPractice ? 10 : 2000000;
    }

    public override string Part1() => this.Part1Ans().ToString();
    public override string Part2() => this.Part2Ans().ToString();

    private readonly int _scanDepth;

    private (Corner, Point, int) FindCorners(Point p, int r)
    {
      var corners = new List<Point>();
      for (int i = 0; i < 4; i++)
      {
        var x = p.Item1 + (i % 2 == 0 ? r : -r);
        var y = p.Item2 + (i % 2 == 0 ? -r : r);
        corners.Add(new Point(x, y));
      }
      var c = corners.ToArray();
      return (new Corner(c[0], c[1], c[2], c[3]), p, r);
    }

    private Point[] GetBall(Point p, int r)
    {
      var ball = new List<Point>();
      for (int x = p.Item1 - r; x <= p.Item1 + r; x++)
      {
        if (this.TaxicabDistance(p, new Point(x, _scanDepth)) <= r)
        {
          ball.Add(new Point(x, _scanDepth));
        }
      }
      return ball.ToArray();
    }

    private int TaxicabDistance(Point p1, Point p2)
    {
      return Math.Abs(p1.Item1 - p2.Item1) + Math.Abs(p1.Item2 - p2.Item2);
    }

    private (Point sensor, Point Beacon)[] ParseInput()
    {
      var parsed = new List<(Point sensor, Point Beacon)>();
      this.PuzzleInput.ForEach(line =>
      {
        var split = line.Split(':');
        var sensor = split[0];
        var splitSensor = sensor.Split(',');
        var sx = int.Parse(splitSensor[0].Split('=').Last());
        var sy = int.Parse(splitSensor[1].Split('=').Last());

        var beacon = split[1];
        var splitBeacon = beacon.Split(',');
        var bx = int.Parse(splitBeacon[0].Split('=').Last());
        var by = int.Parse(splitBeacon[1].Split('=').Last());

        parsed.Add((new Point(sx, sy), new Point(bx, by)));
      });
      return parsed.ToArray();
    }

    private int Part1Ans()
    {
      var scans = this.ParseInput().ToList();
      List<(Corner, Point, int)> groupedCorners = new();
      List<Point> knownPoints = new();

      scans.ForEach(scan =>
      {
        knownPoints.Add(scan.sensor);
        knownPoints.Add(scan.Beacon);
        int r = this.TaxicabDistance(scan.sensor, scan.Beacon);
        groupedCorners.Add(FindCorners(scan.sensor, r));
      });

      List<Point> notBeacon = new();
      foreach (var group in groupedCorners)
      {
        var (corners, sensor, r) = group;
        Point[] arr = new Point[4];
        arr[0] = corners.Item1;
        arr[1] = corners.Item2;
        arr[2] = corners.Item3;
        arr[3] = corners.Item4;

        bool spotAboveOrAt = arr.Any(x => x.Item2 >= this._scanDepth);
        bool spotBelowOrAt = arr.Any(x => x.Item2 <= this._scanDepth);

        if (spotAboveOrAt || spotBelowOrAt)
        {
          var ball = this.GetBall(sensor, r);
          notBeacon.AddRange(ball);
        }
      }

      return notBeacon.Except(knownPoints).ToHashSet().Count;
    }

    private int Part2Ans()
    {
      return 0;
    }
  }
}