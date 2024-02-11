module benchmarks 

open System
open BenchmarkDotNet
open BenchmarkDotNet.Attributes
open Bloom.Filter
open BenchmarkDotNet.Columns
open Experiments

[<MemoryDiagnoser>]
[<RankColumn>]
[<RPlotExporter>]
type AppendBenchmark () =
    [<Params(100_000)>]
    member val public iterations = 0 with get, set

    member val public data = [] with get, set

    [<IterationSetup>]
    member self.IterationSetup() =
        printfn "%s" "Iteration Setup"
        self.data <- [0..self.iterations] |> List.map (fun _ -> Guid.NewGuid().ToByteArray()) 
    
    [<Benchmark>]
    member this.AppendWithRefFilter () = 
        this.data 
        |> List.fold (fun filter item -> WithRecord.append item filter) WithRecord.create
    
    [<Benchmark>]
    member this.AppendWithStructFilter () = 
        this.data 
        |> List.fold (fun filter item -> WithStructRecord.append item filter) WithStructRecord.create

    [<Benchmark>]
    member this.AppendWithoutHashReuse () = 
        this.data 
        |> List.fold (fun filter item -> StructWithoutHashReuse.append item filter) StructWithoutHashReuse.create
    
    [<Benchmark(Baseline = true)>]
    member this.AppendBaseline () = 
        this.data 
        |> List.fold (fun filter item -> append item filter) create


[<MemoryDiagnoser>]
[<RankColumn>]
[<RPlotExporter>]
type ContainsBenchmark () =
    [<Params(100_000)>]
    member val public iterations = 0 with get, set

    member val public filter = create with get, set

    member val public lookup = (Guid.NewGuid().ToByteArray()) with get, set


    [<IterationSetup>]
    member self.IterationSetup() =
        printfn "%s" "Iteration Setup"
        self.filter <-
            [0..10]
            |> List.map (fun _ -> Guid.NewGuid().ToByteArray())
            |> List.fold (fun filter item -> append item filter) create
    
    [<Benchmark(Baseline = true)>]
    member this.AppendBaseline () = 
        this.filter |> contains this.lookup