namespace Benchmark.BuilderSteps
{
  public interface IWithBenchmarkTestContextsStep<TContext>
    where TContext : class, IBenchmarkContext
  {
    IWithNumberOfRunsWithContextStep<TContext> WithContexts(params TContext[] contexts);
  }
}
