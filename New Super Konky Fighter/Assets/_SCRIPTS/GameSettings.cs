using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[CreateAssetMenu]
public class GameSettings : ScriptableObject
{
    #region Singleton
    private static GameSettings _instance;
    public static GameSettings Instance
    {
        get
        {
            if (!_instance)
                _instance = Resources.FindObjectsOfTypeAll<GameSettings>().FirstOrDefault();
#if UNITY_EDITOR
            if (!_instance)
                InitializeFromDefault(UnityEditor.AssetDatabase.LoadAssetAtPath<GameSettings>("Assets/TEST_GAME_SETTINGS.asset"));
#endif
            return _instance;
        }
    }
    #endregion

    [Serializable]
    public class PlayerInfo
    {
        public string name;

        private FighterBase _cachedFighterBase;
        public FighterBase fighterBase
        {
            get
            {
                if (!_cachedFighterBase && !String.IsNullOrEmpty(fighterBaseName))
                {
                    FighterBase[] avaiableFighterBases;

#if UNITY_EDITOR
                    avaiableFighterBases = UnityEditor.AssetDatabase.FindAssets("t:FighterBase")
                                        .Select(guid => UnityEditor.AssetDatabase.GUIDToAssetPath(guid))
                                        .Select(path => UnityEditor.AssetDatabase.LoadAssetAtPath<FighterBase>(path))
                                        .Where(b => b).ToArray();
#else
					avaiableFighterBases = Resources.FindObjectsOfTypeAll<FighterBase>();
#endif

                    _cachedFighterBase = avaiableFighterBases.FirstOrDefault(b => b.name == fighterBaseName);
                }
                return _cachedFighterBase;
            }
            set
            {
                _cachedFighterBase = value;
                fighterBaseName = value ? value.name : String.Empty;
            }
        }

        [SerializeField] private string fighterBaseName;
    }

    public List<PlayerInfo> players;

    public bool DEBUG;

    public static void LoadFromJSON(string path)
    {
        if (!_instance) DestroyImmediate(_instance);
        _instance = ScriptableObject.CreateInstance<GameSettings>();
        JsonUtility.FromJsonOverwrite(System.IO.File.ReadAllText(path), _instance);
        _instance.hideFlags = HideFlags.HideAndDontSave;
    }

    public void SaveToJSON(string path)
    {
        Debug.LogFormat("Saving game settings to {0}", path);
        System.IO.File.WriteAllText(path, JsonUtility.ToJson(this, true));
    }

    public static void InitializeFromDefault(GameSettings settings)
    {
        if (_instance) DestroyImmediate(_instance);
        _instance = Instantiate(settings);
        _instance.hideFlags = HideFlags.HideAndDontSave;
    }

#if UNITY_EDITOR
    [UnityEditor.MenuItem("Window/Game Settings")]
    public static void ShowGameSettings()
    {
        UnityEditor.Selection.activeObject = Instance;
    }
#endif

    public bool ShouldFinishRound()
    {
        return GameState.Instance.players.Count(p => p.IsAlive) <= 1;
    }

}
