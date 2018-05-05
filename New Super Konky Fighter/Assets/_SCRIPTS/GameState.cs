using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using UnityEngine;
using UnityEngine.Assertions;

public class GameState : ScriptableObject
{

    private static GameState _instance;
    public static GameState Instance
    {
        get
        {
            if (!_instance)
                _instance = Resources.FindObjectsOfTypeAll<GameState>().FirstOrDefault();

#if UNITY_EDITOR
            if (!_instance || _instance.players.Count == 0)
                CreateFromSettings(GameSettings.Instance);
#endif

            return _instance;
        }
    }

    [Serializable]
    public class PlayerState
    {
        public FighterDetails fighter;
        public int totalWins;
        [NonSerialized] public GameSettings.PlayerInfo playerInfo;

        public bool IsAlive { get { return fighter && fighter.gameObject.activeSelf; } }
    }

    public List<PlayerState> players;

    public int roundNumber;

    public static void CreateFromSettings(GameSettings settings)
    {
        Assert.IsNotNull(settings);

        _instance = CreateInstance<GameState>();
        _instance.hideFlags = HideFlags.HideAndDontSave;

        _instance.players = new List<PlayerState>();
        foreach (var _playerInfo in settings.players)
        {
            if (!_playerInfo.fighterBase) continue;

            _instance.players.Add(new PlayerState { playerInfo = _playerInfo });
        }
    }

    public PlayerState this[GameSettings.PlayerInfo playerInfo]
    {
        get { return players.FirstOrDefault(p => p.playerInfo == playerInfo); }
    }

    public PlayerState this[FighterDetails details]
    {
        get { return players.FirstOrDefault(p => p.fighter == details); }
    }

#if UNITY_EDITOR
    [UnityEditor.MenuItem("Window/Game State")]
    public static void ShowGameState()
    {
        UnityEditor.Selection.activeObject = Instance;
    }
#endif

    public PlayerState GetPlayerWithMostWins()
    {
        players.Sort((a, b) => Comparer<int>.Default.Compare(b.totalWins, a.totalWins));
        if (players.Count > 1 && players[0].totalWins == players[1].totalWins) return null; // Draw
        return players[0];
    }
}
