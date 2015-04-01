using System;
using System.Linq;
using System.Collections.Generic;
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

    enum TPIObjectTypes
    {
        SerialNumber,
        UtcTime,
        GpsTime,
        Position,
        Voltages,
        Temperature,
        Commands,
        TrackingStatus,
        Tracking,
        GpsSatControls,
        SbasSatControls,
        GlonassSatControls,
        Ephemeris,
        Almanac,
        GpsHealth,
        GpsUtcData,
        GpsIonoData,
        GnssData,
        System,
        ReferenceFrequency,
        ElevationMask,
        PdopMask,
        ClockSteering,
        MultipathReject,
        PPS,
        AntennaTypes,
        Antenna,
        RtkControls,
        IoPorts,
        IoPort,
        RefStation,
        OmniStarSeed,
        FirmwareVersion,
        FirmwareWarranty,
        FirmwareFile,
    }

    enum TPIVerbTypes
    {
        Show,
        Set,
        Reset,
        Enable,
        Disable,
        Delete,
        Download,
        Upload
    }

    public class TPIReceiver : DisposableClass
    {
        private HttpClient client;
        private HashSet<string> supportedCmds;

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

            supportedCmds = new HashSet<string>();
            supportedCmds.Add("Show:Commands");

            ExecuteCommand(TPIVerbTypes.Show, TPIObjectTypes.Commands, pattern: Constants.TPI_Regex_Parse_Commands).ContinueWith(x =>
                {
                    if (x.Status == TaskStatus.RanToCompletion)
                    {
                        foreach (Match match in x.Result)
                        {
                            supportedCmds.Add(match.Groups[1].Value + ":" + match.Groups[2].Value);
                        }
                    }
                }).Wait();
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

        private async Task<MatchCollection> ExecuteCommand(TPIVerbTypes verb, TPIObjectTypes obj, string parameters = "", string pattern = "")
        {
            if (!supportedCmds.Contains(verb + ":" + obj))
            {
                throw new NotSupportedException(Constants.TPI_Not_Supported_Cmd);
            }
            var ret = GetStringAsync(verb.ToString(), obj.ToString(), parameters);
            var matches = Regex.Matches(await ret, pattern, RegexOptions.IgnoreCase);
            return matches;
        }

        internal async Task<string> GetStringAsync(string verb, string objName, string parameters = "")
        {
            return await client.GetStringAsync(string.Format(Constants.TPI_Relative_Uri_Format, verb, objName, parameters));
        }
    }
}
