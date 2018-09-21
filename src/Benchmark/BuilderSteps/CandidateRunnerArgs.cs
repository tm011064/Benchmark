using System;
using System.Collections.Generic;
using System.Linq;

namespace Benchmark.BuilderSteps
{
  internal class CandidateRunnerArgs<TTestCase> :
    IWithTestCasesStep<TTestCase>,
    IWithNumberOfRunsStep<TTestCase>,
    IWithNumberOfDryRunsStep<TTestCase>
    where TTestCase : class, ICandidateTestCase
  {
    public CandidateRunnerArgs(ICandidate<TTestCase>[] candidates)
    {
      Candidates = candidates ?? throw new ArgumentNullException(nameof(candidates));
    }

    public ICandidate<TTestCase>[] Candidates { get; }

    public TTestCase[] TestCases { get; private set; }

    public int NumberOfRuns { get; private set; }

    public int NumberOfDryRuns { get; private set; }

    public TTestCase DryRunTestCase { get; private set; }

    public IWithNumberOfRunsStep<TTestCase> WithTestCases(params TTestCase[] testCases)
    {
      TestCases = testCases;
      return this;
    }

    public IWithNumberOfDryRunsStep<TTestCase> WithNumberOfRuns(int numberOfRuns)
    {
      NumberOfRuns = numberOfRuns;
      return this;
    }

    public IGoStep<TTestCase> WithNumberOfDryRuns(int numberOfRuns, TTestCase dryRunTestCase = null)
    {
      NumberOfDryRuns = numberOfRuns;
      return this;
    }

    public IEnumerable<BenchmarkResult> Go()
    {
      if (Candidates?.Any() != true)
      {
        throw new ArgumentException($"At least one {nameof(ICandidate<TTestCase>)} must be provided");
      }

      if (TestCases?.Any() != true)
      {
        throw new ArgumentException($"At least one {nameof(TTestCase)} must be provided");
      }

      if (NumberOfRuns < 0)
      {
        throw new ArgumentException($"{nameof(NumberOfRuns)} must be greater than zero");
      }

      return new CandidateRunner<TTestCase>(this).Run();
    }
  }
}
