﻿using Redzen.Random;
using SharpNeat.EA;
using SharpNeat.Evaluation;
using SharpNeat.Neat;
using SharpNeat.Neat.DistanceMetrics.Double;
using SharpNeat.Neat.EvolutionAlgorithm;
using SharpNeat.Neat.Genome;
using SharpNeat.Neat.Genome.Double;
using SharpNeat.Neat.Speciation.GeneticKMeans;
using SharpNeat.Phenomes;
using SharpNeatTasks.BinaryElevenMultiplexer;

namespace TestApp1
{
    public class EAFactory
    {
        NeatEvolutionAlgorithmSettings _eaSettings;
        MetaNeatGenome<double> _metaNeatGenome;
        NeatPopulation<double> _neatPop;

        #region Public Methods

        public NeatEvolutionAlgorithm<double> CreateNeatEvolutionAlgorithm()
        {
            // Create an initial population.
            _metaNeatGenome = CreateMetaNeatGenome();
            _eaSettings = new NeatEvolutionAlgorithmSettings();
            _eaSettings.SpeciesCount = 6;
            _neatPop = CreatePopulation(_metaNeatGenome, 100);

            // Create a genome evaluator.
            IGenomeListEvaluator<NeatGenome<double>> genomeListEvaluator = CreateGenomeListEvaluator();

            // Create a speciation strategy instance.
            var distanceMetric = new EuclideanDistanceMetric();
            var speciationStrategy = new GeneticKMeansSpeciationStrategy<double>(distanceMetric, 5);

            // Pull all of the parts together into an evolution algorithm instance.
            var ea = new NeatEvolutionAlgorithm<double>(
                _eaSettings,
                genomeListEvaluator,
                speciationStrategy,
                _neatPop);

            return ea;
        }

        #endregion

        #region Private Static Methods

        private static MetaNeatGenome<double> CreateMetaNeatGenome()
        {
            MetaNeatGenome<double> metaNeatGenome = new MetaNeatGenome<double>(
                inputNodeCount: 12, 
                outputNodeCount: 1,
                isAcyclic: true,
                activationFn: new SharpNeat.NeuralNet.Double.ActivationFunctions.ReLU());

            return metaNeatGenome;
        }

        private static NeatPopulation<double> CreatePopulation(
            MetaNeatGenome<double> metaNeatGenome,
            int popSize)
        {
            NeatPopulation<double> pop = NeatPopulationFactory<double>.CreatePopulation(
                metaNeatGenome,
                connectionsProportion: 1.0,
                popSize: popSize,
                rng: RandomDefaults.CreateRandomSource());

            return pop;
        }

        private IGenomeListEvaluator<NeatGenome<double>> CreateGenomeListEvaluator()
        {
            var genomeDecoder = new NeatGenomeAcyclicDecoder(true);
            var phenomeEvaluator = new BinaryElevenMultiplexerEvaluator();
            var genomeListEvaluator = new SerialGenomeListEvaluator<NeatGenome<double>, IPhenome<double>>(genomeDecoder, phenomeEvaluator);
            return genomeListEvaluator;
        }

        #endregion
    }
}