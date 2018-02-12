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
using Encog.ML.EA.Exceptions;
using Encog.ML.EA.Genome;
using Encog.ML.EA.Train;
using Encog.Neural.Networks.Training;

namespace Encog.ML.EA.Score.Multi
{
    public class ParallelScoreTask
    {
        /// <summary>
        ///     The score adjusters.
        /// </summary>
        private readonly IList<IAdjustScore> adjusters;

        /// <summary>
        ///     The genome to calculate the score for.
        /// </summary>
        private readonly IGenome genome;

        /// <summary>
        ///     The owners.
        /// </summary>
        private readonly ParallelScore owner;

        /// <summary>
        ///     The score function.
        /// </summary>
        private readonly ICalculateScore scoreFunction;

        /// <summary>
        ///     Construct the parallel task.
        /// </summary>
        /// <param name="genome">The genome.</param>
        /// <param name="theOwner">The owner.</param>
        public ParallelScoreTask(IGenome genome, ParallelScore theOwner)
        {
            owner = theOwner;
            this.genome = genome;
            scoreFunction = theOwner.ScoreFunction;
            adjusters = theOwner.Adjusters;
        }

        /// <summary>
        ///     Perform the task.
        /// </summary>
        public void PerformTask()
        {
            IMLMethod phenotype = owner.CODEC.Decode(genome);
            if (phenotype != null)
            {
                double score;
                try
                {
                    score = scoreFunction.CalculateScore(phenotype);
                }
                catch (EARuntimeError e)
                {
                    score = Double.NaN;
                }
                genome.Score = score;
                genome.AdjustedScore = score;
                BasicEA.CalculateScoreAdjustment(genome, adjusters);
            }
        }
    }
}
