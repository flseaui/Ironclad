using System;
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
        
        public void AddPlayer()
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
        }
    }
}