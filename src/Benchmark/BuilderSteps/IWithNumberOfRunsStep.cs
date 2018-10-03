using System;

namespace Benchmark.BuilderSteps
{
  public interface IWithNumberOfRunsStep : IWithNumberOfWarmUpRunsStep
  {
    IWithNumberOfWarmUpRunsStep WithNumberOfRuns(int numberOfRuns);

    IWithNumberOfWarmUpRunsStep WithTestDurationPerContext(TimeSpan duration);
  }
}
