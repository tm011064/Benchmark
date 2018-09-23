using System;

namespace Benchmark
{
  public class BenchmarkMetrics
  {
    public BenchmarkMetrics(
      string candidateName,
      TimeSpan elapsed,
      TimeSpan averageElapsedTicks,
      TimeSpan medianElapsedTicks)
    {
      CandidateName = candidateName;
      TotalElapsed = elapsed;
      AverageElapsed = averageElapsedTicks;
      MedianElapsed = medianElapsedTicks;
    }

    public string CandidateName { get; }

    public TimeSpan TotalElapsed { get; }

    public TimeSpan AverageElapsed { get; }

    public TimeSpan MedianElapsed { get; }
  }
}
