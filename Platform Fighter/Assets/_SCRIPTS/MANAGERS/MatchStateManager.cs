using System.Collections.Generic;
using System.Linq;
using MISC;
using NETWORKING;
using PLAYER;
using TOOLS;
using UnityEngine;
using UnityEngine.Serialization;
using Types = DATA.Types;

namespace MANAGERS
{
    public class MatchStateManager : Singleton<MatchStateManager>
    {
        // Prefabs
        [SerializeField]
        private GameObject _playerPrefab;

        private List<GameObject> _activePlayers;
        
        private void Start()
        {
            _activePlayers = new List<GameObject>();
            
            MatchStart();
        }

        public GameObject GetPlayer(int playerId) => _activePlayers.FirstOrDefault(player => player.GetComponent<NetworkIdentity>().Id == playerId);
        
        private void MatchStart()
        {
            var spawnPoints = SpawnStage();
            
            GameManager.Instance.Characters = new []
            {
                Types.Character.TestCharacter,
                Types.Character.None
            };
            
            for (var i = 0; i < GameManager.Instance.Characters.Count(character => character != Types.Character.None); ++i)
            {
                var player = Instantiate
                (
                    _playerPrefab,
                    spawnPoints[i].position,
                    spawnPoints[i].rotation
                );

                AssetManager.Instance.PopulateActions(GameManager.Instance.Characters);

                player.GetComponent<PlayerInput>().Id = i;
                player.GetComponent<NetworkIdentity>().Id = i;
                _activePlayers.Add(player);
            }
        }

        // spawns stage and returns list of that stages spawn points
        private Transform[] SpawnStage()
        {
            var stagePrefab = AssetManager.Instance.GetStage(GameManager.Instance.Stage);

            var stage = Instantiate(stagePrefab);

            var spawnPointsParent = stage.transform.Find("SpawnPoints");
            
            return spawnPointsParent
                .GetComponentsInChildren<Transform>(true)
                .Where(spawnPoint => spawnPoint.gameObject != spawnPointsParent.gameObject)
                .ToArray();
        }
        
    }
}