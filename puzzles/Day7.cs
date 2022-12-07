internal class Day7 : Puzzle
{
  public Day7(bool part1, bool part2, bool isPractice)
    : base(int.Parse(nameof(Day7).Substring(3)), part1, part2, isPractice)
  {
  }

  private class LinkedDirectory
  {
    public string Name { get; set; }
    public List<(int size, string fileName)> Contents { get; set; }
    public LinkedDirectory? Parent { get; set; }
    public List<LinkedDirectory> SubDirectories { get; set; }

    public int Size
    {
      get
      {
        int size = 0;
        foreach (var content in this.Contents)
        {
          size += content.size;
        }
        foreach (var subDir in this.SubDirectories)
        {
          size += subDir.Size;
        }
        return size;
      }
    }

    public LinkedDirectory(string name)
    {
      this.Name = name;
      this.SubDirectories = new List<LinkedDirectory>();
      this.Contents = new List<(int size, string fileName)>();
      this.Parent = null;
    }

    public void SetParent(LinkedDirectory parent)
    {
      this.Parent = parent;
    }

    public void AddSubDirectory(LinkedDirectory child)
    {
      this.SubDirectories.Add(child);
    }

    public void AddContents(int size, string fileName)
    {
      this.Contents.Add((size, fileName));
    }
  }

  private int GetDirectoryIndex(IEnumerable<string> lines)
  {
    int directoryIndex = -2;
    var enumerator = lines.GetEnumerator();
    while (enumerator.MoveNext())
    {
      var line = enumerator.Current;
      if (!line.StartsWith("$"))
      {
        return directoryIndex;
      }
      directoryIndex++;
    }
    throw new Exception("Invalid");
  }

  private IEnumerable<string> GetContents(IEnumerable<string> lines)
  {
    var subLines = lines.ToArray()[(GetDirectoryIndex(lines) + 2)..].ToList();
    var enumerator = subLines.GetEnumerator();
    // count how many rows until we find a $
    int count = 0;
    while (enumerator.MoveNext())
    {
      if (enumerator.Current.StartsWith("$"))
      {
        break;
      }
      count++;
    }
    return subLines.Take(count);
  }

  private LinkedDirectory GetDirectory(List<string> lines)
  {
    var index = GetDirectoryIndex(lines);
    string directoryName = lines[index].Split(' ')[2];
    var contents = GetContents(lines);
    LinkedDirectory rootDir = new LinkedDirectory(directoryName);
    foreach (var content in contents)
    {
      if (content.StartsWith("dir"))
      {
        var childDir = new LinkedDirectory(content.Split(' ')[1]);
        childDir.SetParent(rootDir);
        rootDir.AddSubDirectory(childDir);
      }
      else
      {
        var size = int.Parse(content.Split(' ')[0]);
        var fileName = content.Split(' ')[1];
        rootDir.AddContents(size, fileName);
      }
    }
    return rootDir;
  }

  private LinkedDirectory Parse()
  {
    var rootDir = GetDirectory(this.PuzzleInput);
    List<LinkedDirectory> directories = new();
    var lines = this.PuzzleInput.ToArray()[(GetDirectoryIndex(this.PuzzleInput) + 2 + rootDir.Contents.Count + rootDir.SubDirectories.Count)..].ToList();
    do
    {
      var dir = GetDirectory(lines);
      directories.Add(dir);
      lines = lines.ToArray()[(GetDirectoryIndex(lines) + 2 + dir.Contents.Count + dir.SubDirectories.Count)..].ToList();
    }
    while (lines.Any());

    BuildDirectoryTree(rootDir, directories);

    return rootDir;
  }

  private void BuildDirectoryTree(LinkedDirectory dir, List<LinkedDirectory> dirs)
  {
    for (int i = 0; i < dir.SubDirectories.Count; i++)
    {
      var subDir = dir.SubDirectories[i];
      subDir = dirs.First(d => d.Name == subDir.Name);
      var indexOf = dirs.IndexOf(subDir);
      dirs.RemoveAt(indexOf);
      BuildDirectoryTree(subDir, dirs);
      subDir.SetParent(dir);
      dir.SubDirectories[i] = subDir;
    }
  }

  public override string Part1() => this.Part1Ans().ToString();

  private int Part1Ans()
  {
    var root = Parse();
    List<int> sizes = new();

    void Traverse(LinkedDirectory dir)
    {
      if (dir.Size <= 100000)
      {
        sizes.Add(dir.Size);
      }
      foreach (var subDir in dir.SubDirectories)
      {
        Traverse(subDir);
      }
    }

    Traverse(root);

    return sizes.Sum();
  }

  public override string Part2() => this.Part2Ans().ToString();

  private int Part2Ans()
  {
    var root = Parse();
    int unused = 70000000 - root.Size;
    var freeUp = 30000000 - unused;

    List<int> sizes = new();

    void Traverse(LinkedDirectory dir)
    {
      if (dir.Size >= freeUp)
      {
        sizes.Add(dir.Size);
      }
      foreach (var subDir in dir.SubDirectories)
      {
        Traverse(subDir);
      }
    }

    Traverse(root);

    return sizes.Min();
  }
}