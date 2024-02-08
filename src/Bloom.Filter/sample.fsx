#r "nuget: System.IO.Hashing, 8.0.0"
#load "Library.fs"
open System
open Bloom

let data = 
    [
        "Hello, World!"
        "Hello, World!!"
        "test"
        "test2"
    ]
    |> Seq.map (fun item -> System.Text.Encoding.UTF8.GetBytes item)
let filter = 
    data |> Seq.fold (fun filter item -> 
            Filter.append item filter) Filter.create

let test = System.Text.Encoding.UTF8.GetBytes "Hello, World!!"
filter |> Filter.contains test