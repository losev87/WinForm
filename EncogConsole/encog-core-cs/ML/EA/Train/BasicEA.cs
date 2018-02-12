//
// Encog(tm) Core v3.3 - .Net Version
// http://www.heatonresearch.com/encog/
//
// Copyright 2008-2014 Heaton Research, Inc.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//  http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//   
// For more information on Heaton Research copyrights, licenses 
// and trademarks visit:
// http://www.heatonresearch.com/copyright
//
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Encog.ML.EA.Codec;
using Encog.ML.EA.Genome;
using Encog.ML.EA.Opp;
using Encog.ML.EA.Opp.Selection;
using Encog.ML.EA.Population;
using Encog.ML.EA.Rules;
using Encog.ML.EA.Score;
using Encog.ML.EA.Score.Multi;
using Encog.ML.EA.Sort;
using Encog.ML.EA.Species;
using Encog.MathUtil.Randomize.Factory;
using Encog.Neural.Networks.Training;
using Encog.Util.Concurrency;

namespace Encog.ML.EA.Train
{
    /// <summary>
    ///     Provides a basic implementation of a multi-threaded Evolutionary Algorithm.
    ///     The EA works from a score function.
    /// </summary>
    [Serializable]
    public class BasicEA : IEvolutionaryAlgorithm, IMultiThreadable
    {
        /// <summary>
        ///     The score adjusters.
        /// </summary>
        private readonly IList<IAdjustScore> _adjusters = new List<IAdjustScore>();

        /// <summary>
        ///     The population for the next iteration.
        /// </summary>
        private readonly IList<IGenome> _newPopulation = new List<IGenome>();

        /// <summary>
        ///     The operators to use.
        /// </summary>
        private readonly OperationList _operators = new OperationList();

        /// <summary>
        ///     Has the first iteration occured.
        /// </summary>
        private bool _initialized;

        /// <summary>
        ///     The best genome from the last iteration.
        /// </summary>
        private IGenome _oldBestGenome;

        /// <summary>
        ///     The speciation method.
        /// </summary>
        private ISpeciation _speciation = new SingleSpeciation();

        /// <summary>
        ///     Construct an EA.
        /// </summary>
        /// <param name="thePopulation">The population.</param>
        /// <param name="theScoreFunction">The score function.</param>
        public BasicEA(IPopulation thePopulation,
                       ICalculateScore theScoreFunction)
        {
            RandomNumberFactory = EncogFramework.Instance.RandomFactory.FactorFactory();
            EliteRate = 0.3;
            MaxTries = 5;
            MaxOperationErrors = 500;
            CODEC = new GenomeAsPhenomeCODEC();


            Population = thePopulation;
            ScoreFunction = theScoreFunction;
            Selection = new TournamentSelection(this, 4);
            Rules = new BasicRuleHolder();

            // set the score compare method
            if (theScoreFunction.ShouldMinimize)
            {
                SelectionComparer = new MinimizeAdjustedScoreComp();
                BestComparer = new MinimizeScoreComp();
            }
            else
            {
                SelectionComparer = new MaximizeAdjustedScoreComp();
                BestComparer = new MaximizeScoreComp();
            }

            // set the iteration
            foreach (ISpecies species in thePopulation.Species)
            {
                foreach (IGenome genome in species.Members)
                {
                    IterationNumber = Math.Max(IterationNumber,
                                               genome.BirthGeneration);
                }
            }


            // Set a best genome, just so it is not null.
            // We won't know the true best genome until the first iteration.
            if (Population.Species.Count > 0 && Population.Species[0].Members.Count > 0)
            {
                BestGenome = Population.Species[0].Members[0];
            }
        }

        /// <summary>
        ///     Should exceptions be ignored.
        /// </summary>
        public bool IgnoreExceptions { get; set; }

        /// <summary>
        ///     Random number factory.
        /// </summary>
        public IRandomFactory RandomNumberFactory { get; set; }

        /// <summary>
        ///     The mutation to be used on the top genome. We want to only modify its
        ///     weights.
        /// </summary>
        public IEvolutionaryOperator ChampMutation { get; set; }

        /// <summary>
        ///     The percentage of a species that is "elite" and is passed on directly.
        /// </summary>
        public double EliteRate { get; set; }

        /// <summary>
        ///     The maximum number of errors to tolerate for the operators before stopping.
        ///     Because this is a stocastic process some operators will generated errors sometimes.
        /// </summary>
        public int MaxOperationErrors { get; set; }

