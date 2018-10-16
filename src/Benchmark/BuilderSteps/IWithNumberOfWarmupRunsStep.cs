namespace Benchmark.BuilderSteps
{
  public interface IWithNumberOfWarmUpRunsStep : IGoStep
  {
    IGoStep NumberOfWarmUpRuns(int numberOfRuns);
  }
}
