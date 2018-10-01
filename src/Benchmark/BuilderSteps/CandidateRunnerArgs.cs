using System;
using System.Linq;

namespace Benchmark.BuilderSteps
{
  internal class CandidateRunnerArgs :
    IWithNumberOfRunsStep,
    IWithNumberOfWarmUpRunsStep
  {
    public CandidateRunnerArgs(IBenchmarkCandidate[] candidates)
    {
      Candidates = candidates ?? throw new ArgumentNullException(nameof(candidates));
    }

    public IBenchmarkCandidate[] Candidates { get; }

    public int NumberOfRuns { get; private set; }

    public int NumberOfWarmUpRuns { get; private set; }

    public IWithNumberOfWarmUpRunsStep WithNumberOfRuns(int numberOfRuns)
    {
      NumberOfRuns = numberOfRuns;
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

      return Measure<NullBenchmarkContext>
        .Candidates(candidates.ToArray())
        .WithContexts(NullBenchmarkContext.Instance)
        .WithNumberOfRuns(NumberOfRuns)
        .WithNumberOfWarmUpRuns(NumberOfWarmUpRuns)
        .Go();
    }
  }
}
