using System;

namespace TOMICZ.Debugger.Data
{
    public enum LogMessageType
    {
        Log,
        Warrning,
        Error,
    }

    public struct LogMessage
    {
        public LogMessageType type => _type;
        public object log => _log;

        private LogMessageType _type;
        private object _log;

        public LogMessage(LogMessageType type, object log)
        {
            _type = type;
            _log = log;
        }
    }
}