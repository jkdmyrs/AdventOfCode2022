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

    private class PacketComparer : IComparer<List<PacketValue>>
    {
      public int Compare(List<PacketValue>? x, List<PacketValue>? y)
      {
        if (x == null)
          throw new ArgumentNullException(nameof(x));
        if (y == null)
          throw new ArgumentNullException(nameof(y));

        var inOrder = IsInOrder(x, y);
        return inOrder ? -1 : 1;
      }
    }

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

    private void AddEmptyListPacketAtDepth(PacketValue topLevel, int depth)
    {
      var newPacket = new PacketValue();
      newPacket.Type = PacketValueType.LIST;

      var packet = topLevel;
      for (int i = 0; i < depth; i++)
      {
        packet = packet.List.Last();
      }
      packet.List.Add(newPacket);
    }

    private void AddValuePacketAtDepth(PacketValue topLevel, int depth, int value)
    {
      var newPacket = new PacketValue();
      newPacket.Type = PacketValueType.INT;
      newPacket.Value = value;

      var packet = topLevel;
      for (int i = 0; i < depth; i++)
      {
        packet = packet.List.Last();
      }
      packet.List.Add(newPacket);
    }

    private List<List<PacketValue>> ParsePackets(List<List<PacketValue>>? packetsToAdd = null)
    {
      var packets = new List<List<PacketValue>>();
      this.PuzzleInput.Where(x => !string.IsNullOrWhiteSpace(x)).ToList().ForEach(line =>
      {
        line = line.Substring(1, line.Length - 2);
        var packet = new List<PacketValue>();
        var packetValue = new PacketValue();
        List<char> openChars = new List<char>();
        for (int i = 0; i < line.Length; i++)
        {
          var c = line[i];

          if (c == '[')
          {
            AddEmptyListPacketAtDepth(packetValue, openChars.Count);
            openChars.Add(c);
          }
          else if (c == ']')
          {
            openChars.RemoveAt(openChars.Count - 1);
            if (!openChars.Any())
            {
              packet.Add(packetValue);
              packetValue = new();
            }
          }
          else if (c == ',')
          {
            continue;
          }
          else
          {
            var count = 0;
            int j = i;
            while (i + 1 < line.Length && int.TryParse(line[j + 1].ToString(), out int test))
            {
              j++;
              count++;
            }
            var value = int.Parse(line.Substring(i, count + 1));
            i += count;

            // if openChars.Any, then we are in a list
            // create a packetValue with type list
            if (openChars.Any())
            {
              AddValuePacketAtDepth(packetValue, openChars.Count, value);
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
      return packets.Concat(packetsToAdd ?? new List<List<PacketValue>>()).ToList();
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

    private static bool? IsInOrder(PacketValue left, PacketValue right)
    {
      if (left.Type == PacketValueType.LIST && !left.List.Any())
      {
        if (right.Type == PacketValueType.LIST && !right.List.Any())
          return null;
        return true;
      }
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
        for (int i = 0; i < Math.Max(left.List.Count, right.List.Count); i++)
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

    private static bool IsInOrder(List<PacketValue> a, List<PacketValue> b)
    {
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
        return true;
      }
      return false;
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
        if (IsInOrder(a, b))
        {
          indexes.Add(i);
        }
        i++;
      });
      return indexes.Sum();
    }

    private int Part2Ans()
    {
      var div2Packet = new List<PacketValue> { new PacketValue { Type = PacketValueType.LIST, List = new List<PacketValue> { new PacketValue { Type = PacketValueType.INT, Value = 2 } } } };
      var div6Packet = new List<PacketValue> { new PacketValue { Type = PacketValueType.LIST, List = new List<PacketValue> { new PacketValue { Type = PacketValueType.INT, Value = 6 } } } };
      var packets = ParsePackets(new List<List<PacketValue>> { div2Packet, div6Packet }).ToArray();
      Array.Sort(packets, new PacketComparer());
      var sorted = packets.ToList();
      var div2Index = sorted.IndexOf(div2Packet) + 1;
      var div6Index = sorted.IndexOf(div6Packet) + 1;
      return div2Index * div6Index;
    }
  }
}