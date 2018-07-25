using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using MISC;
using UnityEngine;

namespace MENU
{
    public class PlayerProfilePanel : MonoBehaviour
    {
        [SerializeField]
        private GameObject _playerProfilePrefab;

        private int _playerCount;

        private List<ProfileInfo> _playerProfiles;

        private struct ProfileInfo
        {
            public GameObject Profile { get; }
            public ulong PlayerId { get; }

            public ProfileInfo(GameObject profile, ulong playerId)
            {
                Profile = profile;
                PlayerId = playerId;
            }
        }

        private void Awake()
        {
            _playerProfiles = new List<ProfileInfo>();
        }

        public void AddPlayerProfile(ulong playerId)
        {
            if (_playerCount == 4) return;
            
            ++_playerCount;
            var playerProfile = Instantiate(_playerProfilePrefab, transform);
            playerProfile.GetComponent<PlayerProfile>().SetBorderColor
            (
                _playerCount
                .Map(1, Color.red)
                .Map(2, Color.blue)
                .Map(3, Color.green)
                .Map(4, Color.yellow)
                .Else(Color.white)
            );

            playerProfile.GetComponent<PlayerProfile>().SetPlayerName($"Player {_playerCount}");
            _playerProfiles.Add(new ProfileInfo(playerProfile, playerId));
        }

        public void RemovePlayerProfile(ulong id)
        {
            --_playerCount;
            Destroy(_playerProfiles.FirstOrDefault(x => x.PlayerId == id).Profile);
        }
        
    }
}