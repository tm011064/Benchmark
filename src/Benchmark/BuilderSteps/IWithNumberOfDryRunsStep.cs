namespace Benchmark.BuilderSteps
{
  public interface IWithNumberOfDryRunsStep : IGoStep
  {
    IGoStep WithNumberOfDryRuns(int numberOfRuns);
  }
}
