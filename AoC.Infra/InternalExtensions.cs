namespace AoC.Infra
{
  internal static class InternalExtensions
  {
    public static CliArg GetArg(this string strArg)
    {
      var split = strArg.Split(' ');
      if (split[0] == "a")
      {
        return new CliArg
        {
          Arg = "a"
        };
      }
      if (split[0] == "u")
      {
        return new CliArg
        {
          Arg = "u"
        };
      }
      return new CliArg
      {
        Arg = split[0],
        Val = split[1]
      };
    }

    public static (int year, int day, int? part, bool solve, bool uploadPrompt, string? session) ToOutput(this (string? year, string? day, string? part, bool solve, bool upload, string? session) val)
    {
      var (yearStr, dayStr, partStr, solve, upload, session) = val;
      return (int.TryParse(yearStr, out int year) ? year : StaticSettings.Year, int.Parse(dayStr ?? throw new ArgumentNullException(nameof(val))), int.TryParse(partStr, out int part) ? part : null, solve, upload, session);
    }

    public static void AddUserAgent(this HttpRequestMessage req)
    {
      req.Headers.TryAddWithoutValidation("User-Agent", "https://github.com/jkdmyrs/advent-of-code-csharp by jk@dmyrs.com");
    }

    public static void AddSessionToken(this HttpRequestMessage req, string session)
    {
      req.Headers.Add("cookie", $"session={session}");
    }

    public static async Task<IEnumerable<string>> GetPuzzleInput(this HttpClient httpClient, int year, int day, string session)
    {
      var request = new HttpRequestMessage(HttpMethod.Get, new Uri($"https://adventofcode.com/{year}/day/{day}/input"));
      request.AddUserAgent();
      request.AddSessionToken(session);
      var response = await new HttpClient().SendAsync(request).ConfigureAwait(false);
      response.EnsureSuccessStatusCode();
      var temp = (await response.Content.ReadAsStringAsync().ConfigureAwait(false)).Split(Environment.NewLine);
      return temp.Take(temp.Count() - 1).ToList();
    }

    public static async Task<bool> AnswerPuzzle(this HttpClient httpClient, int year, int day, int part, string answer, string session)
    {
      var request = new HttpRequestMessage(HttpMethod.Post, new Uri($"https://adventofcode.com/{year}/day/{day}/input"));
      // add json content to the request
      request.Content = new StringContent($"{{\"level\":{part},\"answer\":\"{answer}\"}}");
      request.AddUserAgent();
      request.AddSessionToken(session);
      var response = await new HttpClient().SendAsync(request).ConfigureAwait(false);
      response.EnsureSuccessStatusCode();
      var temp = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
      if (temp.Contains("That's the right answer"))
      {
        return true;
      }
      else if (temp.Contains("That's not the right answer"))
      {
        return false;
      }
      else
      {
        throw new Exception("invalid response from server");
      }
    }
  }
}