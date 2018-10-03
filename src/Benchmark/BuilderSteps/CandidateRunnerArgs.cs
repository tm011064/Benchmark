using System;
using System.Linq;

namespace Benchmark.BuilderSteps
{
  internal class CandidateRunnerArgs : IWithNumberOfRunsStep
  {
    public CandidateRunnerArgs(IBenchmarkCandidate[] candidates)
    {
      Candidates = candidates ?? throw new ArgumentNullException(nameof(candidates));
    }

    public IBenchmarkCandidate[] Candidates { get; }

    public int? NumberOfRuns { get; private set; }

    public TimeSpan DurationPerContext { get; private set; } = TimeSpan.FromSeconds(1);

    public int NumberOfWarmUpRuns { get; private set; }

    public IWithNumberOfWarmUpRunsStep WithNumberOfRuns(int numberOfRuns)
    {
      NumberOfRuns = numberOfRuns;
      return this;
    }

    public IWithNumberOfWarmUpRunsStep WithTestDurationPerContext(TimeSpan duration)
    {
      DurationPerContext = duration;
      return this;
    }

    public IGoStep WithNumberOfWarmUpRuns(int numberOfRuns)
    {
      NumberOfWarmUpRuns = numberOfRuns;
      return this;
    }

    public BenchmarkReport Go()
    {
      var candidates = Candidates.Select(x => new BenchmarkCandidateNullContextWrapper(x));

      var builder = Measure<NullBenchmarkContext>
        .Candidates(candidates.ToArray())
        .WithContexts(NullBenchmarkContext.Instance);

      if (NumberOfRuns.HasValue)
      {
        return builder
          .WithNumberOfRuns(NumberOfRuns.Value)
          .WithNumberOfWarmUpRuns(NumberOfWarmUpRuns)
          .Go();
      }

      return builder
        .WithTestDurationPerContext(DurationPerContext)
        .Go();
    }
  }
}
