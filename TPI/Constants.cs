
namespace TPI
{
    public static class Constants
    {
        public const string IP_Regex = @"^[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}$";

        public const string IP_Msg_Invalid = "Invalid receiver address";
        public const string TPI_Msg_Object_Can_Not_Show = "This object can't be shown";
        public const string TPI_Msg_Not_Supported_Cmd = "This command not supported by this receiver";
        public const string TPI_Msg_ElevationMask_OutOfRange = "Elevation Mask is in degrees, between -10 and 90";
        public const string TPI_Msg_PdopMask_OutOfRange = "PDOP is a unitless measurement. Valid range is 0 to 99.";
        public const string TPI_Msg_Operation_Failed = "Operation Failed";

        // 0: protocol, 'http' or 'https'
        // 1: address, name or ip
        // 2: verb, show, set, reset, enable, disable, delete, download, upload
        // 3: an Object type on which the action is to be performed. Objects can be: A state or characteristic of the system, such as the Serial Number. A function control, such as ElevationMask or data logging Session. A file object, like a logged data file, or a firmware file.
        // 4: parameters, multi-pairs like &param=value&param=value
        public const string TPI_Format_Base_Uri = @"{0}://{1}";
        public const string TPI_Format_Relative_Uri = @"/prog/{0}?{1}{2}";
        public const string TPI_Format_Parameter = @"&{0}={1}";

        public const string TPI_Verb_Show = "show";
        public const string TPI_Verb_Set = "set";
        public const string TPI_Verb_Reset = "reset";
        public const string TPI_Verb_Enable = "enable";
        public const string TPI_Verb_Disable = "disable";
        public const string TPI_Verb_Delete = "delete";
        public const string TPI_Verb_Download = "download";
        public const string TPI_Verb_Upload = "upload";

        // verb, object
        public const string TPI_Regex_Parse_Commands = @"verb=(?<verb>(\S+))\s+object=(?<object>(\S+))";
        public const string TPI_Regex_Parse_Commands_Verb = "verb";
        public const string TPI_Regex_Parse_Commands_Object = "object";

        // sn, rxType
        public const string TPI_Regex_Parse_SerialNumber = @"sn=(?<sn>(\S+))\s+(rxType=""(?<rxType>(.+))"")?";
        public const string TPI_Regex_Parse_SerialNumber_SN = "sn";
        public const string TPI_Regex_Parse_SerialNumber_RxType = "rxType";

        // year, month, day, hour, minute, second, julianDay
        public const string TPI_Regex_Parse_UtcTime = @"year=(?<year>(\d+))\s+month=(?<month>(\d+))\s+day=(?<day>(\d+))\s+hour=(?<hour>(\d+))\s+minute=(?<minute>(\d+))\s+second=(?<second>(\d+))\s+julianDay=(?<julianDay>(\d+))";
        public const string TPI_Regex_Parse_UtcTime_Year = "year";
        public const string TPI_Regex_Parse_UtcTime_Month = "month";
        public const string TPI_Regex_Parse_UtcTime_Day = "day";
        public const string TPI_Regex_Parse_UtcTime_Hour = "hour";
        public const string TPI_Regex_Parse_UtcTime_Minute = "minute";
        public const string TPI_Regex_Parse_UtcTime_Second = "second";
        public const string TPI_Regex_Parse_UtcTime_JulianDay = "julianDay";

        public const string TPI_Regex_Parse_GpsTime = @"gpsweek=(?<gpsweek>(\d+))\s+weekseconds=(?<weekseconds>(\d+))";
        public const string TPI_Regex_Parse_GpsTime_GpsWeek = "gpsweek";
        public const string TPI_Regex_Parse_GpsTime_WeekSeconds = "weekseconds";

