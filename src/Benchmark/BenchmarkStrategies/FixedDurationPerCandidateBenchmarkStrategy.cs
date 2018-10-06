using Benchmark.BuilderSteps;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Benchmark.BenchmarkStrategies
{
  internal class FixedDurationPerCandidateBenchmarkStrategy<TContext> : IBenchmarkStrategy<TContext>
    where TContext : class, IBenchmarkContext
  {
    private readonly TContext context;

    private readonly CandidateRunnerWithContextArgs<TContext> args;

    public FixedDurationPerCandidateBenchmarkStrategy(TContext context, CandidateRunnerWithContextArgs<TContext> args)
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
      foreach (var candidate in args.Candidates)
      {
        var outstandingDuration = args.DurationPerCandidate
          ?? throw new ArgumentException(
            $"{nameof(args.DurationPerCandidate)} must not be null " +
            $"when using {nameof(FixedDurationPerCandidateBenchmarkStrategy<TContext>)}");

        while (outstandingDuration > TimeSpan.Zero)
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
