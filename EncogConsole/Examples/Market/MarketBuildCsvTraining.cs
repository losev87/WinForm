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
using System.IO;
using Encog.ML.Data.Market;
using Encog.ML.Data.Market.Loader;
using Encog.Neural.Networks;
using Encog.Persist;
using Encog.Util.File;
using Encog.Util.Simple;

namespace Encog.Examples.Market
{
    public class MarketBuildCsvTraining
    {
        public static void Generate(FileInfo dataDir)
        {
            IMarketLoader loader = new CSVFileLoader();
            CSVFileLoader.LoadedFile = "D:\\losev\\test\\1.csv";
            var market = new MarketMLDataSet(loader,
                                             Config.INPUT_WINDOW, Config.PREDICT_WINDOW);
            var desc = new MarketDataDescription(
                Config.TICKER, MarketDataType.Close, true, true);
            market.AddDescription(desc);

            var end = new DateTime(2006,4,13); // end today
            var begin = end.AddYears(-2); // begin 30 days ago

            // Gather training data for the last 2 years, stopping 60 days short of today.
            // The 60 days will be used to evaluate prediction.
            //begin = begin.AddDays(-460);
            //end = end.AddDays(-60);
            //begin = begin.AddYears(-2);

            market.Load(begin, end);
            market.Generate();
            EncogUtility.SaveEGB(FileUtil.CombinePath(dataDir, Config.TRAINING_FILE), market);

            // create a network
            BasicNetwork network = EncogUtility.SimpleFeedForward(
                market.InputSize,
                Config.HIDDEN1_COUNT,
                Config.HIDDEN2_COUNT,
                market.IdealSize,
                true);

            // save the network and the training
            EncogDirectoryPersistence.SaveObject(FileUtil.CombinePath(dataDir, Config.NETWORK_FILE), network);
        }
    }
}
