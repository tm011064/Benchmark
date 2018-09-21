namespace Benchmark.BuilderSteps
{
  public interface IWithNumberOfRunsStep<TTestCase>
    where TTestCase : class, ICandidateTestCase
  {
    IWithNumberOfDryRunsStep<TTestCase> WithNumberOfRuns(int numberOfRuns);
  }
}
