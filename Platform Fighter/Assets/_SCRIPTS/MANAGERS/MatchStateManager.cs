using System.Collections.Generic;
using System.Linq;
using MISC;
using PLAYER;
using TOOLS;
using UnityEngine;
using Types = DATA.Types;

namespace MANAGERS
{
    public class MatchStateManager : Singleton<MatchStateManager>
    {
        // Prefabs
        public GameObject PlayerPrefab;

        private void Start()
        {
            MatchStart();
        }

        private void MatchStart()
        {

            var spawnPoints = SpawnStage();
            
            for (var i = 0; i < GameManager.Instance.Characters.Length; ++i)
            {
                var player = Instantiate
                (
                    PlayerPrefab,
                    spawnPoints[i].position,
                    spawnPoints[i].rotation
                );

                AssetManager.Instance.PopulateActions(new[] {Types.Character.TestCharacter});

                player.GetComponent<PlayerInput>().Id = i;
                player.GetComponent<PlayerInput>().Id = i;
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