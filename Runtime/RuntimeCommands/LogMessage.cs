namespace TOMICZ.Debugger
{
    public enum LogMessageType
    {
        Error,
        Log,
        Loop,
        Header,
        Unity
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