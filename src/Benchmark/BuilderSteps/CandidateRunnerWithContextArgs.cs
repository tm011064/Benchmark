﻿using System;
using System.Linq;

namespace Benchmark.BuilderSteps
{
  internal class CandidateRunnerWithContextArgs<TContext> :
    IWithBenchmarkTestContextsStep<TContext>,
    IWithNumberOfRunsWithContextStep<TContext>,
    IWithNumberOfWarmUpRunsWithContextStep<TContext>
    where TContext : class, IBenchmarkContext
  {
    public CandidateRunnerWithContextArgs(IBenchmarkCandidate<TContext>[] candidates)
    {
      Candidates = candidates ?? throw new ArgumentNullException(nameof(candidates));
    }

    public IBenchmarkCandidate<TContext>[] Candidates { get; }

    public TContext[] BenchmarkTestContexts { get; private set; }

    public int NumberOfRuns { get; private set; }

    public int NumberOfWarmUpRuns { get; private set; }

    public TContext WarmUpRunBenchmarkTestContext { get; private set; }

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

      if (NumberOfRuns < 0)
      {
        throw new ArgumentException($"{nameof(NumberOfRuns)} must be greater than zero");
      }

      return new CandidateRunner<TContext>(this).Run();
    }
  }
}
