using UnityEngine;
using System.Collections;

public class PlayerInfoController : MonoBehaviour
{
    public int playerIndex;
    public UnityEngine.UI.InputField playerName;

    private MainMenuController _mainMenu;
    private GameSettings.PlayerInfo _player;

    public void Awake()
    {
        _mainMenu = GetComponentInParent<MainMenuController>();
    }

    public void Refresh()
    {
        _player = GameSettings.Instance.players[playerIndex];

        playerName.text = _player.name;
    }

    public void OnNameChanged()
    {
        _player.name = playerName.text;
    }
}
