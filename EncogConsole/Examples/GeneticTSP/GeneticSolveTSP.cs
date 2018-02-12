//
// Encog(tm) Core v3.2 - .Net Version
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
using System.Text;
using ConsoleExamples.Examples;
using Encog.Examples.Util;
using Encog.MathUtil;
using Encog.ML.Genetic;
using Encog.ML.Genetic.Crossover;
using Encog.ML.Genetic.Genome;
using Encog.ML.Genetic.Mutate;
using Encog.ML.EA.Train;
using Encog.ML.EA.Population;
using Encog.ML.EA.Species;
using Encog.Neural.Networks.Training;

namespace Encog.Examples.GeneticTSP
{
    public class GeneticSolveTSP : IExample
    {
        public const int CITIES = 50;
        public const int POPULATION_SIZE = 1000;
        public const double MUTATION_PERCENT = 0.1;
        public const double PERCENT_TO_MATE = 0.24;
        public const double MATING_POPULATION_PERCENT = 0.5;
        public const int CUT_LENGTH = CITIES/5;
        public const int MAP_SIZE = 256;
        public const int MAX_SAME_SOLUTION = 25;
        private IExampleInterface app;

        private City[] cities;
        private TrainEA genetic;

        public static ExampleInfo Info
        {
            get
            {
                var info = new ExampleInfo(
                    typeof (GeneticSolveTSP),
                    "tsp-genetic",
                    "Genetic Algorithm Traveling Salesman",
                    "Use a Genetic Algorithm to provide a solution for the traveling salesman problem (TSP).");
                return info;
            }
        }

        #region IExample Members

        /// <summary>
        /// Setup and solve the TSP.
        /// </summary>
        public void Execute(IExampleInterface app)
        {
            this.app = app;

            var builder = new StringBuilder();

            initCities();

            IPopulation pop = initPopulation();
		
		ICalculateScore score =  new TSPScore(cities);

		genetic = new TrainEA(pop,score);
		
		genetic.AddOperation(0.9,new SpliceNoRepeat(CITIES/3));
		genetic.AddOperation(0.1,new MutateShuffle());

		int sameSolutionCount = 0;
		int iteration = 1;
		double lastSolution = Double.MaxValue;

		while (sameSolutionCount < MAX_SAME_SOLUTION) {
			genetic.Iteration();

			double thisSolution = genetic.Error;

			builder.Length = 0;
			builder.Append("Iteration: ");
			builder.Append(iteration++);
			builder.Append(", Best Path Length = ");
			builder.Append(thisSolution);

			Console.WriteLine(builder.ToString());

			if (Math.Abs(lastSolution - thisSolution) < 1.0) {
				sameSolutionCount++;
			} else {
				sameSolutionCount = 0;
			}

			lastSolution = thisSolution;
		}

		Console.WriteLine("Good solution found:");
		displaySolution();
		genetic.FinishTraining();
        }

        #endregion

        /**
         * Place the cities in random locations.
         */

        private void initCities()
        {
            cities = new City[CITIES];
            for (int i = 0; i < cities.Length; i++)
            {
                var xPos = (int) (ThreadSafeRandom.NextDouble()*MAP_SIZE);
                var yPos = (int) (ThreadSafeRandom.NextDouble()*MAP_SIZE);

                cities[i] = new City(xPos, yPos);
            }
        }


        private IntegerArrayGenome RandomGenome() {
            Random rnd = new Random();
		IntegerArrayGenome result = new IntegerArrayGenome(cities.Length);
		int[] organism = result.Data;
		bool[] taken = new bool[cities.Length];

		for (int i = 0; i < organism.Length - 1; i++) {
			int icandidate;
			do {
				icandidate = (int) (rnd.NextDouble() * organism.Length);
			} while (taken[icandidate]);
			organism[i] = icandidate;
			taken[icandidate] = true;
			if (i == organism.Length - 2) {
				icandidate = 0;
				while (taken[icandidate]) {
					icandidate++;
				}
				organism[i + 1] = icandidate;
			}
		}
		return result;
	}

        private IPopulation initPopulation()
        {
            		IPopulation result = new BasicPopulation(POPULATION_SIZE, null);

		BasicSpecies defaultSpecies = new BasicSpecies();
		defaultSpecies.Population = result;
		for (int i = 0; i < POPULATION_SIZE; i++) {
			IntegerArrayGenome genome = RandomGenome();
			defaultSpecies.Members.Add(genome);
		}
		result.GenomeFactory = new IntegerArrayGenomeFactory(cities.Length);
		result.Species.Add(defaultSpecies);
		
		return result;
        }


        /**
         * Display the cities in the final path.
         */

        public void displaySolution()
        {
            bool first = true;

            foreach (int gene in ((IntegerArrayGenome)genetic.Population.BestGenome).Data)
            {
                if (!first)
                    Console.Write(">");
                Console.Write("" + gene);
                first = false;
            }

            Console.WriteLine(@"");
        }
    }
}
