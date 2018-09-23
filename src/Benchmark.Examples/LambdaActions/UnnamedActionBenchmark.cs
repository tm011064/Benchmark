using System.Text;

namespace Benchmark.Examples.LambdaActions
{
  public static class UnnamedActionBenchmark
  {
    public static void StringBuilder()
    {
      var text = new StringBuilder();

      for (var i = 0; i < 100; i++)
      {
        text.Append(' ');
      }

      ObservableObject.Observe(text.ToString());
    }

    public static void Concatenate()
    {
      var text = string.Empty;

      for (var i = 0; i < 100; i++)
      {
        text += ' ';
      }

      ObservableObject.Observe(text);
    }

    public static BenchmarkReport Run()
    {
      return Measure
        .Candidates(
          Concatenate,
          StringBuilder)
        .WithNumberOfRuns(1000)
        .WithNumberOfDryRuns(100)
        .Go();
    }
  }
}
