using Benchmark.BuilderSteps;

namespace Benchmark.BenchmarkStrategies
{
  internal class BenchmarkStrategyFactory<TContext>
    where TContext : class, IBenchmarkContext
  {
    public IBenchmarkStrategy<TContext> Create(
      TContext context,
      CandidateRunnerWithContextArgs<TContext> args)
    {
      if (args.FixedNumberOfRuns.HasValue)
      {
        return new FixedNumberOfRunsBenchmarkStrategy<TContext>(context, args);
      }

      if (args.DurationPerCandidate.HasValue)
      {
        return new FixedDurationPerCandidateBenchmarkStrategy<TContext>(context, args);
      }

      return new FixedDurationPerContextBenchmarkStrategy<TContext>(context, args);
    }
  }
}
