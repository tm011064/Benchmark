namespace Benchmark.BuilderSteps
{
  public interface IWithNumberOfWarmUpRunsStep : IGoStep
  {
    IGoStep WithNumberOfWarmUpRuns(int numberOfRuns);
  }
}
