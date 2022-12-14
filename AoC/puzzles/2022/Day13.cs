namespace AOC.Year_2022
{
  using PacketGroup = Tuple<List<Day13.PacketValue>, List<Day13.PacketValue>>;
  internal class Day13 : PuzzleBase
  {
    private static int _year = 2022;
    public Day13(bool part1, bool part2, bool isPractice)
      : base(_year, int.Parse(nameof(Day13).Substring(3)), part1, part2, isPractice) { }

    public override string Part1() => this.Part1Ans().ToString();
    public override string Part2() => this.Part2Ans().ToString();

    internal class PacketValue
    {
      public PacketValueType Type { get; set; }
      public int Value { get; set; }
      public List<PacketValue> List { get; set; } = new();
    }

    internal enum PacketValueType
    {
      LIST,
      INT
    };

    private List<List<PacketValue>> ParsePackets()
    {
      var packets = new List<List<PacketValue>>();
      this.PuzzleInput.Where(x => !string.IsNullOrWhiteSpace(x)).ToList().ForEach(line =>
      {
        line = line.Substring(1, line.Length - 2);
        var packet = new List<PacketValue>();
        var packetValue = new PacketValue();
        List<char> openChars = new List<char>();
        foreach (var c in line)
        {
          if (c == '[')
          {
            openChars.Add(c);
          }
          else if (c == ']')
          {
            packet.Add(packetValue);
            packetValue = new();
            openChars.RemoveAt(openChars.Count - 1);
          }
          else if (c == ',')
          {
            continue;
          }
          else
          {
            // if openChars.Any, then we are in a list
            // create a packetValue with type list
            if (openChars.Any())
            {
              packetValue.Type = PacketValueType.LIST;
              var subVal = new PacketValue();
              subVal.Type = PacketValueType.INT;
              subVal.Value = int.Parse(c.ToString());
              packetValue.List.Add(subVal);
            }
            else
            {
              packetValue.Type = PacketValueType.INT;
              packetValue.Value = int.Parse(c.ToString());
              packet.Add(packetValue);
              packetValue = new();
            }
          }
        }
        packets.Add(packet);
      });
      return packets;
    }

    private List<PacketGroup> BuildGroups(IEnumerable<List<PacketValue>> packets)
    {
      var groups = new List<PacketGroup>();
      while (packets.Any())
      {
        groups.Add(new PacketGroup(packets.ElementAt(0), packets.ElementAt(1)));
        packets = packets.Skip(2);
      }
      return groups;
    }

    private bool? IsInOrder(PacketValue left, PacketValue right)
    {
      if (left.Type == PacketValueType.INT && right.Type == PacketValueType.INT)
      {
        if (left.Value > right.Value)
        {
          return false;
        }
        else if (left.Value < right.Value)
        {
          return true;
        }
        else
        {
          return null;
        }
      }
      else if (left.Type == PacketValueType.LIST && right.Type == PacketValueType.LIST)
      {
        for (int i = 0; i < left.List.Count; i++)
        {
          PacketValue iLeft, iRight;
          try
          {
            iLeft = left.List[i];
          }
          catch
          {
            return true;
          }
          try
          {
            iRight = right.List[i];
          }
          catch
          {
            return false;
          }
          var inOrder = IsInOrder(iLeft, iRight);
          if (inOrder.HasValue)
          {
            return inOrder;
          }
        }
        return null;
      }
      else
      {
        if (left.Type == PacketValueType.INT)
        {
          left.Type = PacketValueType.LIST;
          left.List.Add(new PacketValue() { Type = PacketValueType.INT, Value = left.Value });
          return IsInOrder(left, right);
        }
        else
        {
          right.Type = PacketValueType.LIST;
          right.List.Add(new PacketValue() { Type = PacketValueType.INT, Value = right.Value });
          return IsInOrder(left, right);
        }
      }
    }

    private int Part1Ans()
    {
      var packets = ParsePackets();
      var groups = BuildGroups(packets);

      List<int> indexes = new();
      int i = 1;
      groups.ForEach(group =>
      {
        var (a, b) = group;
        int j = 0;
        bool? inOrder = false;
        do
        {
          PacketValue left, right;
          try
          {
            left = a[j];
          }
          catch
          {
            inOrder = true;
            break;
          }
          try
          {
            right = b[j];
          }
          catch
          {
            inOrder = false;
            break;
          }

          inOrder = IsInOrder(left, right);
          if (inOrder.HasValue)
          {
            break;
          }

          j++;
        }
        while (true);

        if (inOrder.HasValue && inOrder.Value)
        {
          indexes.Add(i);
        }
        i++;
      });
      return indexes.Sum();
    }

    private int Part2Ans()
    {
      return 0;
    }
  }
}