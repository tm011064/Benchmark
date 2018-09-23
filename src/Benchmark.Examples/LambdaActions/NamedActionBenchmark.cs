using System.Text;

namespace Benchmark.Examples.LambdaActions
{
  public static class NamedActionBenchmark
  {
    public static void StringBuilder()
    {
      var text = new StringBuilder();

      for (var i = 0; i < 5; i++)
      {
        text.Append(' ');
      }

      ObservableObject.Observe(text.ToString());
    }

    public static void Concatenate()
    {
      var text = string.Empty;

      for (var i = 0; i < 5; i++)
      {
        text += ' ';
      }

      ObservableObject.Observe(text);
    }

    public static BenchmarkReport Run()
    {
      return Measure
        .Candidates(
          ("Five Concatenations", Concatenate),
          ("Five String Builder Appends", StringBuilder))
        .WithNumberOfRuns(1000)
        .WithNumberOfDryRuns(10)
        .Go();
    }
  }
}
