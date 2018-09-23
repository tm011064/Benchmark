namespace Benchmark.Examples.BenchmarkCandidatesWithoutContext
{
  public class ConcatenateStringsCandidate : IBenchmarkCandidate
  {
    public string Name { get; } = "Concatenate-Strings";

    public void Run()
    {
      var text = string.Empty;

      for (var i = 0; i < 100; i++)
      {
        text += ' ';
      }

      ObservableObject.Observe(text);
    }
  }
}
