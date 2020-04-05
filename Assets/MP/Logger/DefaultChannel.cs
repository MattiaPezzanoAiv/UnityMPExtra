namespace MP.Logging
{
    public class DefaultChannel : ILogChannel
    {
        public virtual string Name => "Default";

        public virtual string Process(string message, LogSeverity severity)
        {
            string finalMessage;

            switch(severity)
            {
                case LogSeverity.Info:
                    {
                        finalMessage = $"<color=white>{Name}: {message}<color/>";
                        break;
                    }
                case LogSeverity.Warning:
                    {
                        finalMessage = $"<color=yellow>{Name}: {message}<color/>";
                        break;
                    }
                case LogSeverity.Error:
                    {
                        finalMessage = $"<color=red>{Name}: {message}<color/>";
                        break;
                    }
                default:
                    {
                        finalMessage = message;
                        break;
                    }
            }

            return finalMessage;
        }
    }
}