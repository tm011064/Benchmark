using Benchmark.BuilderSteps;
using System;
using System.Linq;

namespace Benchmark
{
  public static class Measure<TContext>
    where TContext : class, IBenchmarkContext
  {
    public static IWithBenchmarkTestContextsStep<TContext> Candidates(params IBenchmarkCandidate<TContext>[] candidates)
    {
      return new CandidateRunnerWithContextArgs<TContext>(candidates);
    }

    public static IWithBenchmarkTestContextsStep<TContext> Candidates<TCandidate>()
      where TCandidate : class, IBenchmarkCandidate<TContext>
    {
      var candidate = (TCandidate)Activator.CreateInstance(typeof(TCandidate));

      return new CandidateRunnerWithContextArgs<TContext>(
        new[]
        {
          (TCandidate)Activator.CreateInstance(typeof(TCandidate))
        });
    }

    public static IWithBenchmarkTestContextsStep<TContext> Candidates<TCandidate1, TCandidate2>()
      where TCandidate1 : IBenchmarkCandidate<TContext>
      where TCandidate2 : IBenchmarkCandidate<TContext>
    {
      return new CandidateRunnerWithContextArgs<TContext>(
        new IBenchmarkCandidate<TContext>[]
        {
          (TCandidate1)Activator.CreateInstance(typeof(TCandidate1)),
          (TCandidate2)Activator.CreateInstance(typeof(TCandidate2))
        });
    }

    public static IWithBenchmarkTestContextsStep<TContext> Candidates<TCandidate1, TCandidate2, TCandidate3>()
      where TCandidate1 : IBenchmarkCandidate<TContext>
      where TCandidate2 : IBenchmarkCandidate<TContext>
      where TCandidate3 : IBenchmarkCandidate<TContext>
    {
      return new CandidateRunnerWithContextArgs<TContext>(
        new IBenchmarkCandidate<TContext>[]
        {
          (TCandidate1)Activator.CreateInstance(typeof(TCandidate1)),
          (TCandidate2)Activator.CreateInstance(typeof(TCandidate2)),
          (TCandidate3)Activator.CreateInstance(typeof(TCandidate3))
        });
    }

    public static IWithBenchmarkTestContextsStep<TContext> Candidates<TCandidate1, TCandidate2, TCandidate3, TCandidate4>()
      where TCandidate1 : IBenchmarkCandidate<TContext>
      where TCandidate2 : IBenchmarkCandidate<TContext>
      where TCandidate3 : IBenchmarkCandidate<TContext>
      where TCandidate4 : IBenchmarkCandidate<TContext>
    {
      return new CandidateRunnerWithContextArgs<TContext>(
        new IBenchmarkCandidate<TContext>[]
        {
          (TCandidate1)Activator.CreateInstance(typeof(TCandidate1)),
          (TCandidate2)Activator.CreateInstance(typeof(TCandidate2)),
          (TCandidate3)Activator.CreateInstance(typeof(TCandidate3)),
          (TCandidate4)Activator.CreateInstance(typeof(TCandidate4))
        });
    }

    public static IWithBenchmarkTestContextsStep<TContext> Candidates<TCandidate1, TCandidate2, TCandidate3, TCandidate4, TCandidate5>()
      where TCandidate1 : IBenchmarkCandidate<TContext>
      where TCandidate2 : IBenchmarkCandidate<TContext>
      where TCandidate3 : IBenchmarkCandidate<TContext>
      where TCandidate4 : IBenchmarkCandidate<TContext>
      where TCandidate5 : IBenchmarkCandidate<TContext>
    {
      return new CandidateRunnerWithContextArgs<TContext>(
        new IBenchmarkCandidate<TContext>[]
        {
          (TCandidate1)Activator.CreateInstance(typeof(TCandidate1)),
          (TCandidate2)Activator.CreateInstance(typeof(TCandidate2)),
          (TCandidate3)Activator.CreateInstance(typeof(TCandidate3)),
          (TCandidate4)Activator.CreateInstance(typeof(TCandidate4)),
          (TCandidate5)Activator.CreateInstance(typeof(TCandidate5))
        });
    }

    public static IWithBenchmarkTestContextsStep<TContext> Candidates<TCandidate1, TCandidate2, TCandidate3, TCandidate4, TCandidate5, TCandidate6>()
      where TCandidate1 : IBenchmarkCandidate<TContext>
      where TCandidate2 : IBenchmarkCandidate<TContext>
      where TCandidate3 : IBenchmarkCandidate<TContext>
      where TCandidate4 : IBenchmarkCandidate<TContext>
      where TCandidate5 : IBenchmarkCandidate<TContext>
      where TCandidate6 : IBenchmarkCandidate<TContext>
    {
      return new CandidateRunnerWithContextArgs<TContext>(
        new IBenchmarkCandidate<TContext>[]
        {
          (TCandidate1)Activator.CreateInstance(typeof(TCandidate1)),
          (TCandidate2)Activator.CreateInstance(typeof(TCandidate2)),
          (TCandidate3)Activator.CreateInstance(typeof(TCandidate3)),
          (TCandidate4)Activator.CreateInstance(typeof(TCandidate4)),
          (TCandidate5)Activator.CreateInstance(typeof(TCandidate5)),
          (TCandidate6)Activator.CreateInstance(typeof(TCandidate6))
        });
    }
  }

