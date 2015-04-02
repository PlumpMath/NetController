using System;
using System.Collections.Generic;
using System.Diagnostics;
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

    public class Voltage
    {
        /// <summary>
        /// The number of voltage ports shown is receiver type dependent.
        /// </summary>
        public string Port { get; internal set; }

        /// <summary>
        /// In Voltage
        /// </summary>
        public double Volt { get; internal set; }

        /// <summary>
        /// In hundred percent %
        /// </summary>
        public double Capacity { get; internal set; }

        public override string ToString()
        {
            return string.Format("{0}, {1}v, {2}%", Port, Volt, Capacity);
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
                throw new ArgumentException(Constants.IP_Msg_Invalid);
            }
            Address = address;
            client = new HttpClient();
            if (!string.IsNullOrEmpty(user) && !string.IsNullOrEmpty(password))
            {
                client.BaseAddress = new Uri(string.Format(Constants.TPI_Format_Base_Uri, protocol, address));
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

        /// <summary>
        /// Returns the Serial Number for this GNSS Receiver.
        /// </summary>
        public string SN { get; private set; }

        public string RxType { get; private set; }

        /// <summary>
        /// Returns the current UTC date and time.
        /// </summary>
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

        /// <summary>
        /// Returns the current GPS weeknumber and seconds within week. The response is in the GPS time scale which is slightly offset from UTC time. GPS weeks start at Midnight between Saturday and Sunday.
        /// </summary>
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

        /// <summary>
        /// Returns a multi-line response showing information about the most recent position fix, including timetag, location, velocity, clock measurements, and DOPs.
        /// </summary>
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
                    else if (x.Status == TaskStatus.Faulted)
                    {
                        Debug.WriteLine(x.Exception);
                    }
                }).Wait();
                return position;
            }
        }

        /// <summary>
        /// Returns the currently measured voltage on each of the Power or battery inputs.
        /// </summary>
        public List<Voltage> Voltages
        {
            get
            {
                var voltages = new List<Voltage>();
                ExecuteCommand(TPIVerbTypes.Show, TPIObjectTypes.Voltages, pattern: Constants.TPI_Regex_Parse_Voltages).ContinueWith(x =>
                {
                    if (x.Status == TaskStatus.RanToCompletion)
                    {
                        foreach (Match match in x.Result)
                        {
                            voltages.Add(new Voltage()
                            {
                                Port = match.Groups[Constants.TPI_Regex_Parse_Voltages_Port].Value,
                                Volt = double.Parse(match.Groups[Constants.TPI_Regex_Parse_Voltages_Volts].Value),
                                Capacity = double.Parse(match.Groups[Constants.TPI_Regex_Parse_Voltages_Capacity].Value)
                            });
                        }
                    }
                }).Wait();
                return voltages;
            }
        }

        /// <summary>
        /// Returns a measurement of the temperature inside the GNSS Receiver. This is mainly for diagnostic purposes, and should not be used as an indication of external temperature.
        /// </summary>
        public double Temperature
        {
            get
            {
                double t = 0.0;
                ExecuteCommand(TPIVerbTypes.Show, TPIObjectTypes.Temperature, pattern: Constants.TPI_Regex_Parse_Temperature).ContinueWith(x =>
                {
                    if (x.Status == TaskStatus.RanToCompletion && x.Result.Count > 0)
                    {
                        t = double.Parse(x.Result[0].Groups[Constants.TPI_Regex_Parse_Temperature_Temp].Value);
                    }
                }).Wait();
                return t;
            }
        }

        public string FirmwareVersion { get; private set; }

        public DateTime FirmwareVersionDate { get; private set; }

        public DateTime FirmwareWarrantyDate { get; private set; }

        /// <summary>
        /// The Elevation Mask defines an elevation limit below which GNSS satellites will not be tracked or used.
        /// Mask is in degrees, between -10 and 90.
        /// </summary>
        public int ElevationMask
        {
            get
            {
                int m = 0;
                ExecuteCommand(TPIVerbTypes.Show, TPIObjectTypes.ElevationMask, pattern: Constants.TPI_Regex_Parse_Mask).ContinueWith(x =>
                {
                    if (x.Status == TaskStatus.RanToCompletion && x.Result.Count > 0)
                    {
                        m = int.Parse(x.Result[0].Groups[Constants.TPI_Regex_Parse_Mask_Mask].Value);
                    }
                }).Wait();
                return m;
            }
            set
            {
                if (value < -10 || value > 90)
                    throw new ArgumentOutOfRangeException("ElevationMask", Constants.TPI_Msg_ElevationMask_OutOfRange);
                bool failed = false;
                ExecuteCommand(TPIVerbTypes.Set, TPIObjectTypes.ElevationMask, "&" + Constants.TPI_Regex_Parse_Mask_Mask + "=" + value, pattern: Constants.TPI_Regex_Parse_Mask).ContinueWith(x =>
                {
                    if (x.Status != TaskStatus.RanToCompletion || x.Result == null)
                    {
                        failed = true;
                    }
                }).Wait();
                if (failed)
                    throw new Exception(Constants.TPI_Msg_Operation_Failed);
            }
        }

        /// <summary>
        /// The PDOP Mask defines a limit for the PDOP of a position fix. Position fixes with a PDOP that exceeds the mask will not be logged to data files, streamed to I/O ports, shown on various User Interfaces, etc.
        /// The desired PDOP Mask. PDOP is a unitless measurement. Valid range is 0 to 99.
        /// </summary>
        public int PdopMask
        {
            get
            {
                int m = 0;
                ExecuteCommand(TPIVerbTypes.Show, TPIObjectTypes.PdopMask, pattern: Constants.TPI_Regex_Parse_Mask).ContinueWith(x =>
                {
                    if (x.Status == TaskStatus.RanToCompletion && x.Result.Count > 0)
                    {
                        m = int.Parse(x.Result[0].Groups[Constants.TPI_Regex_Parse_Mask_Mask].Value);
                    }
                }).Wait();
                return m;
            }
            set
            {
                if (value < 0 || value > 99)
                    throw new ArgumentOutOfRangeException("PdopMask", Constants.TPI_Msg_PdopMask_OutOfRange);
                bool failed = false;
                ExecuteCommand(TPIVerbTypes.Set, TPIObjectTypes.PdopMask, "&" + Constants.TPI_Regex_Parse_Mask_Mask + "=" + value, pattern: Constants.TPI_Regex_Parse_Mask).ContinueWith(x =>
                {
                    if (x.Status != TaskStatus.RanToCompletion || x.Result == null)
                    {
                        failed = true;
                    }
                }).Wait();
                if (failed)
                    throw new Exception(Constants.TPI_Msg_Operation_Failed);
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

            await Task.Factory.ContinueWhenAll(new Task[] {

            // initialize sn & receiver model
             ExecuteCommand(TPIVerbTypes.Show, TPIObjectTypes.SerialNumber, pattern: Constants.TPI_Regex_Parse_SerialNumber).ContinueWith(x =>
            {
                if (x.Status == TaskStatus.RanToCompletion && x.Result.Count > 0)
                {
                    SN = x.Result[0].Groups[Constants.TPI_Regex_Parse_SerialNumber_SN].Value;
                    RxType = x.Result[0].Groups[Constants.TPI_Regex_Parse_SerialNumber_RxType].Value;
                }
            }),

            // initialize firmware information
             ExecuteCommand(TPIVerbTypes.Show, TPIObjectTypes.FirmwareVersion, pattern: Constants.TPI_Regex_Parse_FirmwareVersion).ContinueWith(x =>
            {
                if (x.Status == TaskStatus.RanToCompletion && x.Result.Count > 0)
                {
                    FirmwareVersion = x.Result[0].Groups[Constants.TPI_Regex_Parse_FirmwareVersion_Version].Value;
                    FirmwareVersionDate = DateTime.Parse(x.Result[0].Groups[Constants.TPI_Regex_Parse_FirmwareVersion_Date].Value);
                }
            }),

             ExecuteCommand(TPIVerbTypes.Show, TPIObjectTypes.FirmwareWarranty, pattern: Constants.TPI_Regex_Parse_FirmwareWarranty).ContinueWith(x =>
            {
                if (x.Status == TaskStatus.RanToCompletion && x.Result.Count > 0)
                {
                    FirmwareWarrantyDate = DateTime.Parse(x.Result[0].Groups[Constants.TPI_Regex_Parse_FirmwareWarranty_Date].Value);
                }
            })
            }, ts => { });
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
                throw new NotSupportedException(Constants.TPI_Msg_Not_Supported_Cmd);
            }
            var ret = await GetStringAsync(verb.ToString(), obj.ToString(), parameters);
            if (string.IsNullOrEmpty(ret) || ret.StartsWith("ERROR:"))
            {
                Debug.WriteLine(ret);
                return null;
            }
            else
            {
                return Regex.Matches(ret, pattern, RegexOptions.IgnoreCase);
            }
        }

        internal async Task<string> GetStringAsync(string verb, string objName, string parameters = "")
        {
            return await client.GetStringAsync(string.Format(Constants.TPI_Format_Relative_Uri, verb, objName, parameters));
        }

        #endregion
    }
}
