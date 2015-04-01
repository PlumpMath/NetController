using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TPI
{
    public enum TPIProtocols
    {
        Http,
        Https
    }

    //public enum TPIObjectTypes
    //{
    //    SerialNumber,
    //    UtcTime,
    //    GpsTime,
    //    Position,
    //    Voltages,
    //    Temperature,
    //    Commands,
    //    TrackingStatus,
    //    Tracking,
    //    GpsSatControls,
    //    SbasSatControls,
    //    GlonassSatControls,
    //    Ephemeris,
    //    Almanac,
    //    GpsHealth,
    //    GpsUtcData,
    //    GpsIonoData,
    //    GnssData,
    //    System,
    //    ReferenceFrequency,
    //    ElevationMask,
    //    PdopMask,
    //    ClockSteering,
    //    MultipathReject,
    //    PPS,
    //    AntennaTypes,
    //    Antenna,
    //    RtkControls,
    //    IoPorts,
    //    IoPort,
    //    RefStation,
    //    OmniStarSeed,
    //    FirmwareVersion,
    //    FirmwareWarranty,
    //    FirmwareFile,
    //}

    public class TPIReceiver : DisposableClass
    {
        private HttpClient client;

        public TPIReceiver(string address, string user = "", string password = "", TPIProtocols protocol = TPIProtocols.Http)
        {
            if (!Regex.IsMatch(address, Constants.IP_Regex))
            {
                throw new ArgumentException(Constants.IP_Invalid);
            }
            Address = address;
            client = new HttpClient();
            if (!string.IsNullOrEmpty(user) && !string.IsNullOrEmpty(password))
            {
                client.BaseAddress = new Uri(string.Format(Constants.TPI_Base_Uri_Format, protocol, address));
                var byteArray = Encoding.UTF8.GetBytes(user + ":" + password);
                var header = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
                client.DefaultRequestHeaders.Authorization = header;
            }
        }

        public TPIProtocols Protocol { get; private set; }

        public string Address { get; private set; }

        public override void FreeManagedResources()
        {
            client.Dispose();
            base.FreeManagedResources();
        }

        public T Create<T>() where T : TPIObject
        {
            var info = typeof(T).GetTypeInfo();
            foreach (var ctor in info.DeclaredConstructors)
            {
                if (ctor.IsConstructor)
                {
                    var obj = (T)ctor.Invoke(null);
                    obj.Initialize(this);
                    return obj;
                }
            }

            return default(T);
        }

        internal async Task<string> GetStringAsync(string verb, string objName, string parameters = "")
        {
            return await client.GetStringAsync(string.Format(Constants.TPI_Relative_Uri_Format, verb, objName, parameters));
        }
    }
}
