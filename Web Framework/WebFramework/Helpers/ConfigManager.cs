using log4net;
using Ninject;
using System;
using System.Configuration;
using WebFramework.App_Start;

namespace WebFramework.Helpers
{
    public class ConfigManager
    {
        private static readonly ILog _log = ModuleLoader.DI.Get<ILog>(Constants.ServerLoggerName);

        public static T Get<T>(string key, T defaultValue = default(T))
        {
            try
            {
                var value = ConfigurationManager.AppSettings[key];
                return (T)Convert.ChangeType(value, typeof(T));
            }
            catch (Exception ex)
            {
                _log.Error("ConfigManager", ex);
                return defaultValue;
            }
        }
    }
}