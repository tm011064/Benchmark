using Benchmark.BuilderSteps;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Benchmark
{
  internal class CandidateRunner<TTestCase>
    where TTestCase : class, ICandidateTestCase
  {
    private readonly CandidateRunnerArgs<TTestCase> args;

    public CandidateRunner(CandidateRunnerArgs<TTestCase> args)
    {
      this.args = args;
    }

    public IEnumerable<BenchmarkResult> Run()
    {
      for (var i = 0; i < args.NumberOfDryRuns; i++)
      {
        foreach (var candidate in args.Candidates)
        {
          candidate.Run(args.DryRunTestCase ?? args.TestCases.First());
        }
      }

      return args
        .TestCases
        .Select(BuildReports);
    }

    private BenchmarkResult BuildReports(TTestCase testCase)
    {
      var reportResults = Enumerable.Range(0, args.NumberOfRuns)
        .SelectMany(_ => args.Candidates.Select(x => x.Run(testCase)).ToArray())
        .GroupBy(x => x.Name, x => x.Elapsed)
        .Select(grouping => BuildResults(args.NumberOfRuns, grouping.Key, grouping.ToArray()));

      return new BenchmarkResult(testCase, reportResults.ToArray());
    }

    private CandidateMetrics BuildResults(
      int numberOfRuns,
      string name,
      TimeSpan[] durations)
    {
      var elapsedTicks = durations.Sum(x => x.Ticks);

      return new CandidateMetrics(
        name,
        TimeSpan.FromTicks(elapsedTicks),
        TimeSpan.FromTicks((long)Math.Round((decimal)elapsedTicks / numberOfRuns)),
        durations.OrderBy(x => x.Ticks).ElementAt(durations.Count() / 2));
    }
  }
}
