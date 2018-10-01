using Benchmark.BuilderSteps;
using System;
using System.Diagnostics;
using System.Linq;

namespace Benchmark
{
  internal class CandidateRunner<TContext>
    where TContext : class, IBenchmarkContext
  {
    private readonly CandidateRunnerWithContextArgs<TContext> args;

    public CandidateRunner(CandidateRunnerWithContextArgs<TContext> args)
    {
      this.args = args;
    }

    public BenchmarkReport Run()
    {
      PerformDryRuns();

      var metrics = args
        .BenchmarkTestContexts
        .Select(PerformBenchmarkTest);

      return new BenchmarkReport(metrics.ToArray());
    }

    private void PerformDryRuns()
    {
      for (var i = 0; i < args.NumberOfDryRuns; i++)
      {
        foreach (var candidate in args.Candidates)
        {
          candidate.Run(args.DryRunBenchmarkTestContext ?? args.BenchmarkTestContexts.First());
        }
      }
    }

    private BenchmarkResult PerformBenchmarkTest(TContext context)
    {
      var runResults = Enumerable.Range(0, args.NumberOfRuns)
        .SelectMany(_ => args.Candidates.Select(candidate =>
        {
          var watch = Stopwatch.StartNew();
          candidate.Run(context);
          watch.Stop();

          return new { Candidate = candidate, watch.Elapsed };
        }).ToArray());

      var metrics = runResults
        .GroupBy(x => x.Candidate, x => x.Elapsed)
        .Select(grouping => CreateMetrics(args.NumberOfRuns, grouping.Key.Name, grouping.ToArray()));

      return new BenchmarkResult(context, metrics.ToArray());
    }

    private CandidateMetrics CreateMetrics(
      int numberOfRuns,
      string candidateName,
      TimeSpan[] durations)
    {
      var elapsedTicks = durations.Sum(x => x.Ticks);

      return new CandidateMetrics(
        candidateName,
        TimeSpan.FromTicks(elapsedTicks),
        TimeSpan.FromTicks((long)Math.Round((decimal)elapsedTicks / numberOfRuns)),
        durations.OrderBy(x => x.Ticks).ElementAt(durations.Count() / 2));
    }
  }
}
