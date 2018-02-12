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
using System.IO;
using Encog.ML.Data;
using Encog.ML.Data.Market;
using Encog.ML.Data.Market.Loader;
using Encog.ML.SVM;
using Encog.ML.SVM.Training;
using Encog.ML.Train.Strategy;
using Encog.Neural.Networks;
using Encog.Persist;
using Encog.Util.File;
using Encog.Util.Simple;

namespace Encog.Examples.SVMPredictCSV
{
    public class MarketBuildTraining
    {

        private static void MakeAPause()
        {
            Console.WriteLine("Press a key to continue ..");
            Console.ReadKey();
        }


        public static void Generate(string fileName)
        {

          
            FileInfo dataDir = new FileInfo(@Environment.CurrentDirectory);
            IMarketLoader loader = new CSVFinal();
            var market = new MarketMLDataSet(loader,CONFIG.INPUT_WINDOW, CONFIG.PREDICT_WINDOW);
          //  var desc = new MarketDataDescription(Config.TICKER, MarketDataType.Close, true, true);

            var desc = new MarketDataDescription(CONFIG.TICKER, MarketDataType.Close, true, true);
            market.AddDescription(desc);
            string currentDirectory =@"c:\";
            loader.GetFile(fileName);

            var end = DateTime.Now; // end today
            var begin = new DateTime(end.Ticks); // begin 30 days ago

            // Gather training data for the last 2 years, stopping 60 days short of today.
            // The 60 days will be used to evaluate prediction.
            begin = begin.AddDays(-600);
            end = begin.AddDays(200);
           
            Console.WriteLine("You are loading date from:" + begin.ToShortDateString() + " To :" + end.ToShortDateString());

            market.Load(begin, end);
            market.Generate();
            EncogUtility.SaveEGB(FileUtil.CombinePath(dataDir, CONFIG.SVMTRAINING_FILE), market);

            // create a network
            //BasicNetwork network = EncogUtility.SimpleFeedForward(
            //    market.InputSize,
            //    CONFIG.HIDDEN1_COUNT,
            //    CONFIG.HIDDEN2_COUNT,
            //    market.IdealSize,
            //    true);


            SupportVectorMachine network = new SupportVectorMachine(CONFIG.INPUT_WINDOW, true);
            TrainNetworks(network, market);
            // save the network and the training
            EncogDirectoryPersistence.SaveObject(FileUtil.CombinePath(dataDir,CONFIG.SVMTRAINING_FILE), network);
        }


        public static double TrainNetworks(SupportVectorMachine network, MarketMLDataSet training)
        {
            // train the neural network
            SVMTrain trainMain = new SVMTrain(network, training);

            StopTrainingStrategy stop = new StopTrainingStrategy(0.0001, 200);
            trainMain.AddStrategy(stop);


            var sw = new Stopwatch();
            sw.Start();
            while (!stop.ShouldStop())
            {
                trainMain.PreIteration();


                trainMain.Iteration();
                trainMain.PostIteration();

                Console.WriteLine(@"Iteration #:" + trainMain.IterationNumber + @" Error:" + trainMain.Error);
            }
            sw.Stop();
            Console.WriteLine("SVM Trained in :" + sw.ElapsedMilliseconds + "For error:" + trainMain.Error + " Iterated:" + trainMain.IterationNumber);
            return trainMain.Error;
        }
    }
}
