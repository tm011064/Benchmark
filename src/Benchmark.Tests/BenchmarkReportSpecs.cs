using Benchmark.Formatters;
using Machine.Fakes;
using Machine.Specifications;
using System;

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
          new BenchmarkContextMetrics(
            first_context,
            new[]
            {
              new BenchmarkMetrics("candidate one", TimeSpan.FromTicks(1), TimeSpan.FromMilliseconds(1), TimeSpan.FromSeconds(1)),
              new BenchmarkMetrics("candidate two", TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(10), TimeSpan.FromMinutes(2))
            }),
          new BenchmarkContextMetrics(
            second_context,
            new[]
            {
              new BenchmarkMetrics("candidate one", TimeSpan.FromMinutes(1), TimeSpan.FromMilliseconds(1), TimeSpan.FromMilliseconds(1)),
              new BenchmarkMetrics("candidate two", TimeSpan.FromSeconds(10), TimeSpan.FromSeconds(10), TimeSpan.FromTicks(1))
            }),
          new BenchmarkContextMetrics(
            third_context,
            new[]
            {
              new BenchmarkMetrics("candidate one", TimeSpan.FromMinutes(1), TimeSpan.FromSeconds(2), TimeSpan.FromSeconds(1)),
              new BenchmarkMetrics("candidate two", TimeSpan.FromMinutes(2), TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(2))
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

      class when_converting_to_csv
      {
        class when_average
        {
          Because of = () =>
            result = subject.ToCsv(RankColumn.Average);

          It returned_expected_result = () =>
            result.ShouldEqual(@"
Context,Candidate,Rank,+/- Average,Total,Average,Median
""first Context"",""candidate one"",""1"","""",""0.0001 ms"",""0.001 sec"",""1.000 sec""
""first Context"", ""candidate two"", ""2"", ""+ 999900.00 %"", ""1000.0000 ms"", ""10.000 sec"", ""120.000 sec""
""second Context"", ""candidate one"", ""1"", """", ""60.00 sec"", ""0.001 sec"", ""1.0000 ms""
""second Context"", ""candidate two"", ""2"", ""+ 999900.00 %"", ""10.00 sec"", ""10.000 sec"", ""0.0001 ms""
""third Context"", ""candidate two"", ""1"", """", ""00:02:00"", ""1.000 sec"", ""2.000 sec""
""third Context"", ""candidate one"", ""2"", ""+ 100.00 %"", ""00:01:00"", ""2.000 sec"", ""1.000 sec""
".TrimStart());
        }

        class when_median
        {
          Because of = () =>
            result = subject.ToCsv(RankColumn.Median);

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
            result = subject.ToCsv(RankColumn.Total);

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
    }
  }
}
