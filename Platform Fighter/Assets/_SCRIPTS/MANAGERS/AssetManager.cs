using System.Collections.Generic;
using System.IO;
using System.Linq;
using DATA;
using MISC;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using TOOLS;
using UnityEngine;
using Types = DATA.Types;

namespace MANAGERS
{
    public class AssetManager : Singleton<AssetManager>
    {
        [SerializeField] private GameObject[] _stagePrefabs;
        
        private List<Dictionary<Types.ActionType, ActionInfo>> _actionSets;
        
        private void Awake() => _actionSets = new List<Dictionary<Types.ActionType, ActionInfo>>();
        
        /**
         * STAGES
         */

        public GameObject GetStage(Types.Stage stage) =>
            _stagePrefabs.FirstOrDefault(prefab => prefab.name == stage.ToString());

        public GameObject GetStageByIndex(int index) => _stagePrefabs[index];
        
        /**
         * ACTIONS
         */
        
        public ActionInfo GetAction(Types.Character characterType, Types.ActionType actionType) =>
            _actionSets[(int) characterType].ContainsKey(actionType)
                ? _actionSets[(int) characterType][actionType]
                : _actionSets[(int) characterType][Types.ActionType.Idle];

        public Dictionary<Types.ActionType, ActionInfo> GetActionSet(Types.Character characterType) => _actionSets[(int) characterType];
        
        public void PopulateActions(IEnumerable<Types.Character> characters)
        {
            foreach (var character in characters) _actionSets.Add(LoadActions(character));
        }

        public void LogAction(ActionInfo action) =>
            NLog.Log(NLog.LogType.Message, $"ACTION: {action.Name}");

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
                NLog.Log(NLog.LogType.Message, $"READ FILE: {file}");

                var jsonData = File.ReadAllText(file);
                var action = JsonConvert.DeserializeObject<ActionInfo>(jsonData);

                actions.Add(action.Type, action);
            }

            return actions;
        }
    }
}