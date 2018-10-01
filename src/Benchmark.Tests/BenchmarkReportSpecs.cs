using Benchmark.Formatters;
using Machine.Fakes;
using Machine.Specifications;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;

namespace Benchmark.Tests
{
  [Subject(typeof(BenchmarkReport))]
  public class BenchmarkReportSpecs : WithFakes
  {
    static BenchmarkReport subject;

    static string result;

    static readonly IBenchmarkContext first_context = An<IBenchmarkContext>();

    static readonly IBenchmarkContext second_context = An<IBenchmarkContext>();

    static readonly IBenchmarkContext third_context = An<IBenchmarkContext>();

    Establish context = () =>
    {
      first_context.WhenToldTo(x => x.Description).Return("first Context");
      second_context.WhenToldTo(x => x.Description).Return("second Context");
      third_context.WhenToldTo(x => x.Description).Return("third Context");
    };

    class when_testing_time_units
    {
      Establish context = () =>
        subject = new BenchmarkReport(new[]
        {
          new BenchmarkResult(
            first_context,
            new[]
            {
              new CandidateMetrics("candidate one", TimeSpan.FromTicks(1), TimeSpan.FromMilliseconds(1), TimeSpan.FromSeconds(1)),
              new CandidateMetrics("candidate two", TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(10), TimeSpan.FromMinutes(2))
            }),
          new BenchmarkResult(
            second_context,
            new[]
            {
              new CandidateMetrics("candidate one", TimeSpan.FromMinutes(1), TimeSpan.FromMilliseconds(1), TimeSpan.FromMilliseconds(1)),
              new CandidateMetrics("candidate two", TimeSpan.FromSeconds(10), TimeSpan.FromSeconds(10), TimeSpan.FromTicks(1))
            }),
          new BenchmarkResult(
            third_context,
            new[]
            {
              new CandidateMetrics("candidate one", TimeSpan.FromMinutes(1), TimeSpan.FromSeconds(2), TimeSpan.FromSeconds(1)),
              new CandidateMetrics("candidate two", TimeSpan.FromMinutes(2), TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(2))
            })
        });

      class when_converting_to_string
      {
        Because of = () =>
          result = subject.ToString();

        It returned_expected_result = () =>
          result.ShouldEqual(@"
| Context        | Candidate     | Rank | +/- Median    | Total        | Average    | Median      |
| -------------- | ------------- | ---- | ------------- | ------------ | ---------- | ----------- |
| first Context  | candidate one |    1 |               |    0.0001 ms |  0.001 sec |   1.000 sec |
|                | candidate two |    2 |  + 11900.00 % | 1000.0000 ms | 10.000 sec | 120.000 sec |
| -------------- | ------------- | ---- | ------------- | ------------ | ---------- | ----------- |
| second Context | candidate two |    1 |               |    10.00 sec | 10.000 sec |   0.0001 ms |
|                | candidate one |    2 | + 999900.00 % |    60.00 sec |  0.001 sec |   1.0000 ms |
| -------------- | ------------- | ---- | ------------- | ------------ | ---------- | ----------- |
| third Context  | candidate one |    1 |               |     00:01:00 |  2.000 sec |   1.000 sec |
|                | candidate two |    2 |    + 100.00 % |     00:02:00 |  1.000 sec |   2.000 sec |
| -------------- | ------------- | ---- | ------------- | ------------ | ---------- | ----------- |
".TrimStart());
      }

      class when_converting_to_string_with_order
      {
        class when_average
        {
          Because of = () =>
            result = subject.ToString(RankColumn.Average);

          It returned_expected_result = () =>
            result.ShouldEqual(@"
| Context        | Candidate     | Rank | +/- Average   | Total        | Average    | Median      |
| -------------- | ------------- | ---- | ------------- | ------------ | ---------- | ----------- |
| first Context  | candidate one |    1 |               |    0.0001 ms |  0.001 sec |   1.000 sec |
|                | candidate two |    2 | + 999900.00 % | 1000.0000 ms | 10.000 sec | 120.000 sec |
| -------------- | ------------- | ---- | ------------- | ------------ | ---------- | ----------- |
| second Context | candidate one |    1 |               |    60.00 sec |  0.001 sec |   1.0000 ms |
|                | candidate two |    2 | + 999900.00 % |    10.00 sec | 10.000 sec |   0.0001 ms |
| -------------- | ------------- | ---- | ------------- | ------------ | ---------- | ----------- |
| third Context  | candidate two |    1 |               |     00:02:00 |  1.000 sec |   2.000 sec |
|                | candidate one |    2 |    + 100.00 % |     00:01:00 |  2.000 sec |   1.000 sec |
| -------------- | ------------- | ---- | ------------- | ------------ | ---------- | ----------- |
".TrimStart());
        }

