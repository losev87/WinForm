//
// Encog(tm) Core v3.0 - .Net Version
// http://www.heatonresearch.com/encog/
//
// Copyright 2008-2011 Heaton Research, Inc.
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
using System.Collections.Generic;
using Encog.MathUtil;
using Encog.ML.Genetic.Genes;
using Encog.ML.Genetic.Genome;
using Enumerable = System.Linq.Enumerable;

namespace Encog.ML.Genetic.Crossover
{
    /// <summary>
    /// A simple cross over where genes are simply "spliced".
    /// Genes are not allowed to repeat.
    /// </summary>
    public class SpliceNoRepeat : ICrossover
    {
        /// <summary>
        /// The cut length.
        /// </summary>
        private readonly int _cutLength;

        /// <summary>
        /// Construct a splice crossover.
        /// </summary>
        /// <param name="cutLength">The cut length.</param>
        public SpliceNoRepeat(int cutLength)
        {
            _cutLength = cutLength;
        }

        #region ICrossover Members

        /// <summary>
        /// Assuming this chromosome is the "mother" mate with the passed in
        /// "father".
        /// </summary>
        /// <param name="mother">The mother.</param>
        /// <param name="father">The father.</param>
        /// <param name="offspring1">The first offspring.</param>
        /// <param name="offspring2">The second offspring.</param>
        public void Mate(Chromosome mother, Chromosome father,
                         Chromosome offspring1, Chromosome offspring2)
        {
            int geneLength = father.Genes.Count;

            // the chromosome must be cut at two positions, determine them
            var cutpoint1 = (int) (ThreadSafeRandom.NextDouble()*(geneLength - _cutLength));
            int cutpoint2 = cutpoint1 + _cutLength;

            // keep track of which genes have been taken in each of the two
            // offspring, defaults to false.
            IList<IGene> taken1 = new List<IGene>();
            IList<IGene> taken2 = new List<IGene>();

            // handle cut section
            for (int i = 0; i < geneLength; i++)
            {
                if (!((i < cutpoint1) || (i > cutpoint2)))
                {
                    offspring1.Genes[i].Copy(father.Genes[i]);
                    offspring2.Genes[i].Copy(mother.Genes[i]);
                    taken1.Add(offspring1.Genes[i]);
                    taken2.Add(offspring2.Genes[i]);
                }
            }

            // handle outer sections
            for (int i = 0; i < geneLength; i++)
            {
                if ((i < cutpoint1) || (i > cutpoint2))
                {
                    offspring1.Genes[i].Copy(
                        GetNotTaken(mother, taken1));
                    offspring2.Genes[i].Copy(
                        GetNotTaken(father, taken2));
                }
            }
        }

        #endregion

        /// <summary>
        /// Get a list of the genes that have not been taken before. This is useful
        /// if you do not wish the same gene to appear more than once in a
        /// chromosome.
        /// </summary>
        /// <param name="source">The pool of genes to select from.</param>
        /// <param name="taken">An array of the taken genes.</param>
        /// <returns>Those genes in source that are not taken.</returns>
        private static IGene GetNotTaken(Chromosome source,
                                         IList<IGene> taken)
        {
            int geneLength = source.Genes.Count;

            for (int i = 0; i < geneLength; i++)
            {
                IGene trial = source.Genes[i];

                bool found = Enumerable.Contains(taken, trial);
                if (!found)
                {
                    taken.Add(trial);
                    return trial;
                }
            }

            return null;
        }
    }
}
