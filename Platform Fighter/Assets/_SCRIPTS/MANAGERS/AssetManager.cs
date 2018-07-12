using System.Collections.Generic;
using System.IO;
using System.Linq;
using DATA;
using MISC;
using UnityEngine;
using static DATA.Types;

namespace MANAGERS
{
    public static class AssetManager
    {
        private static readonly List<Dictionary<ActionType, ActionInfo>> ActionSets = new List<Dictionary<ActionType, ActionInfo>>();

        public static ActionInfo GetAction(Character characterType, ActionType actionType)
        {
            return ActionSets[(int) characterType][actionType];
        }

        public static void PopulateActions(Character[] characters)
        {
            foreach (var character in characters)
            {
                ActionSets.Add(LoadActions(character));
            }
        }

        public static void LogAction(ActionInfo action)
        {
            Debug.Log($"ACTION: { action.name }");
        }

        // reads in all of a characters actions and returns a list of them
        private static Dictionary<ActionType, ActionInfo> LoadActions(Character character = Character.TEST_CHARACTER)
        {
            var actions = new Dictionary<ActionType, ActionInfo>();

            var actionPath = Path.Combine(Application.streamingAssetsPath, $"_ACTIONS/{ character }/");

            if (!Directory.Exists(actionPath))
                throw new DirectoryNotFoundException($"INVALID CHARACTER DIRECTORY { actionPath }");

            foreach (var file in Directory.GetFiles(actionPath).Where(s => s.EndsWith(".json")))
            {
                Debug.Log($"READ FILE: { file }");

                var jsonData = File.ReadAllText(file);
                var action = JsonUtility.FromJson<ActionInfo>(jsonData);

                actions.Add(action.type, action);
            }

            return actions;
        }
    }
}