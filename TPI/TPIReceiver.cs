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

    public static class Constants
    {
        public const string IP_Regex = @"^[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}$";

        public const string IP_Invalid = "Invalid receiver address";
        public const string TPI_Object_Can_Not_Show = "This object can't be shown";

        // 0: protocol, 'http' or 'https'
        // 1: address, name or ip
        // 2: verb, show, set, reset, enable, disable, delete, download, upload
        // 3: an Object type on which the action is to be performed. Objects can be: A state or characteristic of the system, such as the Serial Number. A function control, such as ElevationMask or data logging Session. A file object, like a logged data file, or a firmware file.
        // 4: parameters, multi-pairs like &param=value&param=value
        public const string TPI_Base_Uri_Format = @"{0}://{1}";
        public const string TPI_Relative_Uri_Format = @"/prog/{0}?{1}{2}";

        public const string TPI_Verb_Show = "show";
        public const string TPI_Verb_Set = "set";
        public const string TPI_Verb_Reset = "reset";
        public const string TPI_Verb_Enable = "enable";
        public const string TPI_Verb_Disable = "disable";
        public const string TPI_Verb_Delete = "delete";
        public const string TPI_Verb_Download = "download";
        public const string TPI_Verb_Upload = "upload";
    }

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
