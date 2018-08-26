using System;
using MISC;
using UnityEngine;
using Object = UnityEngine.Object;

namespace TOOLS
{
    public static class NLog
    {
        public enum LogType
        {
            Message
        }

        public static void Log(LogType logType, object message)
        {
            switch (logType)
            {
                case LogType.Message:
                    if (LogToggle.LogMessages)
                        Debug.Log(message);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(logType), logType, null);
            }
        }
    }
}