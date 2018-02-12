using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpML.Reccurent.Examples.Data;
using SharpML.Recurrent;
using SharpML.Recurrent.DataStructs;
using SharpML.Recurrent.Models;
using SharpML.Recurrent.Networks;
using SharpML.Recurrent.Trainer;
using SharpML.Recurrent.Util;

namespace SharpML.Reccurent.Examples
{
    public class ExampleXor
    {
        public static void Run()
        {
            Random rnd = new Random();
            DataSet data = new XorDataSetGenerator();

            int inputDimension = 2;
            int hiddenDimension = 3;
            int outputDimension = 1;
            int hiddenLayers = 1;
            double learningRate = 0.001;
            double initParamsStdDev = 0.08;

            INetwork nn = NetworkBuilder.MakeFeedForward(inputDimension,
                hiddenDimension,
                hiddenLayers,
                outputDimension,
                data.GetModelOutputUnitToUse(),
                data.GetModelOutputUnitToUse(),
                initParamsStdDev, rnd);

            INetwork nn1 = NetworkBuilder.MakeLstm(inputDimension,
                hiddenDimension,
                hiddenLayers,
                outputDimension,
                data.GetModelOutputUnitToUse(),
                initParamsStdDev, rnd);


            int reportEveryNthEpoch = 10;
            int trainingEpochs = 100000;

            Trainer.train<NeuralNetwork>(ref trainingEpochs, learningRate, nn1, data, reportEveryNthEpoch, rnd);

            Console.WriteLine("Training Completed.");

            Matrix input = new Matrix(new double[] {1, 1});
            Graph g = new Graph(true);
            Matrix output = nn1.Activate(input, g);

            Console.WriteLine("Test: 1,1. Output:" + output.W[0]);

            Matrix input1 = new Matrix(new double[] { 0, 1 });
            Graph g1 = new Graph(false);
            Matrix output1 = nn1.Activate(input1, g1);

            Console.WriteLine("Test: 0,1. Output:" + output1.W[0]);

            Console.WriteLine("done.");
        }
    }
}
