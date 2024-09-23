using System;
using System.Threading;

namespace HelloWorldApp
{
    class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Starting HelloWorld App...");

            // Infinite loop to print "Hello, World!" every 5 seconds
            while (true)
            {
                Console.WriteLine("Hello, World!");
                Thread.Sleep(5000); // Sleep for 5000 milliseconds (5 seconds)
            }
        }
    }
}
