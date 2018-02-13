using System;
using Encog.Examples.CSVMarketExample;

namespace EncogConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            //var example = new MultiThreadBenchmark();
            //example.Execute(null);
            new MarketPredict().Execute(null);
            Console.ReadLine();
        }
    }
}
