using System.Collections.Generic;

namespace Benchmark.BuilderSteps
{
  public interface IGoStep<TTestCase>
    where TTestCase : class, ICandidateTestCase
  {
    IEnumerable<BenchmarkResult> Go();
  }
}
