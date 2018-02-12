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
using System.Text;
using Encog.ML.EA.Genome;
using Encog.ML.EA.Population;

namespace Encog.ML.Genetic
{
    /// <summary>
    /// A factory to create MLMethod based genomes.
    /// </summary>
    public class MLMethodGenomeFactory : IGenomeFactory
    {
        public delegate IMLMethod CreateMethod();

        /// <summary>
        /// The MLMethod factory.
        /// </summary>
        private CreateMethod factory;

        /// <summary>
        /// The population.
        /// </summary>
        private IPopulation population;

        /// <summary>
        /// Construct the genome factory.
        /// </summary>
        /// <param name="theFactory">The factory.</param>
        /// <param name="thePopulation">The population.</param>
        public MLMethodGenomeFactory(CreateMethod theFactory,
                IPopulation thePopulation)
        {
            this.factory = theFactory;
            this.population = thePopulation;
        }

        /// <inheritdoc/>
        public IGenome Factor()
        {
            IGenome result = new MLMethodGenome(
                    (IMLEncodable)this.factory());
            result.Population = this.population;
            return result;
        }

        /// <inheritdoc/>
        public IGenome Factor(IGenome other)
        {
            MLMethodGenome result = (MLMethodGenome)Factor();
            result.Copy(other);
            result.Population = this.population;
            return result;
        }
    }
}
