using System;
using System.Timers;
using TPI;

namespace TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.SetBufferSize(200, Int16.MaxValue - 1);

            var receiver = new TPIReceiver("10.3.97.150", "admin", "password");
            var timer = new Timer(100);
            timer.Elapsed += (sender, e) =>
            {
                Output("Position", receiver.Position);
            };

            Output("SN", receiver.SN);
            Output("RxType", receiver.RxType);
            Output("UtcTime", receiver.UtcTime);
            Output("GpsTime", receiver.GpsTime);

            timer.Start();
            Console.ReadKey();
        }

        static void Output(string key, object value)
        {
            Console.Write("---");
            Console.Write(key);
            Console.Write("---\n");
            Console.WriteLine(value);
        }
    }
}
