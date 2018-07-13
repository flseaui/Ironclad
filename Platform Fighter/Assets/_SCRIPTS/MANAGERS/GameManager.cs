using MISC;
using UnityEngine;

namespace MANAGERS
{
    public class GameManager : Singleton<GameManager>
    {
        public int playerCount = 1;

        // Prefabs
        public GameObject playerPrefab;

        public Transform[] spawnPoints;
    }
}