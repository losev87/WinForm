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
using System;
using System.Collections.Generic;
using Encog.ML.Genetic.Population;

namespace Encog.ML.Genetic.Genome
{
    /// <summary>
    /// A genome is the basic blueprint for creating an organism in Encog. A genome
    /// is made up of one or more chromosomes, which are in turn made up of genes.
    /// </summary>
    ///
    public interface IGenome : IComparable<IGenome>
    {
        /// <summary>
        /// Set the adjusted score.
        /// </summary>
        double AdjustedScore { get; set; }


        /// <summary>
        /// Set the amount to spawn.
        /// </summary>
        double AmountToSpawn { get; set; }


        /// <value>The chromosomes that make up this genome.</value>
        IList<Chromosome> Chromosomes
        { 
            get;
        }


        /// <summary>
        /// Set the GA used by this genome. This is normally a transient field and
        /// only used during training.
        /// </summary>
        GeneticAlgorithm GA
        { 
            get;
            set;
        }


        /// <summary>
        /// Set the genome ID.
        /// </summary>
        long GenomeID
        { 
            get;
            set;
        }


        /// <value>The organism produced by this genome.</value>
        Object Organism
        {
            get;
        }


        /// <summary>
        /// Set the population that this genome belongs to.
        /// </summary>
        IPopulation Population
        {
            get;
            set;
        }


        /// <summary>
        /// Set the score.
        /// </summary>
        double Score
        { 
            get;
            set;
        }

        /// <returns>The number of genes in this genome.</returns>
        int CalculateGeneCount();

        /// <summary>
        /// Use the genes to update the organism.
        /// </summary>
        ///
        void Decode();

        /// <summary>
        /// Use the organism to update the genes.
        /// </summary>
        ///
        void Encode();


        /// <summary>
        /// Mate with another genome and produce two children.
        /// </summary>
        ///
        /// <param name="father">The father genome.</param>
        /// <param name="child1">The first child.</param>
        /// <param name="child2">The second child.</param>
        void Mate(IGenome father, IGenome child1, IGenome child2);
    }
}
