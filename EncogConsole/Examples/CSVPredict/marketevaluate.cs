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
using System.Globalization;
using System.IO;
using Encog.ML.Data;
using Encog.ML.Data.Market;
using Encog.ML.Data.Market.Loader;
using Encog.ML.Data.Temporal;
using Encog.Neural.Networks;
using Encog.Persist;
using Encog.Util;
using Encog.Util.File;

namespace Encog.Examples.CSVMarketExample
{
    public class MarketEvaluate
    {
        #region Direction enum

        public enum Direction
        {
            Up,
            Down
        } ;

        #endregion

        public static Direction DetermineDirection(double d)
        {
            return d < 0 ? Direction.Down : Direction.Up;
        }

        public static MarketMLDataSet GrabData(string newfileLoad)
        {
            IMarketLoader loader = new CSVFileLoader();//CSVLoader();
            loader.GetFile(newfileLoad);

            var result = new MarketMLDataSet(loader,
                                             Config.INPUT_WINDOW, Config.PREDICT_WINDOW);
          //  var desc = new MarketDataDescription(Config.TICKER,
                                              //   MarketDataType.Close, true, true);

            var desc = new MarketDataDescription(Config.TICKER,
                                     MarketDataType.Close, TemporalDataDescription.Type.PercentChange, true, true);
            result.AddDescription(desc);

            var begin = DateTime.ParseExact("03.01.2012", "dd.MM.yyyy", CultureInfo.CurrentCulture); // begin 30 days ago
            var end = DateTime.ParseExact("29.06.2012", "dd.MM.yyyy", CultureInfo.CurrentCulture); // begin 30 days ago

            result.Load(begin, end);
            result.Generate();

            return result;
        }

        public static double Evaluate(FileInfo dataDir,string filename)
        {
            FileInfo file = FileUtil.CombinePath(dataDir, Config.NETWORK_FILE);

            if (!file.Exists)
            {
                Console.WriteLine(@"Can't read file: " + file);
                return 0;
            }

            var network = (BasicNetwork) EncogDirectoryPersistence.LoadObject(file);

            MarketMLDataSet data = GrabData(filename);

            int count = 0;
            int correct = 0;
            foreach (IMLDataPair pair in data)
            {
                IMLData input = pair.Input;
                IMLData actualData = pair.Ideal;
                IMLData predictData = network.Compute(input);

                double actual = actualData[0];
                double predict = predictData[0];
                double diff = Math.Abs(predict - actual);

                Direction actualDirection = DetermineDirection(actual);
                Direction predictDirection = DetermineDirection(predict);

                if (actualDirection == predictDirection)
                    correct++;

                count++;


                Console.WriteLine(@"Day " + count + @":actual="
                                  + Format.FormatDouble(actual, 4) + @"(" + actualDirection + @")"
                                  + @",predict=" + Format.FormatDouble(predict, 4) + @"("
                                  + predictDirection + @")" + @",diff=" + diff);
            }
            double percent = correct/(double) count;
            Console.WriteLine(@"Direction correct:" + correct + @"/" + count);
            Console.WriteLine(@"Directional Accuracy:"
                              + Format.FormatPercent(percent));
            return percent;
        }
    }
}
