using System;
using System.Diagnostics;

namespace Benchmark
{
  public abstract class AbstractCandidate<TTestCase> : ICandidate<TTestCase>
    where TTestCase : class, ICandidateTestCase
  {
    protected readonly Stopwatch Watch = new Stopwatch();

    private readonly Stopwatch defaultWatch = new Stopwatch();

    public abstract string Name { get; }

    protected abstract void Test(TTestCase parameters);

    public (string Name, TimeSpan Elapsed) Run(TTestCase parameters)
    {
      defaultWatch.Reset();
      Watch.Reset();

      defaultWatch.Start();
      Test(parameters);
      defaultWatch.Stop();

      var watch = Watch.Elapsed == TimeSpan.Zero
        ? defaultWatch
        : Watch;

      return (Name, watch.Elapsed);
    }
  }
}
