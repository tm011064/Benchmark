using System;

namespace Benchmark.BuilderSteps
{
  public interface IWithNumberOfRunsWithContextStep<TContext> : IWithNumberOfWarmUpRunsWithContextStep<TContext>
    where TContext : class, IBenchmarkContext
  {
    IWithNumberOfWarmUpRunsWithContextStep<TContext> NumberOfRuns(int numberOfRuns);

    IWithNumberOfWarmUpRunsWithContextStep<TContext> RunEachContextFor(TimeSpan duration);

    IWithNumberOfWarmUpRunsWithContextStep<TContext> RunEachContextCandidateFor(TimeSpan duration);
  }
}
