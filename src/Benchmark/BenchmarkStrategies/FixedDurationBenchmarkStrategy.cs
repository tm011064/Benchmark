using Benchmark.BuilderSteps;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Benchmark.BenchmarkStrategies
{
  internal class FixedDurationBenchmarkStrategy<TContext> : IBenchmarkStrategy<TContext>
    where TContext : class, IBenchmarkContext
  {
    private readonly TContext context;

    private readonly CandidateRunnerWithContextArgs<TContext> args;

    public FixedDurationBenchmarkStrategy(TContext context, CandidateRunnerWithContextArgs<TContext> args)
    {
      this.context = context;
      this.args = args;
    }

    public BenchmarkResult Execute()
    {
      var runResults = PerformRuns().ToArray();

      var metrics = runResults
        .GroupBy(x => x.Candidate, x => x.Elapsed)
        .Select(grouping => CandidateMetrics.Create(grouping.Key, context, grouping.Count(), grouping.ToArray()));

      return new BenchmarkResult(context, metrics.ToArray());
    }

    private IEnumerable<(IBenchmarkCandidate<TContext> Candidate, TimeSpan Elapsed)> PerformRuns()
    {
      var outstandingDuration = args.DurationPerContext;

      while (outstandingDuration > TimeSpan.Zero)
      {
        var runWatch = Stopwatch.StartNew();

        foreach (var candidate in args.Candidates)
        {
          var watch = Stopwatch.StartNew();
          candidate.Run(context);
          watch.Stop();

          yield return (candidate, watch.Elapsed);
        }

        outstandingDuration -= runWatch.Elapsed;
      }
    }
  }
}
