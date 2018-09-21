using System;

namespace Benchmark
{
  public interface ICandidate<TTestCase>
    where TTestCase : class, ICandidateTestCase
  {
    (string Name, TimeSpan Elapsed) Run(TTestCase parameters);
  }
}
