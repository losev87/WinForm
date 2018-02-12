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
using ConsoleExamples.Examples;
using Encog.Examples.Analyzer;
using Encog.Examples.RangeandMarket;
using Encog.ML.Data;
using Encog.ML.Data.Basic;
using Encog.ML.Data.Temporal;
using Encog.ML.SVM;
using Encog.ML.SVM.Training;
using Encog.ML.Train;
using Encog.ML.Train.Strategy;
using Encog.Neural.Networks;
using Encog.Examples.SVMPredictCSV;
using Encog.Neural.Networks.Training;
using Encog.Neural.Networks.Training.Anneal;
using Encog.Neural.Networks.Training.Propagation.Back;
using Encog.Util.Arrayutil;
using SuperUtils = Encog.Util.NetworkUtil.NetworkUtility;
namespace Encog.Examples.SVMPredictCSV
{

    public class MarketPredict
    {
        
        private static void MakeAPause()
        {
            Console.WriteLine("Press a key to continue ..");
            Console.ReadKey();
        }





        #region elhman network trainer
        public static void TrainElmhanNetwork(ref IExampleInterface app)
        {
            BasicMLDataSet set = CreateEval.CreateEvaluationSetAndLoad(app.Args[1], CONFIG.STARTING_YEAR,
                                                                                              CONFIG.TRAIN_END,
                                                                                              CONFIG.INPUT_WINDOW,
                                                                                              CONFIG.PREDICT_WINDOW);

            //create our network.
            BasicNetwork network =
                (BasicNetwork)CreateEval.CreateElmanNetwork(CONFIG.INPUT_WINDOW, CONFIG.PREDICT_WINDOW);

            //Train it..

            double LastError = CreateEval.TrainNetworks(network, set);

            Console.WriteLine("NetWork Trained to :" + LastError);
            SuperUtils.SaveTraining(CONFIG.DIRECTORY, CONFIG.TRAINING_FILE, set);
            SuperUtils.SaveNetwork(CONFIG.DIRECTORY, CONFIG.NETWORK_FILE, network);
            Console.WriteLine("Network Saved to :" + CONFIG.DIRECTORY + " File Named :" +
                              CONFIG.NETWORK_FILE);

            Console.WriteLine("Training Saved to :" + CONFIG.DIRECTORY + " File Named :" +
                              CONFIG.TRAINING_FILE);
            MakeAPause();
        }
        #endregion




        public static IMLDataSet generateTraining(double [] array)
        {
            TemporalWindowArray temp = new TemporalWindowArray(CONFIG.INPUT_WINDOW, 1);
            temp.Analyze(array);
            return temp.Process(array);
        }

        public static SupportVectorMachine createNetwork()
        {
            SupportVectorMachine network = new SupportVectorMachine(CONFIG.INPUT_WINDOW, true);
            return network;
        }

        public static double  train(SupportVectorMachine network, IMLDataSet training)
        {
            SVMTrain train = new SVMTrain(network, training);
            train.Iteration();
            return train.Error;
        }



        public static double TrainNetworks(SupportVectorMachine network, IMLDataSet training)
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
            Console.WriteLine("SVM Trained in :" + sw.ElapsedMilliseconds + "For error:"+trainMain.Error +" Iterated:"+trainMain.IterationNumber);
            return trainMain.Error;
        }

        public static void TrainSVMNetwork(ref IExampleInterface app)
        {
            //BasicMLDataSet set = CreateEval.CreateEvaluationSetAndLoad(app.Args[1],1000,500,CONFIG.INPUT_WINDOW,CONFIG.PREDICT_WINDOW);


            TemporalMLDataSet Tempo = CreateEval.GenerateATemporalSet(app.Args[1], 1000, 500, CONFIG.INPUT_WINDOW, CONFIG.PREDICT_WINDOW);

            SupportVectorMachine machine =  createNetwork();

            //Train it..
            double error = TrainNetworks(machine, Tempo);
            Console.WriteLine(@"SVM NetWork Trained to :" +error);
            SuperUtils.SaveTraining(CONFIG.DIRECTORY, CONFIG.SVMTRAINING_FILE, Tempo);
            SuperUtils.SaveNetwork(CONFIG.DIRECTORY, CONFIG.SVMNETWORK_FILE, machine);
            
            Console.WriteLine(@"Network Saved to :" + CONFIG.DIRECTORY + @" File Named :" +
                              CONFIG.SVMNETWORK_FILE);
            Console.WriteLine(@"Training Saved to :" + CONFIG.DIRECTORY + @" File Named :" +
                              CONFIG.SVMTRAINING_FILE);
            MakeAPause();
        }


      

    }
}
