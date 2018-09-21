namespace Benchmark.BuilderSteps
{
  public interface IWithNumberOfDryRunsStep<TTestCase> : IGoStep<TTestCase>
    where TTestCase : class, ICandidateTestCase
  {
    IGoStep<TTestCase> WithNumberOfDryRuns(int numberOfRuns, TTestCase dryRunTestCase = null);
  }
}
