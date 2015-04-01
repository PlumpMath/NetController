
namespace TPI
{
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
}
