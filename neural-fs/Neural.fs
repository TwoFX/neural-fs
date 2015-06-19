module Neural

open Util
open Genetic

type Neuron<'source, 'meaning> =
    | InputNeuron of id : int
    | HiddenNeuron of incoming : seq<Neuron<'source, 'meaning> * double> * threshold : double
    | OutputNeuron of incoming : seq<Neuron<'source, 'meaning> * double> * threshold : double * meaning : 'meaning

let rec resolve isActivated =
    memoize (fun neuron source -> 
        match neuron with
        | InputNeuron inp -> if isActivated inp source then 1.0 else 0.0
        | HiddenNeuron (inc, thresh) -> if inc |> Seq.sumBy (fun (neur, weight) -> resolve isActivated neur source * weight) > thresh then 1.0 else 0.0
        | OutputNeuron (inc, thresh, _) -> if inc |> Seq.sumBy (fun (neur, weight) -> resolve isActivated neur source * weight) > thresh then 1.0 else 0.0
    )

let fromVals<'a, 'b> inpH hidW hidH outH vals meanings = 
    let inps = Seq.init inpH InputNeuron
    let rec makeHiddenLayers toGo height vals (layer:seq<Neuron<'a, 'b>>) =
        match toGo with
        | 0 -> layer
        | x -> Seq.init height (HiddenNeuron (Seq.zip layer vals, Seq.head vals) |> fix) |> makeHiddenLayers (toGo - 1) height (vals |> Seq.skip ((Seq.length layer + 1) * height))  
    let makeOutputLayers height vals meanings layer = Seq.init height (OutputNeuron (Seq.zip layer vals, Seq.head vals, Seq.head meanings) |> fix) // Falsch
    0