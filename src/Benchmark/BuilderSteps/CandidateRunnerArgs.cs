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

    public int? FixedNumberOfRuns { get; private set; }

    public int? FixedNumberOfWarmUpRuns { get; private set; }

    public TimeSpan? DurationPerContext { get; private set; }

    public TimeSpan? DurationPerCandidate { get; private set; }

    public IWithNumberOfWarmUpRunsStep NumberOfRuns(int numberOfRuns)
    {
      FixedNumberOfRuns = numberOfRuns;
      return this;
    }

    public IWithNumberOfWarmUpRunsStep RunEachContextFor(TimeSpan duration)
    {
      DurationPerContext = duration;
      return this;
    }

    public IWithNumberOfWarmUpRunsStep RunEachContextCandidateFor(TimeSpan duration)
    {
      DurationPerCandidate = duration;
      return this;
    }

    public IGoStep NumberOfWarmUpRuns(int numberOfRuns)
    {
      FixedNumberOfWarmUpRuns = numberOfRuns;
      return this;
    }

    public BenchmarkReport Go()
    {
      var args = new CandidateRunnerWithContextArgs<NullBenchmarkContext>(
        Candidates.Select(x => new BenchmarkCandidateNullContextWrapper(x)).ToArray(),
        new[] { NullBenchmarkContext.Instance },
        FixedNumberOfRuns,
        DurationPerContext,
        DurationPerCandidate,
        FixedNumberOfWarmUpRuns,
        null);

      return args.Go();
    }
  }
}
