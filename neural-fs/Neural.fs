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
        | _ -> failwith "Boom. You crashed (and burned). Don't create Neural Networks consisting of other stuff than OutputNeurons. But I'd rather throw this exception than throw up at the sight of the type safe variant.")

let train<'source, 'meaning> (network:NeuralNetwork<'source, 'meaning>) (tests:seq<'source * seq<'meaning>>) =
    0