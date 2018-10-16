using System;
using System.Collections.Generic;
using System.Text;

namespace Benchmark.Tests
{
  static class CandidateMetricsFixtures
  {
    static CandidateMetrics Create<TContext>(
      IBenchmarkCandidate<TContext> candidate,
      TContext context,
      int numberOfRuns = 10,
      TimeSpan[] durations = null)
    where TContext : class, IBenchmarkContext
    {
      return CandidateMetrics.Create<TContext>(
          candidate,
          context,
          numberOfRuns,
          durations);
    }
  }
}
