using System;
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

    public class GpsTime
    {
        /// <summary>
        /// Gpsweek is the number of weeks since Sunday 1980/Jan/06, the start of GPS week zero.
        /// </summary>
        public int GpsWeek { get; private set; }
        /// <summary>
        /// WeekSeconds is the number of seconds elapsed since the beginning of the current week. Range is from 0 to 604799.
        /// </summary>
        public int WeekSeconds { get; private set; }

        public GpsTime(int week, int weekSeconds)
        {
            GpsWeek = week;
            WeekSeconds = weekSeconds;
        }

        public override string ToString()
        {
            return GpsWeek + "," + WeekSeconds;
        }
    }

    public class Position
    {
        /// <summary>
        /// The time tag when this position was determined.
        /// </summary>
        public int GpsWeek { get; internal set; }
        /// <summary>
        /// The time tag when this position was determined.
        /// </summary>
        public double WeekSeconds { get; internal set; }
        /// <summary>
        /// Decimal Degrees
        /// </summary>
        public double Latitude { get; internal set; }
        /// <summary>
        /// Decimal Degrees
        /// </summary>
        public double Longitude { get; internal set; }
        /// <summary>
        /// Altitude is in meters above the WGS84 Ellipsoid
        /// </summary>
        public double Altitude { get; internal set; }
        /// <summary>
        /// In milliseconds
        /// </summary>
        public double ClockOffset { get; internal set; }
        /// <summary>
        /// In parts per million
        /// </summary>
        public double ClockDrift { get; internal set; }
        /// <summary>
        /// In meters per second
        /// </summary>
        public double VelNorth { get; internal set; }
        /// <summary>
        /// In meters per second
        /// </summary>
        public double VelEast { get; internal set; }
        /// <summary>
        /// In meters per second
        /// </summary>
        public double VelUp { get; internal set; }
        /// <summary>
        /// DOP values are unitless
        /// </summary>
        public double PDOP { get; internal set; }
        /// <summary>
        /// DOP values are unitless
        /// </summary>
        public double HDOP { get; internal set; }
        /// <summary>
        /// DOP values are unitless
        /// </summary>
        public double VDOP { get; internal set; }
        /// <summary>
        /// DOP values are unitless
        /// </summary>
        public double TDOP { get; internal set; }
        /// <summary>
        /// Qualifiers can contain several strings, separated by commas.
        /// </summary>
        /// <remarks>
        /// 'WGS84' specifies the coordinate system.
        /// '3D' specifies the type of fix done. It could be 2D if there are insufficient satellites for a 3D fix.
        /// 'Autonomous' specifies that no differential corrections were made. Otherwise, differential corrections were used with possible types DGPS, BCN-DGPS, SBAS-DGPS, RTK-Fix, RTK-Float, etc.
        /// </remarks>
        public string Qualifiers { get; internal set; }

        public override string ToString()
        {
            return string.Format("Qualifiers: {14}\nLatitude:{2}\nLongitude:{3}\nAltitude:{4}\nClockOffset:{5}\nGpsWeek:{0}\nWeekSeconds:{1}\nClockDrift:{6}\nVelNorth:{7}\nVelEast:{8}\nVelUp:{9}\nPDOP:{10}\nHDOP:{11}\nVDOP:{12}\nTDOP:{13}", GpsWeek, WeekSeconds, Latitude, Longitude, Altitude, ClockOffset, ClockDrift, VelNorth, VelEast, VelUp, PDOP, HDOP, VDOP, TDOP, Qualifiers);
        }
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

            Initialize().Wait();
        }

        #region Properities

        public TPIProtocols Protocol { get; private set; }

        public string Address { get; private set; }

        public string SN { get; private set; }

        public string RxType { get; private set; }

        public DateTime UtcTime
        {
            get
            {
                DateTime dt = DateTime.UtcNow;
                ExecuteCommand(TPIVerbTypes.Show, TPIObjectTypes.UtcTime, pattern: Constants.TPI_Regex_Parse_UtcTime).ContinueWith(x =>
                {
                    if (x.Status == TaskStatus.RanToCompletion && x.Result.Count > 0)
                    {
                        dt = new DateTime(int.Parse(x.Result[0].Groups[Constants.TPI_Regex_Parse_UtcTime_Year].Value),
                        int.Parse(x.Result[0].Groups[Constants.TPI_Regex_Parse_UtcTime_Month].Value),
                        int.Parse(x.Result[0].Groups[Constants.TPI_Regex_Parse_UtcTime_Day].Value),
                        int.Parse(x.Result[0].Groups[Constants.TPI_Regex_Parse_UtcTime_Hour].Value),
                        int.Parse(x.Result[0].Groups[Constants.TPI_Regex_Parse_UtcTime_Minute].Value),
                        int.Parse(x.Result[0].Groups[Constants.TPI_Regex_Parse_UtcTime_Second].Value),
                         DateTimeKind.Utc);
                    }
                }).Wait();
                return dt;
            }
        }

        public GpsTime GpsTime
        {
            get
            {
                GpsTime time = null;
                ExecuteCommand(TPIVerbTypes.Show, TPIObjectTypes.GpsTime, pattern: Constants.TPI_Regex_Parse_GpsTime).ContinueWith(x =>
                {
                    if (x.Status == TaskStatus.RanToCompletion && x.Result.Count > 0)
                    {
                        time = new GpsTime(int.Parse(x.Result[0].Groups[Constants.TPI_Regex_Parse_GpsTime_GpsWeek].Value),
                        int.Parse(x.Result[0].Groups[Constants.TPI_Regex_Parse_GpsTime_WeekSeconds].Value));
                    }
                }).Wait();
                return time;
            }
        }

        public Position Position
        {
            get
            {
                Position position = null;
                ExecuteCommand(TPIVerbTypes.Show, TPIObjectTypes.Position, pattern: Constants.TPI_Regex_Parse_Position).ContinueWith(x =>
                {
                    if (x.Status == TaskStatus.RanToCompletion && x.Result.Count > 0)
                    {
                        position = new TPI.Position()
                        {
                            GpsWeek = int.Parse(x.Result[0].Groups[Constants.TPI_Regex_Parse_Position_GpsWeek].Value),
                            WeekSeconds = double.Parse(x.Result[0].Groups[Constants.TPI_Regex_Parse_Position_WeekSeconds].Value),
                            Latitude = double.Parse(x.Result[0].Groups[Constants.TPI_Regex_Parse_Position_Latitude].Value),
                            Longitude = double.Parse(x.Result[0].Groups[Constants.TPI_Regex_Parse_Position_Longitude].Value),
                            Altitude = double.Parse(x.Result[0].Groups[Constants.TPI_Regex_Parse_Position_Altitude].Value),
                            ClockOffset = double.Parse(x.Result[0].Groups[Constants.TPI_Regex_Parse_Position_ClockOffset].Value),
                            ClockDrift = double.Parse(x.Result[0].Groups[Constants.TPI_Regex_Parse_Position_ClockDrift].Value),
                            VelNorth = double.Parse(x.Result[0].Groups[Constants.TPI_Regex_Parse_Position_VelNorth].Value),
                            VelEast = double.Parse(x.Result[0].Groups[Constants.TPI_Regex_Parse_Position_VelEast].Value),
                            VelUp = double.Parse(x.Result[0].Groups[Constants.TPI_Regex_Parse_Position_VelUp].Value),
                            PDOP = double.Parse(x.Result[0].Groups[Constants.TPI_Regex_Parse_Position_PDOP].Value),
                            HDOP = double.Parse(x.Result[0].Groups[Constants.TPI_Regex_Parse_Position_HDOP].Value),
                            VDOP = double.Parse(x.Result[0].Groups[Constants.TPI_Regex_Parse_Position_VDOP].Value),
                            TDOP = double.Parse(x.Result[0].Groups[Constants.TPI_Regex_Parse_Position_TDOP].Value),
                            Qualifiers = x.Result[0].Groups[Constants.TPI_Regex_Parse_Position_Qualifiers].Value
                        };
                    }
                }).Wait();
                return position;
            }
        }
        #endregion

        #region Private, Protected, Interanl methods

        private async Task Initialize()
        {
            // initialize supported commands
            await ExecuteCommand(TPIVerbTypes.Show, TPIObjectTypes.Commands, pattern: Constants.TPI_Regex_Parse_Commands).ContinueWith(x =>
             {
                 if (x.Status == TaskStatus.RanToCompletion)
                 {
                     foreach (Match match in x.Result)
                     {
                         // If groupname is not the name of a capturing group in the collection, or if groupname is the name of a capturing group that has not been matched in the input string, the method returns a Group object whose Group.Success property is false and whose Group.Value property is String.Empty.
                         supportedCmds.Add(match.Groups[Constants.TPI_Regex_Parse_Commands_Verb].Value + ":" + match.Groups[Constants.TPI_Regex_Parse_Commands_Object].Value);
                     }
                 }
             });

            // initialize sn & receiver model
            await ExecuteCommand(TPIVerbTypes.Show, TPIObjectTypes.SerialNumber, pattern: Constants.TPI_Regex_Parse_SerialNumber).ContinueWith(x =>
            {
                if (x.Status == TaskStatus.RanToCompletion && x.Result.Count > 0)
                {
                    SN = x.Result[0].Groups[Constants.TPI_Regex_Parse_SerialNumber_SN].Value;
                    RxType = x.Result[0].Groups[Constants.TPI_Regex_Parse_SerialNumber_RxType].Value;
                }
            });
        }

        protected override void FreeManagedResources()
        {
            client.Dispose();
            base.FreeManagedResources();
        }

        private T Create<T>() where T : TPIObject
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

        #endregion
    }
}
