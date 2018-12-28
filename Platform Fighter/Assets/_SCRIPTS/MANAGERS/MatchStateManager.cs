using System;
using System.Collections.Generic;
using System.Linq;
using MISC;
using NETWORKING;
using PLAYER;
using UnityEngine;
using UnityEngine.Serialization;
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
        [SerializeField] private GameObject _p2pHandlerPrefab;
        [SerializeField] private GameObject _rollbackHandlerPrefab;
        
        private void OnParticleCollision(GameObject other)
        {
            throw new System.NotImplementedException();
        }

        private void Start()
        {
            _activePlayers = new List<GameObject>();
            
            MatchStart();
        }

        public int ClientPlayerId { get; private set; }
        
        public List<GameObject> Players => _activePlayers;
        
        public GameObject GetPlayer(int playerId) =>
            _activePlayers.FirstOrDefault(player => player.GetComponent<NetworkIdentity>().Id == playerId);

        public GameObject GetPlayerBySteamId(ulong steamId) =>
            _activePlayers.FirstOrDefault(player => player.GetComponent<NetworkIdentity>().SteamId == steamId);
        
        private void MatchStart()
        {
            var spawnPoints = SpawnStage();

            if (GameManager.Instance.MatchType == Types.MatchType.OnlineMultiplayer)
            {
                Instantiate(_p2pHandlerPrefab);
                Instantiate(_rollbackHandlerPrefab);
            }
            
            for (var i = 0;
                i < GameManager.Instance.Characters.Count(character => character != Types.Character.None);
                ++i)
            {
                GameObject player;
                switch (GameManager.Instance.MatchType)
                {
                    case Types.MatchType.OfflineSingleplayer:
                        player = Instantiate
                        (
                            _playerPrefab,
                            spawnPoints[i].position,
                            spawnPoints[i].rotation
                        );
                        player.GetComponent<PlayerInput>().Id = i;
                        break;
                    case Types.MatchType.OnlineMultiplayer:
                        // if is this clients controlled player
                        if (i == int.Parse(Client.Instance.Lobby.GetMemberData(Client.Instance.SteamId, "lobbySpot")))
                        {
                            player = Instantiate
                            (
                                _playerPrefab,
                                spawnPoints[i].position,
                                spawnPoints[i].rotation
                            );
                            player.GetComponent<PlayerInput>().Id = 0;
                            ClientPlayerId = i;
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
    
                        player.AddComponent<NetworkIdentity>();
                        player.GetComponent<NetworkIdentity>().Id = i;
                        player.GetComponent<NetworkIdentity>().SteamId = GameManager.Instance.SteamIds[i];
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                
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