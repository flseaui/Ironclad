using System.Collections.Generic;
using System.Linq;
using MISC;
using NETWORKING;
using PLAYER;
using UnityEngine;
using Client = Facepunch.Steamworks.Client;
using Types = DATA.Types;

namespace MANAGERS
{
    public class MatchStateManager : Singleton<MatchStateManager>
    {
        private List<GameObject> _activePlayers;

        // Prefabs
        [SerializeField] private GameObject _playerPrefab;
        [SerializeField] private GameObject _networkPlayerPrefab;

        private void Start()
        {
            _activePlayers = new List<GameObject>();

            MatchStart();
        }

        public GameObject GetPlayer(int playerId) =>
            _activePlayers.FirstOrDefault(player => player.GetComponent<NetworkIdentity>().Id == playerId);

        private void MatchStart()
        {
            var spawnPoints = SpawnStage();

            for (var i = 0;
                i < GameManager.Instance.Characters.Count(character => character != Types.Character.None);
                ++i)
            {
                GameObject player;
                if (i == int.Parse(Client.Instance.Lobby.GetMemberData(Client.Instance.SteamId, "lobbySpot")))
                {
                    player = Instantiate
                    (
                        _playerPrefab,
                        spawnPoints[i].position,
                        spawnPoints[i].rotation
                    );
                    player.GetComponent<PlayerInput>().Id = 0;
                }
                else
                {
                    player = Instantiate
                    (
                        _networkPlayerPrefab,
                        spawnPoints[i].position,
                        spawnPoints[i].rotation
                    );
                }

                player.GetComponent<NetworkIdentity>().Id = i;
                
                AssetManager.Instance.PopulateActions(GameManager.Instance.Characters);

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