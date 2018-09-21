using Benchmark.BuilderSteps;

namespace Benchmark
{
  public static class Measure<TTestCase>
    where TTestCase : class, ICandidateTestCase
  {
    public static IWithTestCasesStep<TTestCase> Candidates(params ICandidate<TTestCase>[] candidates)
    {
      return new CandidateRunnerArgs<TTestCase>(candidates);
    }
  }
}
