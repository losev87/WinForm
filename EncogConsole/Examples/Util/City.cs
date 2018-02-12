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

namespace Encog.Examples.Util
{
    /// <summary>
    /// Holds the x and y location for a city in the traveling salesman problem.
    /// </summary>
    public class City
    {
        /// <summary>
        /// The city's x position.
        /// </summary>
        private readonly int xpos;

        /// <summary>
        /// The city's y position.
        /// </summary>
        private readonly int ypos;

        /// <summary>
        /// Construct a city.
        /// </summary>
        /// <param name="x">The city's x location.</param>
        /// <param name="y">The city's y location.</param>
        public City(int x, int y)
        {
            xpos = x;
            ypos = y;
        }

        /// <summary>
        /// The city's x position.
        /// </summary>
        private int X
        {
            get { return xpos; }
        }

        /// <summary>
        /// The city's y position.
        /// </summary>
        private int Y
        {
            get { return ypos; }
        }

        /// <summary>
        /// Returns how close the city is to another city.
        /// </summary>
        /// <param name="cother">The other city.</param>
        /// <returns>A distance.</returns>
        public int Proximity(City cother)
        {
            return Proximity(cother.X, cother.Y);
        }

        /// <summary>
        /// Returns how far this city is from a a specific point. This method uses
        /// the pythagorean theorum to calculate the distance.
        /// </summary>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
        /// <returns>The distance.</returns>
        private int Proximity(int x, int y)
        {
            int xdiff = xpos - x;
            int ydiff = ypos - y;
            return (int) Math.Sqrt(xdiff*xdiff + ydiff*ydiff);
        }
    }
}
