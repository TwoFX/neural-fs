using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NeuralTypes;

namespace NeuralHelpers
{
    public class NeuralHelpers
    {
        public static IEnumerable<Neuron<TSource, TMeaning>> FromVals<TSource, TMeaning>(int inputNodes, int hiddenLayers, int hiddenNodesPerLayer, int outputNodes, IEnumerable<TMeaning> meanings, IEnumerable<double> values)
        {
            var startLayer = new Neuron<TSource, TMeaning>[inputNodes];
            Queue<double> vals = new Queue<double>(values);
            for (int i = 0; i < inputNodes; i++)
            {
                startLayer[i] = Neuron<TSource, TMeaning>.NewInputNeuron(i);
            }

            IEnumerable<Neuron<TSource, TMeaning>> currentLayer = startLayer;

            for (int i = 0; i < hiddenLayers; i++)
            {
                var nextLayer = new Neuron<TSource, TMeaning>[hiddenNodesPerLayer];
                for (int j = 0; j < hiddenNodesPerLayer; j++)
                {
                    nextLayer[j] = Neuron<TSource, TMeaning>.NewHiddenNeuron(currentLayer.Select(node => new Tuple<Neuron<TSource, TMeaning>, double>(node, vals.Dequeue())).ToArray(), vals.Dequeue());
                }
                currentLayer = nextLayer;
            }

            var outputLayer = new Neuron<TSource, TMeaning>[outputNodes];
            TMeaning[] ms = meanings.ToArray();
            for (int i = 0; i < outputNodes; i++)
            {
                outputLayer[i] = Neuron<TSource, TMeaning>.NewOutputNeuron(currentLayer.Select(node => new Tuple<Neuron<TSource, TMeaning>, double>(node, vals.Dequeue())).ToArray(), vals.Dequeue(), ms[i]);
            }
            return outputLayer;
        }

        public static double[] toVals<TSource, TMeaning>(IEnumerable<Neuron<TSource, TMeaning>> network)
        {
            return null;
        }
    }
}