        /// <summary>
        ///     The old best genome.
        /// </summary>
        public IGenome OldBestGenome
        {
            get { return _oldBestGenome; }
        }

        /// <summary>
        ///     The genome comparator.
        /// </summary>
        public IGenomeComparer BestComparer { get; set; }

        /// <summary>
        ///     The genome comparator.
        /// </summary>
        public IGenomeComparer SelectionComparer { get; set; }

        /// <summary>
        ///     The population.
        /// </summary>
        public IPopulation Population { get; set; }

        /// <summary>
        ///     The score calculation function.
        /// </summary>
        public ICalculateScore ScoreFunction { get; set; }

        /// <summary>
        ///     The selection operator.
        /// </summary>
        public ISelectionOperator Selection { get; set; }

        /// <summary>
        ///     The CODEC to use to convert between genome and phenome.
        /// </summary>
        public IGeneticCODEC CODEC { get; set; }

        /// <summary>
        ///     The current iteration.
        /// </summary>
        public int IterationNumber { get; set; }

        /// <summary>
        ///     The validation mode.
        /// </summary>
        public bool ValidationMode { get; set; }

        /// <summary>
        ///     The number of times to try certian operations, so an endless loop does
        ///     not occur.
        /// </summary>
        public int MaxTries { get; set; }

        /// <summary>
        ///     The best ever genome.
        /// </summary>
        public IGenome BestGenome { get; set; }

        /// <summary>
        ///     Holds rewrite and constraint rules.
        /// </summary>
        public IRuleHolder Rules { get; set; }

        /// <inheritdoc />
        public void AddOperation(double probability,
                                 IEvolutionaryOperator opp)
        {
            Operators.Add(probability, opp);
            opp.Init(this);
        }

        /// <inheritdoc />
        public void AddScoreAdjuster(IAdjustScore scoreAdjust)
        {
            _adjusters.Add(scoreAdjust);
        }

        /// <inheritdoc />
        public void CalculateScore(IGenome g)
        {
            // try rewrite
            Rules.Rewrite(g);

            // decode
            IMLMethod phenotype = CODEC.Decode(g);
            double score;

            // deal with invalid decode
            if (phenotype == null)
            {
                score = BestComparer.ShouldMinimize ? Double.PositiveInfinity : Double.NegativeInfinity;
            }
            else
            {
                var context = phenotype as IMLContext;
                if (context != null)
                {
                    context.ClearContext();
                }
                score = ScoreFunction.CalculateScore(phenotype);
            }

            // now set the scores
            g.Score = score;
            g.AdjustedScore = score;
        }

        /// <inheritdoc />
        public virtual void FinishTraining()
        {
        }

        /// <inheritdoc />
        public double Error
        {
            get
            {
                // do we have a best genome, and does it have an error?
                if (BestGenome != null)
                {
                    double err = BestGenome.Score;
                    if (!Double.IsNaN(err))
                    {
                        return err;
                    }
                }

                // otherwise, assume the worst!
                if (ScoreFunction.ShouldMinimize)
                {
                    return Double.PositiveInfinity;
                }
                return Double.NegativeInfinity;
            }
        }


        /// <inheritdoc />
        public void Iteration()
        {
            if (!_initialized)
            {
                PreIteration();
            }

            if (Population.Species.Count == 0)
            {
                throw new EncogError("Population is empty, there are no species.");
            }

            IterationNumber++;

            // Clear new population to just best genome.
            _newPopulation.Clear();
            _newPopulation.Add(BestGenome);
            _oldBestGenome = BestGenome;


            // execute species in parallel
            IList<EAWorker> threadList = new List<EAWorker>();
            foreach (ISpecies species in Population.Species)
            {
                int numToSpawn = species.OffspringCount;

                // Add elite genomes directly
                if (species.Members.Count > 5)
                {
                    var idealEliteCount = (int) (species.Members.Count*EliteRate);
                    int eliteCount = Math.Min(numToSpawn, idealEliteCount);
                    for (int i = 0; i < eliteCount; i++)
                    {
                        IGenome eliteGenome = species.Members[i];
                        if (_oldBestGenome != eliteGenome)
                        {
                            numToSpawn--;
                            if (!AddChild(eliteGenome))
                            {
                                break;
                            }
                        }
                    }
                }

                // now add one task for each offspring that each species is allowed
                while (numToSpawn-- > 0)
                {
                    var worker = new EAWorker(this, species);
                    threadList.Add(worker);
                }
            }

            // run all threads and wait for them to finish
            Parallel.ForEach(threadList, currentTask => currentTask.PerformTask());

            // validate, if requested
            if (ValidationMode)
            {
                if (_oldBestGenome != null
                    && !_newPopulation.Contains(_oldBestGenome))
                {
                    throw new EncogError(
                        "The top genome died, this should never happen!!");
                }

                if (BestGenome != null
                    && _oldBestGenome != null
                    && BestComparer.IsBetterThan(_oldBestGenome,
                                                 BestGenome))
                {
                    throw new EncogError(
                        "The best genome's score got worse, this should never happen!! Went from "
                        + _oldBestGenome.Score + " to "
                        + BestGenome.Score);
                }
            }

            _speciation.PerformSpeciation(_newPopulation);

            // purge invalid genomes
            Population.PurgeInvalidGenomes();

            PostIteration();
        }

