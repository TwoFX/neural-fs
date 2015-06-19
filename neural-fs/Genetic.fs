module Genetic

let rnd = System.Random()

let makeNew (mutation:double) (number:int) (winners:seq<seq<double>>) =
    Seq.map (fun genome -> Seq.init number (fun _ -> genome)) winners
    |> Seq.concat
    |> Seq.map 
        (Seq.map (fun x -> x * (rnd.NextDouble() * 2.0 * mutation + (1.0 - mutation))))

let rec mutate (fitness:seq<double> -> double) (size:int) (chosen:int) (mutation:double) (gens:int) (specimen:seq<seq<double>>) = 
        match gens with 
        | 0 -> specimen
        | _ -> Seq.sortBy fitness specimen |> Seq.skip (size - chosen) |> makeNew mutation (size / chosen) |> mutate fitness size chosen mutation (gens - 1) 

let genetic (fitness:seq<double> -> double) (argc:int) (initmin:double) (initmax:double) (size:int) (chosen:int) (mutation:double) (gens:int) =
    Seq.init size (fun _ -> Seq.init argc (fun _ -> rnd.NextDouble() * (initmax - initmin) + initmin)) |> mutate fitness size chosen mutation gens |> Seq.maxBy fitness
