using System;
using TPI;

namespace TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var receiver = new TPIReceiver("10.3.97.150", "admin", "password");
            //for (int i = 0; i < 100; i++)
            //    Console.WriteLine(receiver.ShowAsync(new TPIPosition()).Result);
            var obj = receiver.Create<TPICommands>();

            Console.ReadKey();
        }
    }
}
