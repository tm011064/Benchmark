using System.Collections.Generic;
using System.Linq;

namespace Benchmark.Formatters
{
  internal class JsonFormatter
  {
    public string Format(IEnumerable<BenchmarkResult> results)
    {
      return "{" +
        $@" ""contexts"": [{string.Join(", ", results.Select(ToJson))}]" +
        " }";
    }

    private string ToJson(BenchmarkResult result)
    {
      return "{" +
        $@" ""name"": ""{result.Context.Description}""," +
        $@" ""candidates"": [{string.Join(", ", result.CandidateMetrics.Select(ToJson))}]" +
        " }";
    }

    private string ToJson(CandidateMetrics metrics)
    {
      return "{" +
        $@" ""name"": ""{metrics.CandidateName}""," +
        $@" ""totalMilliseconds"": ""{metrics.TotalElapsed.TotalMilliseconds}""," +
        $@" ""averageMilliseconds"": ""{metrics.AverageElapsed.TotalMilliseconds}""," +
        $@" ""medianMilliseconds"": ""{metrics.MedianElapsed.TotalMilliseconds}""," +
        $@" ""comment"": ""{metrics.Comment}""," +
        $@" ""numberOfRuns"": ""{metrics.NumberOfRuns}""" +
        " }";
    }
  }
}
