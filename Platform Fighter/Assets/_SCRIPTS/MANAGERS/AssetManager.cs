using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace PlatformFighter
{
	public class AssetManager : Singleton<AssetManager>
	{
		private List<Dictionary<Types.ActionType, ActionInfo>> actionSets;

		void Start ()
		{
			actionSets = new List<Dictionary<Types.ActionType, ActionInfo>>();
		}

		public ActionInfo GetAction(Types.Character characterType, Types.ActionType actionType)
		{
			return actionSets[(int) characterType][actionType];
		}

		public void PopulateActions(Types.Character[] characters)
		{
			foreach (var character in characters)
			{
				actionSets.Add(LoadActions(character));
			}
		}

		public void LogAction(ActionInfo action)
		{
			Debug.Log($"ACTION: { action.name }");
		}

		// reads in all of a characters actions and returns a list of them
		private Dictionary<Types.ActionType, ActionInfo> LoadActions(Types.Character character = Types.Character.TEST_CHARACTER)
		{
			var actions = new Dictionary<Types.ActionType, ActionInfo>();

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