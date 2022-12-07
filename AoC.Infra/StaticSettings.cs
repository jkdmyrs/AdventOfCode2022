namespace AoC.Infra
{
  public static class StaticSettings
  {
    public static string ProjectBasePath { get; set; } = string.Empty;
    public static int Year { get; set; } = DateTime.Now.Year;
  }
}