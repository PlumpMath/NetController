﻿
namespace TPI
{
    public static class TPIArbitraryChars
    {
        public const string Space = "%20";
        public const string Percent = "%25";
        public const string Ampersand = "%26";
        public const string QuestionMark = "%3F";

        public static string GetEncodedString(string orignal)
        {
            return orignal.Replace(" ", Space).Replace("%", Percent).Replace("&", Ampersand).Replace("?", QuestionMark);
        }
    }

    class TPIParameters
    {
        public string MakeOneParameter(string name, object value)
        {
            return string.Format(Constants.TPI_Format_Parameter, Constants.TPI_Regex_Parse_Mask_Mask, TPIArbitraryChars.GetEncodedString(value.ToString()));
        }
    }
}
