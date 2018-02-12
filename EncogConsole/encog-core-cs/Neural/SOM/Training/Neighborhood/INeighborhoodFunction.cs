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
namespace Encog.Neural.SOM.Training.Neighborhood
{
    /// <summary>
    /// Defines how a neighborhood function should work in competitive training. This
    /// is most often used in the training process for a self-organizing map. This
    /// function determines to what degree the training should take place on a
    /// neuron, based on its proximity to the "winning" neuron.
    /// </summary>
    ///
    public interface INeighborhoodFunction
    {
        /// <summary>
        /// Set the radius.
        /// </summary>
        double Radius { 
            get;
            set; }

        /// <summary>
        /// Determine how much the current neuron should be affected by training
        /// based on its proximity to the winning neuron.
        /// </summary>
        ///
        /// <param name="currentNeuron">THe current neuron being evaluated.</param>
        /// <param name="bestNeuron">The winning neuron.</param>
        /// <returns>The ratio for this neuron's adjustment.</returns>
        double Function(int currentNeuron, int bestNeuron);
    }
}
