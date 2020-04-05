namespace MP.Logging
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public interface ILogChannel
    {
        string Name { get; }
        string Process(string message, LogSeverity severity);
    }
}
