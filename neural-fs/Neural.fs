module Neural

open Util
open Genetic
open NeuralTypes

let rec resolve isActivated =
    memoize (fun neuron source -> 
        match neuron with
        | InputNeuron inp -> if isActivated inp source then 1.0 else 0.0
        | HiddenNeuron (inc, thresh) -> if inc |> Seq.sumBy (fun (neur, weight) -> resolve isActivated neur source * weight) > thresh then 1.0 else 0.0
        | OutputNeuron (inc, thresh, _) -> if inc |> Seq.sumBy (fun (neur, weight) -> resolve isActivated neur source * weight) > thresh then 1.0 else 0.0
    )