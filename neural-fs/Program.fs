module Program

open System
open Neural
open NeuralHelpers

[<EntryPoint>]
let main argv = 
    printfn "Hello World"
    NeuralHelpers.FromVals (2, 1, 3, 2, ["Test 1"; "Test 2"], Seq.init 17 (fun x -> x + 1 |> float)) |> ignore
    Console.ReadKey true |> ignore
    0
