using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using static Types;

public class AssetManager : Singleton<AssetManager>
{
	private List<Dictionary<ActionType, ActionInfo>> actionSets;

    private void Start ()
	{
		actionSets = new List<Dictionary<ActionType, ActionInfo>>();
	}

	public ActionInfo GetAction(Character characterType, ActionType actionType)
	{
		return actionSets[(int) characterType][actionType];
	}

	public void PopulateActions(Character[] characters)
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
	private Dictionary<ActionType, ActionInfo> LoadActions(Character character = Character.TEST_CHARACTER)
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