open System
open BenchmarkDotNet.Running
open benchmarks

[<EntryPoint>]
let main argv =
    let switcher = BenchmarkSwitcher.FromTypes(
        [|
            typeof<AppendBenchmark>
            typeof<ContainsBenchmark>
        |])
    switcher.Run(argv) |> ignore
    0 // return an integer exit code