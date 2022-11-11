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

    public class LogMessage
    {
        public LogMessageType type => _type;
        public string message => _message;

        private LogMessageType _type;
        private string _message;

        public LogMessage(LogMessageType type, string message)
        {
            _type = type;
            _message = message;
        }
    }
}