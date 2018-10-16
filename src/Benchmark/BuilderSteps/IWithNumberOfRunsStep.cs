using System;

namespace Benchmark.BuilderSteps
{
  public interface IWithNumberOfRunsStep : IWithNumberOfWarmUpRunsStep
  {
    IWithNumberOfWarmUpRunsStep NumberOfRuns(int numberOfRuns);

    IWithNumberOfWarmUpRunsStep RunEachContextFor(TimeSpan duration);

    IWithNumberOfWarmUpRunsStep RunEachContextCandidateFor(TimeSpan duration);
  }
}
