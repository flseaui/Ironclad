using System.Collections.Generic;
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
			AssetManager.Instance.PopulateActions(new Types.Character[] { Types.Character.TEST_CHARACTER });
			AssetManager.Instance.GetAction(Types.Character.TEST_CHARACTER, Types.ActionType.JAB);
			
            for (int i = 0; i < playerCount; ++i)
            {
                Debug.Log("d");
                Instantiate(playerPrefab, spawnPoints[i].position, spawnPoints[i].rotation);
            }
        }

        private void Update()
        {

        }
    }
}