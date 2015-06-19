module Neural

open Util
open Genetic
open NeuralTypes
open NeuralHelpers

// I don't want to have to check NeuralType.fs every time I want to know types.
//type Neuron<'source, 'meaning> =
//    | InputNeuron of id : int
//    | HiddenNeuron of incoming : seq<Neuron<'source, 'meaning> * double> * threshold : double
//    | OutputNeuron of incoming : seq<Neuron<'source, 'meaning> * double> * threshold : double * meaning : 'meaning
//
//type NeuralNetwork<'source, 'meaning> = seq<Neuron<'source, 'meaning>>

let rec resolve (isActivated:int -> 'source -> bool) =
    memoize (fun source neuron -> 
        match neuron with
        | InputNeuron inp -> if isActivated inp source then 1.0 else 0.0
        | HiddenNeuron (inc, thresh) -> if inc |> Seq.sumBy (fun (neur, weight) -> resolve isActivated source neur * weight) > thresh then 1.0 else 0.0
        | OutputNeuron (inc, thresh, _) -> if inc |> Seq.sumBy (fun (neur, weight) -> resolve isActivated source neur * weight) > thresh then 1.0 else 0.0
    )

let query<'source, 'meaning when 'source : equality and 'meaning : equality> (value:'source) (activated:int -> 'source -> bool) (network:NeuralNetwork<'source, 'meaning>) =
    network 
    |> Seq.filter (resolve activated value >> (<) 0.0) 
    |> Seq.map (fun n ->
        match n with 
        | OutputNeuron (_, _, meaning) -> meaning
        | _ -> failwith "Boom. You crashed (and burned). Don't create Neural Networks consisting of other stuff than OutputNeurons. I'd rather throw this exception than throw up at the sight of the type safe variant.")

let computeargc (inputNodes:int) (hiddenLayers:int) (nodesPerHidden:int) (outputNodes:int) =
    inputNodes * nodesPerHidden + (hiddenLayers - 1) * nodesPerHidden * nodesPerHidden + nodesPerHidden * outputNodes + nodesPerHidden * hiddenLayers + outputNodes

let makeNetwork a b c d e vals = NeuralHelpers.FromVals (a, b, c, d, e, vals)

let train<'source, 'meaning when 'source : comparison and 'meaning : comparison> activated inputNodes hiddenLayers nodesPerHidden outputNodes meanings initmin initmax size chosen mutation gens tests =
    genetic (fun inp ->
        let network = makeNetwork inputNodes hiddenLayers nodesPerHidden outputNodes meanings inp
        tests
        |> Seq.sumBy (fun ((source, output):'source * seq<'meaning>) -> (network
            |> query source activated
            |> Set.ofSeq
            |> (Set.intersect <| Set.ofSeq output)
            |> Set.count
            |> float)))
            (computeargc inputNodes hiddenLayers nodesPerHidden outputNodes)
            initmin
            initmax
            size
            chosen
            mutation
            gens
    |> makeNetwork inputNodes hiddenLayers nodesPerHidden outputNodes meanings