        // GpsWeek, WeekSeconds, Latitude, Longitude, ClockOffset, ClockDrift, VelNorth, VelEast, VelUp, PDOP, HDOP, VDOP, TDOP
        public const string TPI_Regex_Parse_Position = @"GpsWeek\s+(?<GpsWeek>(\d+))\s+WeekSeconds\s+(?<WeekSeconds>([\-\+]?[0-9]*(\.[0-9]+)?))\s+Latitude\s+(?<Latitude>([\-\+]?[0-9]*(\.[0-9]+)?)).+\s+Longitude\s+(?<Longitude>([\-\+]?[0-9]*(\.[0-9]+)?)).+\s+Altitude\s+(?<Altitude>([\-\+]?[0-9]*(\.[0-9]+)?)).+\s+Qualifiers\s+(?<Qualifiers>(\S+))\s+Satellites\s+(?<Satellites>(([0-9]*,?)*)).+\s+ClockOffset\s+(?<ClockOffset>([\-\+]?[0-9]*(\.[0-9]+)?)).*\s+ClockDrift\s+(?<ClockDrift>([\-\+]?[0-9]*(\.[0-9]+)?)).*\s+VelNorth\s+(?<VelNorth>([\-\+]?[0-9]*(\.[0-9]+)?)).*\s+VelEast\s+(?<VelEast>([\-\+]?[0-9]*(\.[0-9]+)?)).*\s+VelUp\s+(?<VelUp>([\-\+]?[0-9]*(\.[0-9]+)?)).*\s+PDOP\s+(?<PDOP>([0-9]*(\.[0-9]+)?)).*\s+HDOP\s+(?<HDOP>([0-9]*(\.[0-9]+)?)).*\s+VDOP\s+(?<VDOP>([0-9]*(\.[0-9]+)?)).*\s+TDOP\s+(?<TDOP>([0-9]*(\.[0-9]+)?))";
        public const string TPI_Regex_Parse_Position_GpsWeek = "GpsWeek";
        public const string TPI_Regex_Parse_Position_WeekSeconds = "WeekSeconds";
        public const string TPI_Regex_Parse_Position_Latitude = "Latitude";
        public const string TPI_Regex_Parse_Position_Longitude = "Longitude";
        public const string TPI_Regex_Parse_Position_Altitude = "Altitude";
        public const string TPI_Regex_Parse_Position_ClockOffset = "ClockOffset";
        public const string TPI_Regex_Parse_Position_ClockDrift = "ClockDrift";
        public const string TPI_Regex_Parse_Position_VelNorth = "VelNorth";
        public const string TPI_Regex_Parse_Position_VelEast = "VelEast";
        public const string TPI_Regex_Parse_Position_VelUp = "VelUp";
        public const string TPI_Regex_Parse_Position_PDOP = "PDOP";
        public const string TPI_Regex_Parse_Position_HDOP = "HDOP";
        public const string TPI_Regex_Parse_Position_VDOP = "VDOP";
        public const string TPI_Regex_Parse_Position_TDOP = "TDOP";
        public const string TPI_Regex_Parse_Position_Qualifiers = "Qualifiers";

        // port, volts, cap
        public const string TPI_Regex_Parse_Voltages = @"port=(?<port>(.*))\s+volts=(?<volts>([0-9]*(\.[0-9]*)?))\s+cap=(?<cap>(\d+))%";
        public const string TPI_Regex_Parse_Voltages_Port = "port";
        public const string TPI_Regex_Parse_Voltages_Volts = "volts";
        public const string TPI_Regex_Parse_Voltages_Capacity = "cap";

        // temp
        public const string TPI_Regex_Parse_Temperature = @"temp=(?<temp>([\-\+]?[0-9]*(\.[0-9]+)?))";
        public const string TPI_Regex_Parse_Temperature_Temp = "temp";

        // version, date
        public const string TPI_Regex_Parse_FirmwareVersion = @"version=(?<version>(\S+))\s+date=(?<date>(\S+))";
        public const string TPI_Regex_Parse_FirmwareVersion_Version = "version";
        public const string TPI_Regex_Parse_FirmwareVersion_Date = "date";

        // date
        public const string TPI_Regex_Parse_FirmwareWarranty = @"date=(?<date>(\S+))";
        public const string TPI_Regex_Parse_FirmwareWarranty_Date = "date";

        // temp
        public const string TPI_Regex_Parse_Mask = @"mask=(?<mask>(\d+))";
        public const string TPI_Regex_Parse_Mask_Mask = "mask";
    }
}
