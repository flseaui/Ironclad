using UnityEditor;

namespace MISC
{
    public class LoggerMenuItems
    {
        [MenuItem("Log/Log Messages/Do")]
        private static void LogMessages() => LogToggle.LogMessages = true;

        [MenuItem("Log/Log Messages/Do", true)]
        private static bool LogMessagesValidation() => !LogToggle.LogMessages;

        [MenuItem("Log/Log Messages/Dont")]
        private static void DontLogMessages() => LogToggle.LogMessages = false;

        [MenuItem("Log/Log Messages/Dont", true)]
        private static bool DontLogMessagesValidation() => LogToggle.LogMessages;
        
        // tweet about text aliasing on switch
        // tweet about vlog straying from blog and difference from video essay/how it should be
    }
}