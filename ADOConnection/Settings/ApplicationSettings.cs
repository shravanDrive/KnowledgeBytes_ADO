using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADOConnection.Settings
{
    /// <summary>
    /// 
    /// </summary>
    public interface IApplicationSettings
    {
        /// <summary>
        /// 
        /// </summary>
        public string ClientId { get; }

        /// <summary>
        /// 
        /// </summary>
        public string ClientSecret { get; }

        /// <summary>
        /// 
        /// </summary>
        public ConnectionStrings ConnectionStrings { get; }

        /// <summary>
        /// 
        /// </summary>
        public int DefaultCommandTimeoutInSeconds { get; }

    }

    public class ApplicationSettings : IApplicationSettings
    {
        /// <summary>
        /// 
        /// </summary>
        public string ClientId => GetSettingValue("ClientId");

        /// <summary>
        /// 
        /// </summary>
        public string ClientSecret => GetSettingValue("ClientSecret");

        /// <summary>
        /// 
        /// </summary>
        public string APIUrl => GetSettingValue("APIUrl");

        /// <summary>
        /// 
        /// </summary>
        public ConnectionStrings ConnectionStrings => new ConnectionStrings
        {
            databasePath = ConfigurationManager.ConnectionStrings["DatabasePath"].ToString()
        };

        /// <summary>
        /// 
        /// </summary>
        public int DefaultCommandTimeoutInSeconds => int.Parse(GetSettingValue("DefaultCommandTimeoutInSeconds"));
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private static string GetSettingValue(string key)
        {
            var settingValue = ConfigurationManager.AppSettings[key];
            return settingValue ?? string.Empty;
        }
    }

    public class ConnectionStrings
    {
        /// <summary>
        /// databasePath
        /// </summary>
        public string databasePath { get; set; }
    }
}
