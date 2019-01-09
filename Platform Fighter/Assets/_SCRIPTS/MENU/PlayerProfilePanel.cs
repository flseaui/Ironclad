using System.Collections.Generic;
using System.Linq;
using Facepunch.Steamworks;
using MISC;
using UnityEngine;
using Color = UnityEngine.Color;
using Image = UnityEngine.UI.Image;

namespace MENU
{
    public class PlayerProfilePanel : MonoBehaviour
    {
        private int _playerCount;

        [SerializeField] private GameObject _playerProfilePrefab;

        private List<ProfileInfo> _playerProfiles;

        private void Awake()
        {
            _playerProfiles = new List<ProfileInfo>();
        }

        public void AddPlayerProfile(ulong playerId)
        {
            if (_playerCount == 4) return;

            ++_playerCount;
            var playerProfile = Instantiate(_playerProfilePrefab, transform);

            var steamAvatar = playerProfile.transform.Find("PlayerAvatar").GetComponent<SteamAvatar>();
            steamAvatar.SteamId = playerId;
            steamAvatar.Fetch();

            var d = new Vector2();

            playerProfile.GetComponent<PlayerProfile>().SetBorderColor
            (
                _playerCount
                    .Map(1, Color.red)
                    .Map(2, Color.blue)
                    .Map(3, Color.green)
                    .Map(4, Color.yellow)
                    .Else(Color.white)
            );

            playerProfile.GetComponent<PlayerProfile>().SetPlayerName(Client.Instance.Friends.GetName(playerId));
            _playerProfiles.Add(new ProfileInfo(playerProfile, playerId));
        }

        public void ReadyPlayerProfile(ulong id)
        {
            var profile = _playerProfiles.FirstOrDefault(x => x.PlayerId == id);
            profile.Profile.transform.Find("ReadyBadge").GetComponent<Image>().color = Color.green;
        }

        public void UnreadyPlayerProfile(ulong id)
        {
            var profile = _playerProfiles.FirstOrDefault(x => x.PlayerId == id);
            profile.Profile.transform.Find("ReadyBadge").GetComponent<Image>().color = Color.white;
        }

        public void RemovePlayerProfile(ulong id)
        {
            --_playerCount;
            var profile = _playerProfiles.FirstOrDefault(x => x.PlayerId == id);
            _playerProfiles.Remove(profile);
            Destroy(profile.Profile);
        }

        public void ClearPlayerProfiles()
        {
            _playerCount = 0;
            _playerProfiles.Clear();
            transform.ClearChildren();
        }

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
    }
}