using WebFramework.App_Start;
using log4net;
using Ninject;
using System;
using System.Web.Http;
using WebFramework.Helpers;

namespace WebFramework.Controllers.Api
{
    public class LogController : ApiController
    {
        private readonly ILog _log;

        private bool _logClientErrors;

        public LogController([Named(Constants.ClientLoggerName)] ILog log)
        {
            _log = log;
            _logClientErrors = ConfigManager.Get<bool>(Constants.ConfigKey.LogClientErrors);
        }

        [HttpPost]
        public void Error(dynamic data)
        {
            if (!_logClientErrors)
            {
                return;
            }

            _log.Error(data.ToString());
        }

        [HttpPost]
        public void Info(dynamic data)
        {
            if (!_logClientErrors)
            {
                return;
            }

            _log.Info(data.ToString());
        }

        [Route("api/Log/Message/{severity}/{message}")]
        [HttpPost]
        public void Message(string severity, string message)
        {
            if (!_logClientErrors)
            {
                return;
            }

            if (string.IsNullOrWhiteSpace(severity))
            {
                severity = "info";
            }

            switch (severity.ToLower())
            {
                case "fatal":
                    _log.Fatal(message);
                    break;
                case "error":
                    _log.Error(message);
                    break;
                case "warn":
                    _log.Warn(message);
                    break;
                default:
                    _log.Info(message);
                    break;
            }
        }

        [HttpGet]
        public string TestApi(int id)
        {
            return "Success" + id.ToString();
        }

        [HttpGet]
        public string TestApi(Guid guid)
        {
            return "Success" + guid.ToString();
        }

        [HttpGet]
        public string TestApi()
        {
            throw new NotImplementedException("This message is from the exception. This exception has an inner exception.",
                new Exception("This message is from the inner exception. This inner exception has a further inner exception.",
                    new Exception("This message is from the second-level inner exception. This inner exception has no further inner exceptions.")));
        }
    }
}
