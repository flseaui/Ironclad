using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace PlatformFighter
{
    public class GameManager : Singleton<GameManager>
    {
        public int playerCount = 1;

        // Prefabs
        [SerializeField]
        private GameObject playerPrefab;

        [SerializeField]
        private Transform[] spawnPoints;

        private void Start()
        {
            for (int i = 0; i < playerCount; ++i)
            {
                Instantiate(playerPrefab, spawnPoints[i].position, spawnPoints[i].rotation);
            }
        }

        private void Update()
        {

        }

        
		private void LogAction(ActionInfo action)
		{
			Debug.Log($"ACTION: { action.name }");
		}

		// reads in all of a characters actions and returns a list of them
		private List<ActionInfo> LoadActions(string character = "TEST_CHARACTER")
		{
			var actions = new List<ActionInfo>();

			var actionPath = Path.Combine(Application.streamingAssetsPath, $"_ACTIONS/{ character }/");

			if (!Directory.Exists(actionPath))
				throw new DirectoryNotFoundException($"INVALID CHARACTER DIRECTORY { actionPath }");

			foreach (var file in Directory.GetFiles(actionPath).Where(s => s.EndsWith(".json")))
			{
				Debug.Log($"READ FILE: { file }");

				var jsonData = File.ReadAllText(file);
				var action = JsonUtility.FromJson<ActionInfo>(jsonData);

				actions.Add(action);
			}

			return actions;
		}
    }
}