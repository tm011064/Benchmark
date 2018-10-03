using System;

namespace Benchmark.BuilderSteps
{
  public interface IWithNumberOfRunsWithContextStep<TContext> : IWithNumberOfWarmUpRunsWithContextStep<TContext>
    where TContext : class, IBenchmarkContext
  {
    IWithNumberOfWarmUpRunsWithContextStep<TContext> WithNumberOfRuns(int numberOfRuns);

    IWithNumberOfWarmUpRunsWithContextStep<TContext> WithTestDurationPerContext(TimeSpan duration);
  }
}