        class when_median
        {
          Because of = () =>
            result = subject.ToString(RankColumn.Median);

          It returned_expected_result = () =>
            result.ShouldEqual(@"
| Context        | Candidate     | Rank | +/- Median    | Total        | Average    | Median      |
| -------------- | ------------- | ---- | ------------- | ------------ | ---------- | ----------- |
| first Context  | candidate one |    1 |               |    0.0001 ms |  0.001 sec |   1.000 sec |
|                | candidate two |    2 |  + 11900.00 % | 1000.0000 ms | 10.000 sec | 120.000 sec |
| -------------- | ------------- | ---- | ------------- | ------------ | ---------- | ----------- |
| second Context | candidate two |    1 |               |    10.00 sec | 10.000 sec |   0.0001 ms |
|                | candidate one |    2 | + 999900.00 % |    60.00 sec |  0.001 sec |   1.0000 ms |
| -------------- | ------------- | ---- | ------------- | ------------ | ---------- | ----------- |
| third Context  | candidate one |    1 |               |     00:01:00 |  2.000 sec |   1.000 sec |
|                | candidate two |    2 |    + 100.00 % |     00:02:00 |  1.000 sec |   2.000 sec |
| -------------- | ------------- | ---- | ------------- | ------------ | ---------- | ----------- |
".TrimStart());
        }

        class when_total
        {
          Because of = () =>
            result = subject.ToString(RankColumn.Total);

          It returned_expected_result = () =>
            result.ShouldEqual(@"
| Context        | Candidate     | Rank | +/- Total        | Total        | Average    | Median      |
| -------------- | ------------- | ---- | ---------------- | ------------ | ---------- | ----------- |
| first Context  | candidate one |    1 |                  |    0.0001 ms |  0.001 sec |   1.000 sec |
|                | candidate two |    2 | + 999999900.00 % | 1000.0000 ms | 10.000 sec | 120.000 sec |
| -------------- | ------------- | ---- | ---------------- | ------------ | ---------- | ----------- |
| second Context | candidate two |    1 |                  |    10.00 sec | 10.000 sec |   0.0001 ms |
|                | candidate one |    2 |       + 500.00 % |    60.00 sec |  0.001 sec |   1.0000 ms |
| -------------- | ------------- | ---- | ---------------- | ------------ | ---------- | ----------- |
| third Context  | candidate one |    1 |                  |     00:01:00 |  2.000 sec |   1.000 sec |
|                | candidate two |    2 |       + 100.00 % |     00:02:00 |  1.000 sec |   2.000 sec |
| -------------- | ------------- | ---- | ---------------- | ------------ | ---------- | ----------- |
".TrimStart());
        }
      }

      class when_converting_to_markup
      {
        class when_average
        {
          Because of = () =>
            result = subject.ToMarkuo(RankColumn.Average);

          It returned_expected_result = () =>
            result.ShouldEqual(@"
| Context | Candidate | Rank | +/- Average | Total | Average | Median |
| --- | --- | --- | --- | --- | --- | --- |
| first Context | candidate one | 1 |  | 0.0001 ms | 0.001 sec | 1.000 sec |
|  | candidate two | 2 | + 999900.00 % | 1000.0000 ms | 10.000 sec | 120.000 sec |
|   |   |   |   |   |   |   |
| second Context | candidate one | 1 |  | 60.00 sec | 0.001 sec | 1.0000 ms |
|  | candidate two | 2 | + 999900.00 % | 10.00 sec | 10.000 sec | 0.0001 ms |
|   |   |   |   |   |   |   |
| third Context | candidate two | 1 |  | 00:02:00 | 1.000 sec | 2.000 sec |
|  | candidate one | 2 | + 100.00 % | 00:01:00 | 2.000 sec | 1.000 sec |
|   |   |   |   |   |   |   |
".TrimStart());
        }

        class when_median
        {
          Because of = () =>
            result = subject.ToMarkuo(RankColumn.Median);

