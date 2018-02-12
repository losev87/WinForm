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
using Encog.ML.Genetic.Genes;
using Encog.ML.Genetic.Genome;
using Encog.MathUtil;

namespace Encog.ML.Genetic.Mutate
{
    /// <summary>
    /// A simple mutation where genes are shuffled.
    /// This mutation will not produce repeated genes.
    /// </summary>
    ///
    public class MutateShuffle : IMutate
    {
        #region IMutate Members

        /// <summary>
        /// Perform a shuffle mutation.
        /// </summary>
        ///
        /// <param name="chromosome">The chromosome to mutate.</param>
        public void PerformMutation(Chromosome chromosome)
        {
            int length = chromosome.Genes.Count;
            var iswap1 = (int)(ThreadSafeRandom.NextDouble()*length);
            var iswap2 = (int)(ThreadSafeRandom.NextDouble() * length);

            // can't be equal
            if (iswap1 == iswap2)
            {
                // move to the next, but
                // don't go out of bounds
                if (iswap1 > 0)
                {
                    iswap1--;
                }
                else
                {
                    iswap1++;
                }
            }

            // make sure they are in the right order
            if (iswap1 > iswap2)
            {
                int temp = iswap1;
                iswap1 = iswap2;
                iswap2 = temp;
            }

            IGene gene1 = chromosome.Genes[iswap1];
            IGene gene2 = chromosome.Genes[iswap2];

            // remove the two genes
            chromosome.Genes.Remove(gene1);
            chromosome.Genes.Remove(gene2);

            // insert them back in, reverse order
            chromosome.Genes.Insert(iswap1, gene2);
            chromosome.Genes.Insert(iswap2, gene1);
        }

        #endregion
    }
}
