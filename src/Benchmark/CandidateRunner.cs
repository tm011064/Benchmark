using Benchmark.BenchmarkStrategies;
using Benchmark.BuilderSteps;
using System.Linq;

namespace Benchmark
{
  internal class CandidateRunner<TContext>
    where TContext : class, IBenchmarkContext
  {
    private readonly BenchmarkStrategyFactory<TContext> strategyFactory = new BenchmarkStrategyFactory<TContext>();

    private readonly CandidateRunnerWithContextArgs<TContext> args;

    public CandidateRunner(CandidateRunnerWithContextArgs<TContext> args)
    {
      this.args = args;
    }

    public BenchmarkReport Run()
    {
      PerformWarmUpRuns();

      var metrics = args
        .BenchmarkTestContexts
        .Select(context => strategyFactory.Create(context, args).Execute());

      return new BenchmarkReport(metrics.ToArray());
    }

    private void PerformWarmUpRuns()
    {
      for (var i = 0; i < args.NumberOfWarmUpRuns; i++)
      {
        foreach (var candidate in args.Candidates)
        {
          candidate.Run(args.WarmUpRunBenchmarkTestContext ?? args.BenchmarkTestContexts.First());
        }
      }
    }
  }
}