          It returned_expected_result = () =>
            result.ShouldEqual(@"
| Context | Candidate | Rank | +/- Median | Total | Average | Median |
| --- | --- | --- | --- | --- | --- | --- |
| first Context | candidate one | 1 |  | 0.0001 ms | 0.001 sec | 1.000 sec |
|  | candidate two | 2 | + 11900.00 % | 1000.0000 ms | 10.000 sec | 120.000 sec |
|   |   |   |   |   |   |   |
| second Context | candidate two | 1 |  | 10.00 sec | 10.000 sec | 0.0001 ms |
|  | candidate one | 2 | + 999900.00 % | 60.00 sec | 0.001 sec | 1.0000 ms |
|   |   |   |   |   |   |   |
| third Context | candidate one | 1 |  | 00:01:00 | 2.000 sec | 1.000 sec |
|  | candidate two | 2 | + 100.00 % | 00:02:00 | 1.000 sec | 2.000 sec |
|   |   |   |   |   |   |   |
".TrimStart());
        }

        class when_total
        {
          Because of = () =>
            result = subject.ToMarkuo(RankColumn.Total);

          It returned_expected_result = () =>
            result.ShouldEqual(@"
| Context | Candidate | Rank | +/- Total | Total | Average | Median |
| --- | --- | --- | --- | --- | --- | --- |
| first Context | candidate one | 1 |  | 0.0001 ms | 0.001 sec | 1.000 sec |
|  | candidate two | 2 | + 999999900.00 % | 1000.0000 ms | 10.000 sec | 120.000 sec |
|   |   |   |   |   |   |   |
| second Context | candidate two | 1 |  | 10.00 sec | 10.000 sec | 0.0001 ms |
|  | candidate one | 2 | + 500.00 % | 60.00 sec | 0.001 sec | 1.0000 ms |
|   |   |   |   |   |   |   |
| third Context | candidate one | 1 |  | 00:01:00 | 2.000 sec | 1.000 sec |
|  | candidate two | 2 | + 100.00 % | 00:02:00 | 1.000 sec | 2.000 sec |
|   |   |   |   |   |   |   |
".TrimStart());
        }
      }

      class when_converting_to_json
      {
        static JsonReport report;

        Because of = () =>
        {
          var json = subject.ToJson();

          var serializer = new JsonSerializer();

          using (var stringReader = new StringReader(json))
          {
            report = serializer.Deserialize<JsonReport>(new JsonTextReader(stringReader));
          }
        };

        It returned_expected_result = () =>
        {
          report.Contexts.Count().ShouldEqual(3);

          var firstContext = report.Contexts.Single(x => x.Name == "first Context");
          firstContext.Candidates.Count().ShouldEqual(2);

          AssertCandidate(firstContext, "candidate one", 0.0001m, 1m, 1000m);
          AssertCandidate(firstContext, "candidate two", 1000m, 10000m, 120000m);

          var secondContext = report.Contexts.Single(x => x.Name == "second Context");
          secondContext.Candidates.Count().ShouldEqual(2);

          AssertCandidate(secondContext, "candidate one", 60000m, 1m, 1m);
          AssertCandidate(secondContext, "candidate two", 10000m, 10000m, .0001m);

          var thirdContext = report.Contexts.Single(x => x.Name == "third Context");
          thirdContext.Candidates.Count().ShouldEqual(2);

          AssertCandidate(thirdContext, "candidate one", 60000m, 2000m, 1000m);
          AssertCandidate(thirdContext, "candidate two", 120000m, 1000m, 2000m);
        };
        
        static void AssertCandidate(
          JsonReport.Context context,
          string name,
          decimal expectedTotalMilliseconds,
          decimal expectedAverageMilliseconds,
          decimal expectedMedianMilliseconds)
        {
          var candidate = context.Candidates.Single(x => x.Name == name);

          candidate.TotalMilliseconds.ShouldEqual(expectedTotalMilliseconds);
          candidate.AverageMilliseconds.ShouldEqual(expectedAverageMilliseconds);
          candidate.MedianMilliseconds.ShouldEqual(expectedMedianMilliseconds);
        }

        public class JsonReport
        {
          public Context[] Contexts { get; set; }

          public class Context
          {
            public string Name { get; set; }

            public Candidate[] Candidates { get; set; }

            public class Candidate
            {
              public string Name { get; set; }

              public decimal TotalMilliseconds { get; set; }

              public decimal AverageMilliseconds { get; set; }

              public decimal MedianMilliseconds { get; set; }
            }
          }
        }
      }
    }
  }
}
