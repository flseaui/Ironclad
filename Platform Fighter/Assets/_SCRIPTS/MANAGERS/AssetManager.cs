using System.Collections.Generic;
using System.IO;
using System.Linq;
using DATA;
using MISC;
using Newtonsoft.Json;
using UnityEngine;
using Types = DATA.Types;

namespace MANAGERS
{
    public class AssetManager : Singleton<AssetManager>
    {
        private List<Dictionary<Types.ActionType, ActionInfo>> _actionSets;

        private void Awake()
        {
            _actionSets = new List<Dictionary<Types.ActionType, ActionInfo>>();
        }

        public ActionInfo GetAction(Types.Character characterType, Types.ActionType actionType)
        {
            return _actionSets[(int) characterType].ContainsKey(actionType)
                ? _actionSets[(int) characterType][actionType]
                : _actionSets[(int) characterType][Types.ActionType.Idle];
        }

        public void PopulateActions(IEnumerable<Types.Character> characters)
        {
            foreach (var character in characters) _actionSets.Add(LoadActions(character));
        }

        public void LogAction(ActionInfo action)
        {
            Debug.Log($"ACTION: {action.Name}");
        }

        // reads in all of a characters actions and returns a list of them
        private Dictionary<Types.ActionType, ActionInfo> LoadActions(
            Types.Character character = Types.Character.TestCharacter)
        {
            var actions = new Dictionary<Types.ActionType, ActionInfo>();

            var actionPath = Path.Combine(Application.streamingAssetsPath, $"_ACTIONS/{character}/");

            if (!Directory.Exists(actionPath))
                throw new DirectoryNotFoundException($"INVALID CHARACTER DIRECTORY {actionPath}");

            foreach (var file in Directory.GetFiles(actionPath).Where(s => s.EndsWith(".json")))
            {
                Debug.Log($"READ FILE: {file}");

                var jsonData = File.ReadAllText(file);
                var action = JsonConvert.DeserializeObject<ActionInfo>(jsonData);

                actions.Add(action.Type, action);
            }

            return actions;
        }
    }
}