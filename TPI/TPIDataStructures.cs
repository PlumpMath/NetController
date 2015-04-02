using System.Collections.Generic;

namespace TPI
{
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

    public class AntennaType
    {
        /// <summary>
        /// Type is a unique, Trimble-specific, numeric identifier.
        /// </summary>
        public int Type { get; internal set; }
        /// <summary>
        /// Name is a unique, Trimble-specific, descriptive string.
        /// </summary>
        public string Name { get; internal set; }
        /// <summary>
        /// A list of measurement method identifiers which are valid for the given antenna type.
        /// </summary>
        public IReadOnlyList<string> MeasureMethods { get; internal set; }

        internal AntennaType()
        {

        }

        public bool IsSupport(string method)
        {
            var sup = false;
            foreach (var m in MeasureMethods)
            {
                if (m == method)
                {
                    sup = true;
                    break;
                }
            }
            return sup;
        }

        public override string ToString()
        {
            return string.Format("{0}, {1}, {2}", Type, Name, string.Join(",", MeasureMethods));
        }
    }

    public class AntennaTypeCollection : IEnumerable<AntennaType>
    {
        List<AntennaType> types;

        internal AntennaTypeCollection(List<AntennaType> types)
        {
            this.types = types;
        }

        public AntennaType this[int type]
        {
            get
            {
                return types.Find(x => x.Type == type);
            }
        }

        public AntennaType this[string name]
        {
            get
            {
                return types.Find(x => x.Name == name);
            }
        }

        #region IEnumerable<AntennaType> Members

        public IEnumerator<AntennaType> GetEnumerator()
        {
            return types.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion
    }

    public class AntennaSetting
    {
        //public AntennaSetting(int type, string mearMethod, double height)
        //{
        //    //if (!type.IsSupport(mearMethod))
        //    //    throw new ArgumentOutOfRangeException(Constants.TPI_Msg_Antenna_MeasMethod_NotSupport);
        //}

        public int Type { get; set; }
        public string Name { get; set; }
        public string MeasureMethod { get; set; }
        public double Height { get; set; }
        public string SN { get; set; }

        public override string ToString()
        {
            return string.Format("{0},{1},{2},{3},{4}", Type, Name, MeasureMethod, Height, SN);
        }
    }

    public class Satellite
    {
        /// <summary>
        /// The Prn-Code for the tracked satellite.
        /// </summary>
        public int Prn { get; internal set; }
        /// <summary>
        /// The system type for the tracked satellite - GPS, GLN (Glonass), GAL (Galileo), SBS (Sbas/Waas/Egnos), etc.
        /// </summary>
        public string System { get; internal set; }
        /// <summary>
        /// The satellite's elevation in degrees above horizontal.
        /// </summary>
        public int Elevation { get; internal set; }
        /// <summary>
        /// The satellite's azimuth in degrees relative to True North.
        /// </summary>
        public int Azimuth { get; internal set; }

        public override string ToString()
        {
            return string.Format("{0},{1},{2},{3}", Prn, System, Elevation, Azimuth);
        }
    }
}
