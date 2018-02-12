using System;
using SharpML.Reccurent.Examples;

namespace EncogConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            //var example = new MultiThreadBenchmark();
            //example.Execute(null);
            ExampleXor.Run();
            Console.ReadLine();
        }
    }
}
