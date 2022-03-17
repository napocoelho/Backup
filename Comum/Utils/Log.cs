using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using Comum.Models;

namespace Comum.Utils
{
    //Cria um um Delegate
    public delegate void LogEvent(object sender, StatusLog status, string message, DateTime time);

    [Serializable]
    public enum StatusLog
    {
        Event,
        Error,
        Warning,
        Debug
    }

    public class Log
    {
        public   void RemoveAllBindings()
        {
            EventLog = null;
            ErrorLog = null;
            WarningLog = null;
            DebugLog = null;
            CompleteLog = null;
        }

        //Cria eventos:
        public   event LogEvent EventLog = null;
        public   event LogEvent ErrorLog = null;
        public   event LogEvent WarningLog = null;
        public   event LogEvent DebugLog = null;
        public   event LogEvent CompleteLog = null;

        public Log()
        {
        }

        public   void Event(object sender, string message)
        {
            if (EventLog != null)
            {
                EventLog(sender, StatusLog.Event, message, DateTime.Now);
            }

            if (CompleteLog != null)
            {
                CompleteLog(sender, StatusLog.Event, message, DateTime.Now);
            }
        }

        public   void Error(object sender, string message)
        {
            if (ErrorLog != null)
            {
                ErrorLog(sender, StatusLog.Error, message, DateTime.Now);
            }

            if (CompleteLog != null)
            {
                CompleteLog(sender, StatusLog.Error, message, DateTime.Now);
            }
        }

        public   void Warning(object sender, string message)
        {
            if (WarningLog != null)
            {
                WarningLog(sender, StatusLog.Warning, message, DateTime.Now);
            }

            if (CompleteLog != null)
            {
                CompleteLog(sender, StatusLog.Warning, message, DateTime.Now);
            }
        }

        public   void Debug(object sender, string message)
        {
            if (WarningLog != null)
            {
                DebugLog(sender, StatusLog.Debug, message, DateTime.Now);
            }

            if (CompleteLog != null)
            {
                CompleteLog(sender, StatusLog.Debug, message, DateTime.Now);
            }
        }
    }
}