using UnityEngine;
using System.Collections;

public class PlayerInfoController : MonoBehaviour
{
    [SerializeField]
    private UnityEngine.UI.InputField playerName;

    [SerializeField]
    private int playerIndex;

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
