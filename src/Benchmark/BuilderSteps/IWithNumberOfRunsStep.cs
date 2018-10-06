using System;

namespace Benchmark.BuilderSteps
{
  public interface IWithNumberOfRunsStep : IWithNumberOfWarmUpRunsStep
  {
    IWithNumberOfWarmUpRunsStep WithNumberOfRuns(int numberOfRuns);

    IWithNumberOfWarmUpRunsStep RunEachContextFor(TimeSpan duration);

    IWithNumberOfWarmUpRunsStep RunEachCandidateFor(TimeSpan duration);
  }
}
