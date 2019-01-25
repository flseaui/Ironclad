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
        private List<ActionSet> _actionSets;
        [SerializeField] private GameObject[] _stagePrefabs;

        private void Awake()
        {
            _actionSets = new List<ActionSet>();
        }

        /**
         * STAGES
         */

        public GameObject GetStage(Types.Stage stage)
        {
            return _stagePrefabs.FirstOrDefault(prefab => prefab.name == stage.ToString());
        }

        public GameObject GetStageByIndex(int index)
        {
            return _stagePrefabs[index];
        }

        /**
         * ACTIONS
         */

        public ActionInfo GetAction(Types.Character characterType, Types.ActionType actionType)
        {
            return GetActionSet(characterType).GetAction(actionType);
        }

        public ActionSet GetActionSet(Types.Character characterType)
        {
            return _actionSets.FirstOrDefault(set => set.Character == characterType);
        }

        public void PopulateActions(IEnumerable<Types.Character> characters)
        {
            foreach (var character in characters)
            {
                if (character == Types.Character.None)
                    continue;

                _actionSets.Add(new ActionSet(character, LoadActions(character)));
            }
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
                //Debug.Log($"READ FILE: {file}");

                var jsonData = File.ReadAllText(file);
                var action = JsonConvert.DeserializeObject<ActionInfo>(jsonData);

                actions.Add(action.Type, action);
            }

            return actions;
        }

        public class ActionSet
        {
            private readonly Dictionary<Types.ActionType, ActionInfo> _actionsDictionary;

            public ActionSet()
            {
                _actionsDictionary = new Dictionary<Types.ActionType, ActionInfo>();
            }

            public ActionSet(Types.Character character, Dictionary<Types.ActionType, ActionInfo> actionsDictionary)
            {
                Character = character;
                _actionsDictionary = actionsDictionary;
            }

            public Types.Character Character { get; }

            public IEnumerable<ActionInfo> Actions => _actionsDictionary.Values;

            public ActionInfo GetAction(Types.ActionType actionType)
            {
                return _actionsDictionary.ContainsKey(actionType)
                    ? _actionsDictionary[actionType]
                    : _actionsDictionary[Types.ActionType.Idle];
            }
        }
    }
}