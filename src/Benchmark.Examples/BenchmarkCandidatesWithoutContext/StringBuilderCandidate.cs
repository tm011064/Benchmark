using System.Text;

namespace Benchmark.Examples.BenchmarkCandidatesWithoutContext
{
  public class StringBuilderCandidate : IBenchmarkCandidate
  {
    public string Name { get; } = "StringBuilder";

    public void Run()
    {
      var text = new StringBuilder();

      for (var i = 0; i < 100; i++)
      {
        text.Append(' ');
      }

      ObservableObject.Observe(text.ToString());
    }
  }
}
