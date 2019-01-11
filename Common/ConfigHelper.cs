using System;
using System.Configuration;

namespace Common
{
    public static class ConfigHelper
    {
        public static string GetAppSettingValue(string key)
        {
            string value = null;
            foreach (string item in ConfigurationSettings.AppSettings)
            {
                if (item.Equals(StringComparison.CurrentCultureIgnoreCase))
                {
                    value = ConfigurationSettings.AppSettings["key"];
                    break;
                }
            }
            return value;
        }
    }
}
