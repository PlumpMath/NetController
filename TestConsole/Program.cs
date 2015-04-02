using System;
using System.Collections;
using System.Timers;
using TPI;

namespace TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            //Console.SetBufferSize(80, Int16.MaxValue - 1);

            var receiver = new TPIReceiver("10.3.97.150", "admin", "password");
            var timer = new Timer(500);
            timer.Elapsed += (sender, e) =>
            {
                Output("Position", receiver.Position);
            };

            Output("SN", receiver.SN);
            Output("RxType", receiver.RxType);
            Output("UtcTime", receiver.UtcTime);
            Output("GpsTime", receiver.GpsTime);
            Output("Voltages", receiver.Voltages);
            Output("Temperature", receiver.Temperature);

            Output("FirmwareVersion", receiver.FirmwareVersion);
            Output("FirmwareVersionDate", receiver.FirmwareVersionDate);
            Output("FirmwareWarrantyDate", receiver.FirmwareWarrantyDate);

            Output("ElevationMask", receiver.ElevationMask);
            receiver.ElevationMask = DateTime.Now.Second;
            Output("ElevationMask After Update", receiver.ElevationMask);

            Output("PdopMask", receiver.PdopMask);
            receiver.PdopMask = DateTime.Now.Second;
            Output("PdopMask After Update", receiver.PdopMask);

            //Output("AntennaTypes", receiver.AntennaTypes);
            var ant = receiver.Antenna;
            Output("AntennaSetting", ant);
            var newant = receiver.AntennaTypes[85];
            ant.Type = newant.Type;
            ant.Name = newant.Name;
            ant.MeasureMethod = newant.MeasureMethods[0];
            ant.Height = ant.Height + 0.1;
            receiver.Antenna = ant;
            Output("AntennaSetting After Update", receiver.Antenna);
            //Output("Restart Result", receiver.RestartReceiver());

            //timer.Start();
            Console.ReadKey();
        }

        static void Output(string key, object value)
        {
            Console.Write("---");
            Console.Write(key);
            Console.Write("---\n");
            if (value is IList)
            {
                foreach (var obj in (value as IList))
                {
                    Console.WriteLine(obj);
                }
            }
            else
                Console.WriteLine(value);
        }
    }
}
