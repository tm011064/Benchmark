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
  .Go();  
```

#### Benchmark Candidates Without Context

This is similar to lambda actions but written in a more formalized way (see LINK for complete example):

``` c#
var report = Measure
  .Candidates<ConcatenateStringsCandidate, StringBuilderCandidate>()
  .Go();
```

## Number of runs

You have multiple options to define how many runs a benchmark test should perform. Your test setting can either be time based or based on a fixed number of executions:

``` c#
// this will run the tests 100 times
Measure
  .Candidates<Foo, Bar>()
  .NumberOfRuns(100)
  .Go();
  
// this will run each test context for one second
Measure
  .Candidates<Foo, Bar>()
  .RunEachContextFor(TimeSpan.FromSecond(1))
  .Go();
  
// this will run each context candidate for one second
Measure
  .Candidates<Foo, Bar>()
  .RunEachContextFor(TimeSpan.FromSecond(1))
  .Go();
```

If no option is defined, Benchmark will run each candidate for one second per context.

## Output Options

#### ToString(), ToString(RankColumn column)

Good for console output or Visual Studio debugging:

``` text
| Context       | Candidate   | Rank | +/- Median | Total     | Average   | Median    | Runs | Comment        |
| ------------- | ----------- | ---- | ---------- | --------- | --------- | --------- | ---- | -------------- |
| 100 x 10 obj  | MessagePack |    1 |            | 0.608 sec | 0.003 sec | 0.002 sec |  228 | Size: 1.44 KB  |
|               | Avro        |    2 |  + 23.16 % | 0.738 sec | 0.003 sec | 0.003 sec |  228 | Size: 1.14 KB  |
|               | Protobuf    |    3 |  + 66.13 % | 0.969 sec | 0.004 sec | 0.004 sec |  228 | Size: 1.51 KB  |
|               | Json        |    4 | + 382.12 % | 2.691 sec | 0.012 sec | 0.011 sec |  228 | Size: 3.04 KB  |
| ------------- | ----------- | ---- | ---------- | --------- | --------- | --------- | ---- | -------------- |
| 100 x 100 obj | MessagePack |    1 |            | 0.606 sec | 0.023 sec | 0.024 sec |   26 | Size: 14.36 KB |
|               | Avro        |    2 |  + 17.71 % | 0.754 sec | 0.029 sec | 0.028 sec |   26 | Size: 11.42 KB |
|               | Protobuf    |    3 |  + 44.71 % | 0.978 sec | 0.038 sec | 0.035 sec |   26 | Size: 15.06 KB |
|               | Json        |    4 | + 350.81 % | 2.789 sec | 0.107 sec | 0.108 sec |   26 | Size: 30.28 KB |
| ------------- | ----------- | ---- | ---------- | --------- | --------- | --------- | ---- | -------------- |
```

### ToMarkdown(), ToMarkdown(RankColumn column)

Good for posting your results on github:

| Context | Candidate | Rank | +/- Median | Total | Average | Median | Runs | Comment |
| --- | --- | --- | --- | --- | --- | --- | --- | --- |
| 100 x 10 obj | MessagePack | 1 |  | 0.608 sec | 0.003 sec | 0.002 sec | 228 | Size: 1.44 KB |
|  | Avro | 2 | + 23.16 % | 0.738 sec | 0.003 sec | 0.003 sec | 228 | Size: 1.14 KB |
|  | Protobuf | 3 | + 66.13 % | 0.969 sec | 0.004 sec | 0.004 sec | 228 | Size: 1.51 KB |
|  | Json | 4 | + 382.12 % | 2.691 sec | 0.012 sec | 0.011 sec | 228 | Size: 3.04 KB |
|   |   |   |   |   |   |   |   |   |
| 100 x 100 obj | MessagePack | 1 |  | 0.606 sec | 0.023 sec | 0.024 sec | 26 | Size: 14.36 KB |
|  | Avro | 2 | + 17.71 % | 0.754 sec | 0.029 sec | 0.028 sec | 26 | Size: 11.42 KB |
|  | Protobuf | 3 | + 44.71 % | 0.978 sec | 0.038 sec | 0.035 sec | 26 | Size: 15.06 KB |
|  | Json | 4 | + 350.81 % | 2.789 sec | 0.107 sec | 0.108 sec | 26 | Size: 30.28 KB |
|   |   |   |   |   |   |   |   |   |

#### ToJson()

Returns results in json format

#### IBenchmarkComment

If your [IBenchmarkCandidate](https://github.com/tm011064/Benchmark/blob/master/src/Benchmark/IBenchmarkCandidate.cs) also implements the [IBenchmarkComment](https://github.com/tm011064/Benchmark/blob/master/src/Benchmark/IBenchmarkComment.cs) interface you can render a comment for each candidate per context. You can use this method to output additional information gathered during tests.

## Warm Up Runs
The [Measure](https://github.com/tm011064/Benchmark/blob/master/src/Benchmark/Measure.cs) builder allows you to specify a number of warm up runs for your algorithm to counter JIT compilation influencing your results. You can use the `.NumberOfWarmUpRuns(...)` method to specify how many warm up runs to perform. You can also pass in a warm up context if needed.

By default, Benchmark will do one warm up run for each test context. To disable warm up runs, define `.NumberOfWarmUpRuns(0)`.
