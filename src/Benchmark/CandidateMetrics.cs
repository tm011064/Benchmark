using System;
using System.Linq;

namespace Benchmark
{
  public class CandidateMetrics
  {
    public static CandidateMetrics Create<TContext>(
      IBenchmarkCandidate<TContext> candidate,
      TContext context,
      int numberOfRuns,
      TimeSpan[] durations)
      where TContext : class, IBenchmarkContext
    {
      var elapsedTicks = durations.Sum(x => x.Ticks);

      var benchmarkComment = candidate as IBenchmarkComment;

      return new CandidateMetrics(
        candidate.Name,
        TimeSpan.FromTicks(elapsedTicks),
        TimeSpan.FromTicks((long)Math.Round((decimal)elapsedTicks / numberOfRuns)),
        durations.OrderBy(x => x.Ticks).ElementAt(durations.Count() / 2),
        benchmarkComment?.GetComment(context),
        numberOfRuns);
    }

    private CandidateMetrics(
      string candidateName,
      TimeSpan elapsed,
      TimeSpan averageElapsedTicks,
      TimeSpan medianElapsedTicks,
      string comment,
      int numberOfRuns)
    {
      CandidateName = candidateName;
      TotalElapsed = elapsed;
      AverageElapsed = averageElapsedTicks;
      MedianElapsed = medianElapsedTicks;
      Comment = comment;
      NumberOfRuns = numberOfRuns;
    }

    public string CandidateName { get; }

    public TimeSpan TotalElapsed { get; }

    public TimeSpan AverageElapsed { get; }

    public TimeSpan MedianElapsed { get; }

    public string Comment { get; }

    public int NumberOfRuns { get; }

    public bool HasComment()
    {
      return !string.IsNullOrWhiteSpace(Comment);
    }
  }
}
