using System;
using System.Linq;

namespace Benchmark.BuilderSteps
{
  internal class CandidateRunnerWithContextArgs<TContext> :
    IWithBenchmarkTestContextsStep<TContext>,
    IWithNumberOfRunsWithContextStep<TContext>
    where TContext : class, IBenchmarkContext
  {
    public CandidateRunnerWithContextArgs(IBenchmarkCandidate<TContext>[] candidates)
    {
      Candidates = candidates ?? throw new ArgumentNullException(nameof(candidates));
    }

    public IBenchmarkCandidate<TContext>[] Candidates { get; }

    public TContext[] BenchmarkTestContexts { get; private set; }

    public int? NumberOfRuns { get; private set; }

    public TimeSpan DurationPerContext { get; private set; } = TimeSpan.FromSeconds(1);

    public int NumberOfWarmUpRuns { get; private set; }

    public TContext WarmUpRunBenchmarkTestContext { get; private set; }

    public bool HasNumberOfRuns()
    {
      return NumberOfRuns.HasValue;
    }

    public IWithNumberOfRunsWithContextStep<TContext> WithContexts(params TContext[] contexts)
    {
      BenchmarkTestContexts = contexts;
      return this;
    }

    public IWithNumberOfWarmUpRunsWithContextStep<TContext> WithNumberOfRuns(int numberOfRuns)
    {
      NumberOfRuns = numberOfRuns;
      return this;
    }

    public IWithNumberOfWarmUpRunsWithContextStep<TContext> WithTestDurationPerContext(TimeSpan duration)
    {
      DurationPerContext = duration;
      return this;
    }

    public IGoStep<TContext> WithNumberOfWarmUpRuns(int numberOfRuns, TContext WarmUpRunBenchmarkTestContext = null)
    {
      NumberOfWarmUpRuns = numberOfRuns;
      return this;
    }

    public BenchmarkReport Go()
    {
      if (Candidates?.Any() != true)
      {
        throw new ArgumentException($"At least one {nameof(IBenchmarkCandidate<TContext>)} must be provided");
      }

      if (BenchmarkTestContexts?.Any() != true)
      {
        throw new ArgumentException($"At least one {nameof(TContext)} must be provided");
      }

      if (NumberOfRuns < 1)
      {
        throw new ArgumentException($"{nameof(NumberOfRuns)} must be greater than zero");
      }

      return new CandidateRunner<TContext>(this).Run();
    }
  }
}
