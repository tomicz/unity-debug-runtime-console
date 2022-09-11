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

    public static class LogMessage
    {
        public static string GetType(LogMessageType logType)
        {
            switch (logType)
            {
                case LogMessageType.Error:
                    return "[Error]";
                case LogMessageType.Log:
                    return "[Log]";
                case LogMessageType.Loop:
                    return "[Loop0]";
                case LogMessageType.Header:
                    return "";
                case LogMessageType.Unity:
                    return "<[Unity]";
            }

            return "";
        }
    }
}