using System.Web.Configuration;

namespace ClosestAddress
{
    public static class Constants
    {
        //Get google API from Webconfig file
        public static string GoogleAPIKey = WebConfigurationManager.AppSettings["GoogleAPIKey"];
        //Get CSV file path from Webconfig file
        public static string FileLocation = WebConfigurationManager.AppSettings["FileLocation"];
        //Get Google API Url from Webconfig file
        public static string GoogleAPIUrl = WebConfigurationManager.AppSettings["GoogleAPIUrl"];
        //Get Cache Time duration from Webconfig file
        public static string CacheTimeDuration = WebConfigurationManager.AppSettings["CacheTimeDuration"];
        //Get number of address in result from Webconfig file
        public static string NumberOfAddress = WebConfigurationManager.AppSettings["NumberOfAddress"];
    }
}
