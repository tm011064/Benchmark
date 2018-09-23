using System;

namespace Benchmark.BuilderSteps
{
  internal class BenchmarkCandidateActionWrapper : IBenchmarkCandidate
  {
    private readonly Action action;

    public BenchmarkCandidateActionWrapper(Action action, string name)
    {
      this.action = action;

      Name = name;
    }

    public string Name { get; }

    public void Run()
    {
      action();
    }
  }
}
