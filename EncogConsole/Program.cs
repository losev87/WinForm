using System;
using Encog.Examples.CSVMarketExample;
using SharpML.Reccurent.Examples;

namespace EncogConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            //var example = new MultiThreadBenchmark();
            //example.Execute(null);
            //ExampleXor.Run();
            new MarketPredict().Execute(null);
            Console.ReadLine();
        }
    }
}
