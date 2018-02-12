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
using Encog.ML.EA.Genome;
using Encog.ML.EA.Sort;
using Encog.ML.EA.Train;

namespace Encog.ML.EA.Species
{
    /// <summary>
    ///     This speciation strategy simply creates a single species that contains the
    ///     entire population. Use this speciation strategy if you do not wish to use
    ///     species.
    /// </summary>
    [Serializable]
    public class SingleSpeciation : ISpeciation
    {
        /// <summary>
        ///     The trainer.
        /// </summary>
        private IEvolutionaryAlgorithm _owner;

        /// <summary>
        ///     The method used to sort the genomes in the species. More desirable
        ///     genomes should come first for later selection.
        /// </summary>
        private SortGenomesForSpecies _sortGenomes;

        /// <inheritdoc />
        public void Init(IEvolutionaryAlgorithm theOwner)
        {
            _owner = theOwner;
            _sortGenomes = new SortGenomesForSpecies(_owner);
        }

        /// <inheritdoc />
        public void PerformSpeciation(IList<IGenome> genomeList)
        {
            UpdateShare();
            ISpecies species = _owner.Population.Species[0];
            species.Members.Clear();
            species.Members = species.Members.Union(genomeList).ToList();
            species.Members.Sort(_sortGenomes);
            species.Leader = species.Members[0];
        }

        /// <inheritdoc />
        private void UpdateShare()
        {
            int speciesCount = _owner.Population.Species.Count;
            if (speciesCount != 1)
            {
                throw new EncogError(
                    "SingleSpeciation can only be used with a species count of 1, species count is "
                    + speciesCount);
            }

            ISpecies species = _owner.Population.Species[0];
            species.OffspringCount = _owner.Population.PopulationSize;
        }
    }
}
