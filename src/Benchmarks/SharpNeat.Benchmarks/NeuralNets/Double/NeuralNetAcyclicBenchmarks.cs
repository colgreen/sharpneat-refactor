﻿using BenchmarkDotNet.Attributes;
using Redzen.Random;
using SharpNeat.Neat.Genome;
using SharpNeat.Neat.Genome.Double;
using SharpNeat.Neat.Genome.IO;

namespace SharpNeat.NeuralNets.Double.Benchmarks
{
    public class NeuralNetAcyclicBenchmarks
    {
        static readonly NeuralNetAcyclic __nn;

        static NeuralNetAcyclicBenchmarks()
        {
            // TODO: Load neural nets directly, instead of loading a genome and decoding.
            var metaNeatGenome = new MetaNeatGenome<double>(12, 1, true, new ActivationFunctions.LeakyReLU());
            var genomeLoader = NeatGenomeLoaderFactory.CreateLoaderDouble(metaNeatGenome);
            var genome = genomeLoader.Load("data/genomes/binary11.genome");

            var genomeDecoder = new NeatGenomeDecoderAcyclic();
            __nn = (NeuralNetAcyclic)genomeDecoder.Decode(genome);

            // Set some non-zero random input values.
            var rng = RandomDefaults.CreateRandomSource();
            for(int i=0; i < __nn.InputVector.Length; i++)
            {
                __nn.InputVector[i] = rng.NextDouble();
            }
        }

        [Benchmark]
        public void Activate()
        {
            for(int i=0; i < 1000; i++)
            {
                __nn.Activate();
            }
        }
    }
}
