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

namespace Encog.Neural.NEAT.Training
{
    /// <summary>
    /// Defines a base class for NEAT genes. A neat gene holds instructions on how to
    /// create either a neuron or a link. The NEATLinkGene and NEATLinkNeuron classes
    /// extend NEATBaseGene to provide this specific functionality.
    /// 
    /// -----------------------------------------------------------------------------
    /// http://www.cs.ucf.edu/~kstanley/ Encog's NEAT implementation was drawn from
    /// the following three Journal Articles. For more complete BibTeX sources, see
    /// NEATNetwork.java.
    /// 
    /// Evolving Neural Networks Through Augmenting Topologies
    /// 
    /// Generating Large-Scale Neural Networks Through Discovering Geometric
    /// Regularities
    /// 
    /// Automatic feature selection in neuroevolution
    /// </summary>
    [Serializable]
    public class NEATBaseGene : IComparable<NEATBaseGene>
    {
        /// <summary>
        /// ID of this gene, -1 for unassigned.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Innovation ID, -1 for unassigned.
        /// </summary>
        public long InnovationId { get; set; }

        /// <summary>
        /// Construct the base gene.
        /// </summary>
        public NEATBaseGene()
        {
            Id = -1;
            InnovationId = -1;
        }

        /// <inheritdoc/>
        public int CompareTo(NEATBaseGene o)
        {
            return ((int)(InnovationId - o.InnovationId));
        }
    }
}