  public static class Measure
  {
    public static IWithNumberOfRunsStep Candidates(params (string Name, Action Action)[] candidates)
    {
      var wrapped = candidates
        .Select(candidate => new BenchmarkCandidateActionWrapper(candidate.Action, candidate.Name));

      return new CandidateRunnerArgs(wrapped.ToArray());
    }

    public static IWithNumberOfRunsStep Candidates(params Action[] actions)
    {
      var wrapped = actions
        .Select((action, index) => new BenchmarkCandidateActionWrapper(action, $"Candidate {index + 1}"));

      return new CandidateRunnerArgs(wrapped.ToArray());
    }

    public static IWithNumberOfRunsStep Candidates(params IBenchmarkCandidate[] candidates)
    {
      return new CandidateRunnerArgs(candidates);
    }

    public static IWithNumberOfRunsStep Candidates<TCandidate>()
      where TCandidate : class, IBenchmarkCandidate
    {
      var candidate = (TCandidate)Activator.CreateInstance(typeof(TCandidate));

      return new CandidateRunnerArgs(
        new[]
        {
          (TCandidate)Activator.CreateInstance(typeof(TCandidate))
        });
    }

    public static IWithNumberOfRunsStep Candidates<TCandidate1, TCandidate2>()
      where TCandidate1 : IBenchmarkCandidate
      where TCandidate2 : IBenchmarkCandidate
    {
      return new CandidateRunnerArgs(
        new IBenchmarkCandidate[]
        {
          (TCandidate1)Activator.CreateInstance(typeof(TCandidate1)),
          (TCandidate2)Activator.CreateInstance(typeof(TCandidate2))
        });
    }

    public static IWithNumberOfRunsStep Candidates<TCandidate1, TCandidate2, TCandidate3>()
      where TCandidate1 : IBenchmarkCandidate
      where TCandidate2 : IBenchmarkCandidate
      where TCandidate3 : IBenchmarkCandidate
    {
      return new CandidateRunnerArgs(
        new IBenchmarkCandidate[]
        {
          (TCandidate1)Activator.CreateInstance(typeof(TCandidate1)),
          (TCandidate2)Activator.CreateInstance(typeof(TCandidate2)),
          (TCandidate3)Activator.CreateInstance(typeof(TCandidate3))
        });
    }

    public static IWithNumberOfRunsStep Candidates<TCandidate1, TCandidate2, TCandidate3, TCandidate4>()
      where TCandidate1 : IBenchmarkCandidate
      where TCandidate2 : IBenchmarkCandidate
      where TCandidate3 : IBenchmarkCandidate
      where TCandidate4 : IBenchmarkCandidate
    {
      return new CandidateRunnerArgs(
        new IBenchmarkCandidate[]
        {
          (TCandidate1)Activator.CreateInstance(typeof(TCandidate1)),
          (TCandidate2)Activator.CreateInstance(typeof(TCandidate2)),
          (TCandidate3)Activator.CreateInstance(typeof(TCandidate3)),
          (TCandidate4)Activator.CreateInstance(typeof(TCandidate4))
        });
    }

    public static IWithNumberOfRunsStep Candidates<TCandidate1, TCandidate2, TCandidate3, TCandidate4, TCandidate5>()
      where TCandidate1 : IBenchmarkCandidate
      where TCandidate2 : IBenchmarkCandidate
      where TCandidate3 : IBenchmarkCandidate
      where TCandidate4 : IBenchmarkCandidate
      where TCandidate5 : IBenchmarkCandidate
    {
      return new CandidateRunnerArgs(
        new IBenchmarkCandidate[]
        {
          (TCandidate1)Activator.CreateInstance(typeof(TCandidate1)),
          (TCandidate2)Activator.CreateInstance(typeof(TCandidate2)),
          (TCandidate3)Activator.CreateInstance(typeof(TCandidate3)),
          (TCandidate4)Activator.CreateInstance(typeof(TCandidate4)),
          (TCandidate5)Activator.CreateInstance(typeof(TCandidate5))
        });
    }

    public static IWithNumberOfRunsStep Candidates<TCandidate1, TCandidate2, TCandidate3, TCandidate4, TCandidate5, TCandidate6>()
      where TCandidate1 : IBenchmarkCandidate
      where TCandidate2 : IBenchmarkCandidate
      where TCandidate3 : IBenchmarkCandidate
      where TCandidate4 : IBenchmarkCandidate
      where TCandidate5 : IBenchmarkCandidate
      where TCandidate6 : IBenchmarkCandidate
    {
      return new CandidateRunnerArgs(
        new IBenchmarkCandidate[]
        {
          (TCandidate1)Activator.CreateInstance(typeof(TCandidate1)),
          (TCandidate2)Activator.CreateInstance(typeof(TCandidate2)),
          (TCandidate3)Activator.CreateInstance(typeof(TCandidate3)),
          (TCandidate4)Activator.CreateInstance(typeof(TCandidate4)),
          (TCandidate5)Activator.CreateInstance(typeof(TCandidate5)),
          (TCandidate6)Activator.CreateInstance(typeof(TCandidate6))
        });
    }
  }
}
