using Benchmark.BuilderSteps;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Benchmark.BenchmarkStrategies
{
  internal class FixedDurationPerContextBenchmarkStrategy<TContext> : IBenchmarkStrategy<TContext>
    where TContext : class, IBenchmarkContext
  {
    private readonly TContext context;

    private readonly CandidateRunnerWithContextArgs<TContext> args;

    public FixedDurationPerContextBenchmarkStrategy(TContext context, CandidateRunnerWithContextArgs<TContext> args)
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
      var outstandingDuration = args.DurationPerContext ?? TimeSpan.FromSeconds(1);

      while (outstandingDuration > TimeSpan.Zero)
      {
        foreach (var candidate in args.Candidates)
        {
          var watch = Stopwatch.StartNew();
          candidate.Run(context);
          watch.Stop();

          outstandingDuration -= watch.Elapsed;

          yield return (candidate, watch.Elapsed);
        }
      }
    }
  }
}