        /// <summary>
        /// post iteration added to support strategies.
        /// </summary>
        public virtual void PostIteration()
        {

        }

        /// <summary>
        ///     The operators.
        /// </summary>
        public OperationList Operators
        {
            get { return _operators; }
        }


        public int MaxIndividualSize { get; set; }


        public IList<IAdjustScore> ScoreAdjusters
        {
            get { return _adjusters; }
        }

        public bool ShouldIgnoreExceptions { get; set; }

        public ISpeciation Speciation
        {
            get { return _speciation; }
            set { _speciation = value; }
        }

        /// <summary>
        ///     The desired thread count.
        /// </summary>
        public int ThreadCount { get; set; }

        /// <summary>
        ///     Calculate the score adjustment, based on adjusters.
        /// </summary>
        /// <param name="genome">The genome to adjust.</param>
        /// <param name="adjusters">The score adjusters.</param>
        public static void CalculateScoreAdjustment(IGenome genome,
                                                    IList<IAdjustScore> adjusters)
        {
            double score = genome.Score;
            double delta = adjusters.Sum(a => a.CalculateAdjustment(genome));

            genome.AdjustedScore = (score + delta);
        }

        /// <summary>
        ///     Add a child to the next iteration.
        /// </summary>
        /// <param name="genome">The child.</param>
        /// <returns>True, if the child was added successfully.</returns>
        public bool AddChild(IGenome genome)
        {
            lock (_newPopulation)
            {
                if (_newPopulation.Count < Population.PopulationSize)
                {
                    // don't readd the old best genome, it was already added
                    if (genome != _oldBestGenome)
                    {
                        if (ValidationMode)
                        {
                            if (_newPopulation.Contains(genome))
                            {
                                throw new EncogError(
                                    "Genome already added to population: "
                                    + genome);
                            }
                        }

                        _newPopulation.Add(genome);
                    }

                    if (!Double.IsInfinity(genome.Score)
                        && !Double.IsNaN(genome.Score)
                        && BestComparer.IsBetterThan(genome,
                                                     BestGenome))
                    {
                        BestGenome = genome;
                        Population.BestGenome = BestGenome;
                    }
                    return true;
                }
                return false;
            }
        }

        /// <summary>
        ///     Called before the first iteration. Determine the number of threads to
        ///     use.
        /// </summary>
        private void PreIteration()
        {
            _initialized = true;
            _speciation.Init(this);

            // score the population
            var pscore = new ParallelScore(Population,
                                           CODEC, _adjusters, ScoreFunction, ThreadCount);
            pscore.Process();

            // just pick the first genome with a valid score as best, it will be
            // updated later.
            // also most populations are sorted this way after training finishes
            // (for reload)
            // if there is an empty population, the constructor would have blow
            IList<IGenome> list = Population.Flatten();

            int idx = 0;
            do
            {
                BestGenome = list[idx++];
            } while (idx < list.Count
                     && (Double.IsInfinity(BestGenome.Score) || Double
                                                                    .IsNaN(BestGenome.Score)));

            Population.BestGenome = BestGenome;

            // speciate
            IList<IGenome> genomes = Population.Flatten();
            _speciation.PerformSpeciation(genomes);

            // purge invalid genomes
            Population.PurgeInvalidGenomes();
        }
    }
}
