# Benchmark
Simple library that allows you to compare the performance of algorithms and output benchmark results as text, markdown or json. The following example shows how to benchmark two algorithms for concatenating strings:

``` c#
public void StringBuilder()
{
  var text = new StringBuilder();

  for (var i = 0; i < 5; i++)
  {
	text.Append(' ');
  }

  ObservableObject.Observe(text.ToString());
}

public void Concatenate()
{
  var text = string.Empty;

  for (var i = 0; i < 5; i++)
  {
	text += ' ';
  }

  ObservableObject.Observe(text);
}

var report = Measure
  .Candidates(
    ("Five Concatenations", Concatenate),
    ("Five String Builder Appends", StringBuilder))
  .RunFor(TimeSpan.FromSeconds(3))
  .WithNumberOfWarmUpRuns(10)
  .Go();

Console.Write(report);
```

## Examples

All example code can be found [here](https://github.com/tm011064/Benchmark/tree/master/src/Benchmark.Examples)

#### Lambda Actions

Lambda actions are a quick way to evaluate an algorithm. Just create any number of `void` methods and compare their execution times:

``` c#
var report = Measure
  .Candidates(
    ("First Algo", RunFirstAlgo),
    ("Second Algo", RunSecondAlgo))
  .WithNumberOfRuns(1000)
  .Go();
```

#### Benchmark Candidates With Contexts

Often it is useful to compare algorithms against different data scenarios. An algorithm which performs well against a few data records may become inefficient when run against large datasets. You can define classes implementing the [IBenchmarkContext](https://github.com/tm011064/Benchmark/blob/master/src/Benchmark/IBenchmarkContext.cs) interface to pass parameters to your test methods:

``` c#
var items = Enumerable.Range(0, 1000)
  .Select(_ => new ObservableObject())
  .ToArray();

var report = Measure<LoopContext>
  .Candidates<WhileLoopCandidate, ForLoopCandidate, ForEachLoopCandidate, ForLoopInlineRangeEvaluationCandidate>()
  .WithContexts(
    new LoopContext(items.Take(10).ToArray(), 1),
    new LoopContext(items, 0),
    new LoopContext(items, 1),
    new LoopContext(items, 10))
  .RunFor(TimeSpan.FromSeconds(5))
  .WithNumberOfWarmUpRuns(10, new LoopContext(items.Take(1).ToArray(), 1))
  .Go();  
```

#### Benchmark Candidates Without Context

This is similar to lambda actions but written in a more formalized way (see LINK for complete example):

``` c#
var report = Measure
  .Candidates<ConcatenateStringsCandidate, StringBuilderCandidate>()
  .WithNumberOfRuns(300)
  .WithNumberOfWarmUpRuns(10)
  .Go();
```

## Output Options

#### ToString(), ToString(RankColumn column)

Good for console output or Visual Studio debugging:

``` text
| Context           | Candidate   | Rank | +/- Median | Total     | Average   | Median    | Runs | Comment                           |
| ----------------- | ----------- | ---- | ---------- | --------- | --------- | --------- | ---- | --------------------------------- |
| 100 x 10 objects  | MessagePack |    1 |            | 0.593 sec | 0.003 sec | 0.002 sec |  201 | Average compressed size: 1.44 KB  |
|                   | Avro        |    2 |  + 23.69 % | 0.711 sec | 0.004 sec | 0.003 sec |  201 | Average compressed size: 1.14 KB  |
|                   | Protobuf    |    3 |  + 70.85 % | 0.978 sec | 0.005 sec | 0.004 sec |  201 | Average compressed size: 1.51 KB  |
|                   | Json        |    4 | + 400.95 % | 2.728 sec | 0.014 sec | 0.012 sec |  201 | Average compressed size: 3.04 KB  |
| ----------------- | ----------- | ---- | ---------- | --------- | --------- | --------- | ---- | --------------------------------- |
| 100 x 100 objects | MessagePack |    1 |            | 0.643 sec | 0.029 sec | 0.027 sec |   22 | Average compressed size: 14.36 KB |
|                   | Avro        |    2 |  + 46.98 % | 0.883 sec | 0.040 sec | 0.039 sec |   22 | Average compressed size: 11.42 KB |
|                   | Protobuf    |    3 |  + 68.40 % | 1.033 sec | 0.047 sec | 0.045 sec |   22 | Average compressed size: 15.06 KB |
|                   | Json        |    4 | + 321.70 % | 2.537 sec | 0.115 sec | 0.112 sec |   22 | Average compressed size: 30.28 KB |
| ----------------- | ----------- | ---- | ---------- | --------- | --------- | --------- | ---- | --------------------------------- |
```

### ToMarkdown(), ToMarkdown(RankColumn column)

Good for posting your results on github:

| Context | Candidate | Rank | +/- Median | Total | Average | Median | Runs | Comment |
| --- | --- | --- | --- | --- | --- | --- | --- | --- |
| 100 x 10 objects | MessagePack | 1 |  | 0.593 sec | 0.003 sec | 0.002 sec | 201 | Average compressed size: 1.44 KB |
|  | Avro | 2 | + 23.69 % | 0.711 sec | 0.004 sec | 0.003 sec | 201 | Average compressed size: 1.14 KB |
|  | Protobuf | 3 | + 70.85 % | 0.978 sec | 0.005 sec | 0.004 sec | 201 | Average compressed size: 1.51 KB |
|  | Json | 4 | + 400.95 % | 2.728 sec | 0.014 sec | 0.012 sec | 201 | Average compressed size: 3.04 KB |
|   |   |   |   |   |   |   |   |   |
| 100 x 100 objects | MessagePack | 1 |  | 0.643 sec | 0.029 sec | 0.027 sec | 22 | Average compressed size: 14.36 KB |
|  | Avro | 2 | + 46.98 % | 0.883 sec | 0.040 sec | 0.039 sec | 22 | Average compressed size: 11.42 KB |
|  | Protobuf | 3 | + 68.40 % | 1.033 sec | 0.047 sec | 0.045 sec | 22 | Average compressed size: 15.06 KB |
|  | Json | 4 | + 321.70 % | 2.537 sec | 0.115 sec | 0.112 sec | 22 | Average compressed size: 30.28 KB |
|   |   |   |   |   |   |   |   |   |

#### ToJson()

``` json
{ "contexts": [{ "name": "100 x 10 objects", "candidates": [{ "name": "Json", "totalMilliseconds": "2727.7543", "averageMilliseconds": "13.5709", "medianMilliseconds": "12.3856" "comment": "Average compressed size: 3.04 KB" "numberOfRuns": "201" }, { "name": "Avro", "totalMilliseconds": "710.5347", "averageMilliseconds": "3.535", "medianMilliseconds": "3.058" "comment": "Average compressed size: 1.14 KB" "numberOfRuns": "201" }, { "name": "Protobuf", "totalMilliseconds": "977.5612", "averageMilliseconds": "4.8635", "medianMilliseconds": "4.224" "comment": "Average compressed size: 1.51 KB" "numberOfRuns": "201" }, { "name": "MessagePack", "totalMilliseconds": "592.6003", "averageMilliseconds": "2.9483", "medianMilliseconds": "2.4724" "comment": "Average compressed size: 1.44 KB" "numberOfRuns": "201" }] }, { "name": "100 x 100 objects", "candidates": [{ "name": "Json", "totalMilliseconds": "2537.3437", "averageMilliseconds": "115.3338", "medianMilliseconds": "111.8661" "comment": "Average compressed size: 30.28 KB" "numberOfRuns": "22" }, { "name": "Avro", "totalMilliseconds": "882.6966", "averageMilliseconds": "40.1226", "medianMilliseconds": "38.9896" "comment": "Average compressed size: 11.42 KB" "numberOfRuns": "22" }, { "name": "Protobuf", "totalMilliseconds": "1032.6908", "averageMilliseconds": "46.9405", "medianMilliseconds": "44.6737" "comment": "Average compressed size: 15.06 KB" "numberOfRuns": "22" }, { "name": "MessagePack", "totalMilliseconds": "643.2519", "averageMilliseconds": "29.2387", "medianMilliseconds": "26.5277" "comment": "Average compressed size: 14.36 KB" "numberOfRuns": "22" }] }] }
```

#### IBenchmarkComment

If your [IBenchmarkCandidate](https://github.com/tm011064/Benchmark/blob/master/src/Benchmark/IBenchmarkCandidate.cs) also implements the [IBenchmarkComment](https://github.com/tm011064/Benchmark/blob/master/src/Benchmark/IBenchmarkComment.cs) interface you can render a comment for each candidate per context. You can use this method to output additional information gathered during tests.

## Warm Up Runs
The [Measure](https://github.com/tm011064/Benchmark/blob/master/src/Benchmark/Measure.cs) builder allows you to specify a number of warm up runs for your algorithm to counter JIT compilation influencing your results. If you have a good number of runs and are only interested in the median runtime, you won't need this feature. If overall/average execution time of the runs is important, warm up runs will remove any distortions caused by JIT compilation.
