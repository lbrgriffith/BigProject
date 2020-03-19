using NLog;
using System;

namespace BigProject.Logging
{
    public static class Log
    {
        // get a Logger object and log exception here using NLog. 
        // this will use the "databaseLogger" logger from our NLog.config file
        private static Logger LogAs = LogManager.GetLogger("databaseLogger");

        public static void Error(Exception e, string methodName, string message)
        {
            // Validate required fields.
            if (e != null && !string.IsNullOrWhiteSpace(methodName))
            {
                LogAs.WithProperty("MethodName", methodName).Error(e, message);
            }
        }
    }
}
