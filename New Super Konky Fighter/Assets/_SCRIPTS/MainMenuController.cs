using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public GameSettings gameSettingsTemplate;

    public string savedSettingsPath
    {
        get
        {
            return System.IO.Path.Combine(Application.persistentDataPath, "skf-settings.json");
        }
    }

    void Start ()
	{
        if (System.IO.File.Exists(savedSettingsPath))
            GameSettings.LoadFromJSON(savedSettingsPath);
        else
            GameSettings.InitializeFromDefault(gameSettingsTemplate);

        foreach (var info in GetComponentsInChildren<PlayerInfoController>())
            info.Refresh();
    }

    public void Play()
    {
        GameSettings.Instance.SaveToJSON(savedSettingsPath);
        GameState.CreateFromSettings(GameSettings.Instance);
        SceneManager.LoadScene(1, LoadSceneMode.Single);
    }

    void Update ()
	{
		
	}
}
