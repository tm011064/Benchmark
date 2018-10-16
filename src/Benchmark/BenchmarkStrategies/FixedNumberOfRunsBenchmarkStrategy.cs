using Benchmark.BuilderSteps;
using System.Diagnostics;
using System.Linq;

namespace Benchmark.BenchmarkStrategies
{
  internal class FixedNumberOfRunsBenchmarkStrategy<TContext> : IBenchmarkStrategy<TContext>
    where TContext : class, IBenchmarkContext
  {
    private readonly TContext context;

    private readonly CandidateRunnerWithContextArgs<TContext> args;

    public FixedNumberOfRunsBenchmarkStrategy(TContext context, CandidateRunnerWithContextArgs<TContext> args)
    {
      this.context = context;
      this.args = args;
    }

    public BenchmarkResult Execute()
    {
      var numberOfRuns = args.FixedNumberOfRuns.Value;

      var runResults = Enumerable.Range(0, numberOfRuns)
        .SelectMany(_ => args.Candidates.Select(candidate =>
        {
          var watch = Stopwatch.StartNew();
          candidate.Run(context);
          watch.Stop();

          return new { Candidate = candidate, watch.Elapsed };
        }).ToArray());

      var metrics = runResults
        .GroupBy(x => x.Candidate, x => x.Elapsed)
        .Select(grouping => CandidateMetrics.Create(grouping.Key, context, numberOfRuns, grouping.ToArray()));

      return new BenchmarkResult(context, metrics.ToArray());
    }
  }
}
