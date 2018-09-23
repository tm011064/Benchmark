namespace Benchmark.BuilderSteps
{
  internal class BenchmarkCandidateNullContextWrapper : IBenchmarkCandidate<NullBenchmarkContext>
  {
    private readonly IBenchmarkCandidate candidate;

    public BenchmarkCandidateNullContextWrapper(IBenchmarkCandidate candidate)
    {
      this.candidate = candidate;
    }

    public string Name => candidate.Name;

    public void Run(NullBenchmarkContext context)
    {
      candidate.Run();
    }
  }
}
