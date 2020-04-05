namespace MP.Logging
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

#if UNITY_EDITOR
    using System.Reflection;
    using System.Linq;
#endif

    public enum LogSeverity
    {
        Info, Warning, Error
    }

    public static class Log
    {
#if UNITY_EDITOR
        public static System.Type[] GetChannelTypes()
        {
            var t = typeof(ILogChannel);
            return Assembly.GetAssembly(typeof(ILogChannel))
                .GetTypes()
                .Where((type) => type.GetInterfaces().Contains(t))
                .ToArray();
        }
#endif
        public static void Info(ILogChannel channel, string message)
        {
            var m = channel.Process(message, LogSeverity.Info);
            Debug.Log(m);
        }

        public static void Warning(ILogChannel channel, string message)
        {
            var m = channel.Process(message, LogSeverity.Warning);
            Debug.LogWarning(m);
        }

        public static void Error(ILogChannel channel, string message)
        {
            var m = channel.Process(message, LogSeverity.Error);
            Debug.LogError(m);
        }
    }
}
