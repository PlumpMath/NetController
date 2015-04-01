using System;
using System.Collections.Generic;
using TPI;

namespace TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var receiver = new TPIReceiver("10.3.97.150", "admin", "password");
            var shows = new List<ICanShow>()
            {
                receiver.Create<TPISerialNumber>(),
                receiver.Create<TPIUtcTime>(),
                receiver.Create<TPIGpsTime>(),
                receiver.Create<TPIPosition>(),
                receiver.Create<TPIVoltages>(),
                receiver.Create<TPITemperature>(),
                receiver.Create<TPICommands>()
            };

            shows.ForEach(x =>
                {
                    Console.WriteLine(x.Show().Result);
                    Console.ReadKey();
                });

            Console.ReadKey();
        }
    }
}
