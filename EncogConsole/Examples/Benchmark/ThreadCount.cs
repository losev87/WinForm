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
using System.Diagnostics;
using ConsoleExamples.Examples;
using Encog.ML.Data;
using Encog.Neural.Networks;
using Encog.Neural.Networks.Layers;
using Encog.Neural.Networks.Training.Propagation.Resilient;
using Encog.Util.Banchmark;

namespace Encog.Examples.Benchmark
{
    public class ThreadCount : IExample
    {
        public const int INPUT_COUNT = 40;
        public const int HIDDEN_COUNT = 60;
        public const int OUTPUT_COUNT = 20;

        public static ExampleInfo Info
        {
            get
            {
                var info = new ExampleInfo(
                    typeof (ThreadCount),
                    "threadcount",
                    "Evaluate Thread Count Performance",
                    "Compare Encog performance at various thread counts.");
                return info;
            }
        }

        #region IExample Members

        public void Execute(IExampleInterface app)
        {
            for (int i = 1; i < 16; i++)
            {
                Perform(i);
            }
        }

        #endregion

        public void Perform(int thread)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            var network = new BasicNetwork();
            network.AddLayer(new BasicLayer(INPUT_COUNT));
            network.AddLayer(new BasicLayer(HIDDEN_COUNT));
            network.AddLayer(new BasicLayer(OUTPUT_COUNT));
            network.Structure.FinalizeStructure();
            network.Reset();

            IMLDataSet training = RandomTrainingFactory.Generate(1000, 50000,
                                                                 INPUT_COUNT, OUTPUT_COUNT, -1, 1);

            var rprop = new ResilientPropagation(network, training);
            rprop.ThreadCount = thread;
            for (int i = 0; i < 5; i++)
            {
                rprop.Iteration();
            }
            stopwatch.Stop();
            Console.WriteLine("Result with " + thread + " was " + stopwatch.ElapsedMilliseconds + "ms");
        }
    }
}
