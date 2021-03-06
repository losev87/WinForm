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
using System.Text;
using ConsoleExamples.Examples;
using Encog.Neural.Networks;

namespace Encog.Examples.CSVMarketExample
{
    public class MarketPredict : IExample
    {
        public static ExampleInfo Info
        {
            get
            {
                var info = new ExampleInfo(
                    typeof (MarketPredict),
                    "csvmarket",
                    "Simple Market Prediction",
                    "Use csv data to predict direction of a stock or forex, futures instrument.");
                return info;
            }
        }

        #region IExample Members

        public void Execute(IExampleInterface app)
        {
            for (int a = 0; a < 10; a++)
            {
                string name = "";
                switch (a)
                {
                    case 0:
                        name = "Lukoil1440";
                        break;
                    //case 1: name = "DAI1440"; break;
                    //case 2: name = "EURUSD5"; break;
                    //case 3: name = "Gazprom1440"; break;
                    //case 4: name = "BMW1440"; break;
                    //case 5: name = "Sberbank1440"; break;
                    //case 6: name = "USDCHF5"; break;
                    //case 7: name = "USDJPY5"; break;
                    //case 8: name = "USDJPY5_1"; break;
                    //case 9: name = "XAUUSD5"; break;
                }

                if (string.IsNullOrEmpty(name)) continue;

                var forexFile = $"D:\\1\\{name}.csv";


                var dataDir = new FileInfo(AppDomain.CurrentDomain.BaseDirectory);

                //for (Config.DAYS_OFFSET = 0; Config.DAYS_OFFSET <= 300; Config.DAYS_OFFSET += 100)
                Config.TEST_STRATCH = 80;
                for (Config.TEST_OFFSET = 0; Config.TEST_OFFSET <= 200; Config.TEST_OFFSET += 50)
                    //for (Config.TEST_STRATCH = 0; Config.TEST_STRATCH <= 160; Config.TEST_STRATCH += 40)
                {
                    var resultFile = $"D:\\1\\_res_{name}_{Config.DAYS_OFFSET}_{Config.TEST_OFFSET}_{Config.TEST_STRATCH}.txt";
                        for (int i = 0; i < 3; i++)
                    {
                        Config.OFFSET = i;

                        for (int j = 0; j < 3; j++)
                        {
                            var timeStart = DateTime.Now;
                            Config.TRAINING_FILE = $"marketData({name}-{i}-{j}){DateTime.Now.Ticks}.egb";
                            Config.NETWORK_FILE = $"marketNetwork({name}-{i}-{j}){DateTime.Now.Ticks}.eg";

                            //public static readonly String TRAINING_FILE = $"marketData{DateTime.Now.Ticks}.egb";
                            //public static readonly String NETWORK_FILE = $"marketNetwork{DateTime.Now.Ticks}.eg";

                            MarketBuildTraining.Generate(forexFile);
                            var err = MarketTrain.Train(dataDir);
                            var best = MarketPrune.Incremental(dataDir);
                            var gErr = MarketEvaluate.Evaluate(dataDir, forexFile);

                            var result =
                                $"Offset = {i};\t trainErr = {err};\t best: {NetworkToString(best)};\t result = {gErr:0.00};\t timeTaken: {DateTime.Now - timeStart:g};\t startTime: {timeStart:g}";

                            File.AppendAllLines(resultFile, new[] {result});
                        }
                    }
                }
            }

            //if (app.Args.Length < 3)
            //{
            //    Console.WriteLine(@"MarketPredict [data dir] [generate/train/prune/evaluate] PathToFile");
            //    Console.WriteLine(@"e.g csvMarketPredict [data dir] [generate/train/prune/evaluate] c:\\EURUSD.csv");
            //}
            //else
            //{
            //    var dataDir1 = new FileInfo(app.Args[0]);
            //    if (String.Compare(app.Args[1], "generate", true) == 0)
            //    {
            //    }
            //    else if (String.Compare(app.Args[1], "train", true) == 0)
            //    {
            //    }
            //    else if (String.Compare(app.Args[1], "evaluate", true) == 0)
            //    {
            //    }
            //    else if (String.Compare(app.Args[1], "prune", true) == 0)
            //    {
            //        {
            //        }
            //    }
            //}
        }
        public static String NetworkToString(BasicNetwork network)
        {
            if (network != null)
            {
                var result = new StringBuilder();
                int num = 1;

                // display only hidden layers
                for (int i = 1; i < network.LayerCount - 1; i++)
                {
                    if (result.Length > 0)
                    {
                        result.Append(",");
                    }
                    result.Append("H");
                    result.Append(num++);
                    result.Append("=");
                    result.Append(network.GetLayerNeuronCount(i));
                }

                return result.ToString();
            }
            else
            {
                return "N/A";
            }
        }

        #endregion
    }
}
