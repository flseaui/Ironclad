using MISC;
using PLAYER;
using UnityEngine;
using Types = DATA.Types;

namespace MANAGERS
{
    public class MatchStateManager : Singleton<MatchStateManager>
    {
        // Prefabs
        public GameObject PlayerPrefab;

        public Transform[] SpawnPoints;

        private void Start()
        {
            MatchStart();
        }

        private void MatchStart()
        {
            for (var i = 0; i < GameManager.Instance.Characters.Length; ++i)
            {
                var player = Instantiate
                (
                    PlayerPrefab,
                    SpawnPoints[i].position,
                    SpawnPoints[i].rotation
                );

                AssetManager.Instance.PopulateActions(new[] {Types.Character.TestCharacter});

                player.GetComponent<PlayerInput>().Id = i;
                player.GetComponent<PlayerInput>().Id = i;
            }
        }
    }
}