namespace Benchmark.BuilderSteps
{
  public interface IWithTestCasesStep<TTestCase>
    where TTestCase : class, ICandidateTestCase
  {
    IWithNumberOfRunsStep<TTestCase> WithTestCases(params TTestCase[] testCases);
  }
}
