namespace Benchmark
{
  public interface IBenchmarkCandidate<TContext>
    where TContext : class, IBenchmarkContext
  {
    string Name { get; }

    void Run(TContext context);
  }

  public interface IBenchmarkCandidate
  {
    string Name { get; }

    void Run();
  }
}
