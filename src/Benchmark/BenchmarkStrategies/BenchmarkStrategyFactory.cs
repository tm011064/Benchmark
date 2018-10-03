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
      if (args.NumberOfRuns.HasValue)
      {
        return new FixedNumberOfRunsBenchmarkStrategy<TContext>(context, args);
      }

      return new FixedDurationBenchmarkStrategy<TContext>(context, args);
    }
  }
}
