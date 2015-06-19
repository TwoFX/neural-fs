module Program

open System
open Neural
open neural_cs

[<EntryPoint>]
let main argv = 
    printfn "Hello World"
    NeuralHelpers.FromVals (2, 1, 3, 2, Seq.init 17 (fun x -> x + 1 |> float), ["Test 1"; "Test 2"]) |> ignore
    Console.ReadKey true |> ignore
    0
