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

    public CandidateRunnerWithContextArgs(
      IBenchmarkCandidate<TContext>[] candidates,
      TContext[] benchmarkTestContexts,
      int? numberOfRuns,
      TimeSpan? durationPerContext,
      TimeSpan? durationPerCandidate,
      int? numberOfWarmUpRuns,
      TContext warmUpRunBenchmarkTestContext)
    {
      BenchmarkTestContexts = benchmarkTestContexts;
      FixedNumberOfRuns = numberOfRuns;
      DurationPerContext = durationPerContext;
      DurationPerCandidate = durationPerCandidate;
      FixedNumberOfWarmUpRuns = numberOfWarmUpRuns;
      WarmUpContext = warmUpRunBenchmarkTestContext;
    }

    public IBenchmarkCandidate<TContext>[] Candidates { get; }

    public TContext[] BenchmarkTestContexts { get; private set; }

    public int? FixedNumberOfRuns { get; private set; }

    public int? FixedNumberOfWarmUpRuns { get; private set; }

    public TimeSpan? DurationPerContext { get; private set; }

    public TimeSpan? DurationPerCandidate { get; private set; }

    public TContext WarmUpContext { get; private set; }

    public bool HasNumberOfRuns()
    {
      return FixedNumberOfRuns.HasValue;
    }

    public bool HasNumberOfWarmupRuns()
    {
      return FixedNumberOfWarmUpRuns.HasValue;
    }

    public IWithNumberOfRunsWithContextStep<TContext> WithContexts(params TContext[] contexts)
    {
      BenchmarkTestContexts = contexts;
      return this;
    }

    public IWithNumberOfWarmUpRunsWithContextStep<TContext> NumberOfRuns(int numberOfRuns)
    {
      FixedNumberOfRuns = numberOfRuns;
      return this;
    }

    public IWithNumberOfWarmUpRunsWithContextStep<TContext> RunEachContextFor(TimeSpan duration)
    {
      DurationPerContext = duration;
      return this;
    }

    public IWithNumberOfWarmUpRunsWithContextStep<TContext> RunEachContextCandidateFor(TimeSpan duration)
    {
      DurationPerCandidate = duration;
      return this;
    }

    public IGoStep<TContext> NumberOfWarmUpRuns(int numberOfRuns, TContext warmUpContext = null)
    {
      FixedNumberOfWarmUpRuns = numberOfRuns;
      WarmUpContext = warmUpContext;
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

      if (FixedNumberOfRuns < 1)
      {
        throw new ArgumentException($"{nameof(NumberOfRuns)} must be greater than zero");
      }

      return new CandidateRunner<TContext>(this).Run();
    }
  }
}
