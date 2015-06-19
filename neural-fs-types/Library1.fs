namespace NeuralTypes

type Neuron<'source, 'meaning> =
    | InputNeuron of id : int
    | HiddenNeuron of incoming : seq<Neuron<'source, 'meaning> * double> * threshold : double
    | OutputNeuron of incoming : seq<Neuron<'source, 'meaning> * double> * threshold : double * meaning : 'meaning