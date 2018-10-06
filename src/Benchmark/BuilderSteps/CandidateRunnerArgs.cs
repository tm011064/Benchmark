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

    public TimeSpan? DurationPerContext { get; private set; }

    public TimeSpan? DurationPerCandidate { get; private set; }

    public int? NumberOfWarmUpRuns { get; private set; }

    public IWithNumberOfWarmUpRunsStep WithNumberOfRuns(int numberOfRuns)
    {
      NumberOfRuns = numberOfRuns;
      return this;
    }

    public IWithNumberOfWarmUpRunsStep RunEachContextFor(TimeSpan duration)
    {
      DurationPerContext = duration;
      return this;
    }

    public IWithNumberOfWarmUpRunsStep RunEachCandidateFor(TimeSpan duration)
    {
      DurationPerCandidate = duration;
      return this;
    }

    public IGoStep WithNumberOfWarmUpRuns(int numberOfRuns)
    {
      NumberOfWarmUpRuns = numberOfRuns;
      return this;
    }

    public BenchmarkReport Go()
    {
      var args = new CandidateRunnerWithContextArgs<NullBenchmarkContext>(
        Candidates.Select(x => new BenchmarkCandidateNullContextWrapper(x)).ToArray(),
        new[] { NullBenchmarkContext.Instance },
        NumberOfRuns,
        DurationPerContext,
        DurationPerCandidate,
        NumberOfWarmUpRuns,
        null);

      return args.Go();
    }
  }
}
