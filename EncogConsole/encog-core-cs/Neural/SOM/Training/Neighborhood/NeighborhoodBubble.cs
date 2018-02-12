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

namespace Encog.Neural.SOM.Training.Neighborhood
{
    /// <summary>
    /// A neighborhood function that uses a simple bubble. A radius is defined, and
    /// any neuron that is plus or minus that width from the winning neuron will be
    /// updated as a result of training.
    /// </summary>
    ///
    public class NeighborhoodBubble : INeighborhoodFunction
    {
        /// <summary>
        /// The radius of the bubble.
        /// </summary>
        ///
        private double _radius;

        /// <summary>
        /// Create a bubble neighborhood function that will return 1.0 (full update)
        /// for any neuron that is plus or minus the width distance from the winning
        /// neuron.
        /// </summary>
        ///
        /// <param name="radius">bubble, is actually two times this parameter.</param>
        public NeighborhoodBubble(int radius)
        {
            _radius = radius;
        }

        #region INeighborhoodFunction Members

        /// <summary>
        /// Determine how much the current neuron should be affected by training
        /// based on its proximity to the winning neuron.
        /// </summary>
        ///
        /// <param name="currentNeuron">THe current neuron being evaluated.</param>
        /// <param name="bestNeuron">The winning neuron.</param>
        /// <returns>The ratio for this neuron's adjustment.</returns>
        public double Function(int currentNeuron, int bestNeuron)
        {
            int distance = Math.Abs(bestNeuron - currentNeuron);
            if (distance <= _radius)
            {
                return 1.0d;
            }
            return 0.0d;
        }

        /// <summary>
        /// Set the radius.
        /// </summary>
        public virtual double Radius
        {
            get { return _radius; }
            set { _radius = value; }
        }

        #endregion
    }
}
