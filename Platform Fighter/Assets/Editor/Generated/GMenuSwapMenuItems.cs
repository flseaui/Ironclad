/** This class is Auto-Generated **/

using UnityEditor;

namespace Editor.Generated
{
    public static class GMenuSwapMenuItems {

    [MenuItem("Menu/MainMenu")]
    private static void SwapToMainMenu() => MenuSwap.SwapMenu("MainMenu");
    [MenuItem("Menu/MainMenu", true)]
    private static bool SwapToMainMenuValidation() => MenuSwap.SwapMenuValidation("MainMenu");

    [MenuItem("Menu/SingleplayerMenu")]
    private static void SwapToSingleplayerMenu() => MenuSwap.SwapMenu("SingleplayerMenu");
    [MenuItem("Menu/SingleplayerMenu", true)]
    private static bool SwapToSingleplayerMenuValidation() => MenuSwap.SwapMenuValidation("SingleplayerMenu");

    [MenuItem("Menu/MultiplayerMenu")]
    private static void SwapToMultiplayerMenu() => MenuSwap.SwapMenu("MultiplayerMenu");
    [MenuItem("Menu/MultiplayerMenu", true)]
    private static bool SwapToMultiplayerMenuValidation() => MenuSwap.SwapMenuValidation("MultiplayerMenu");

    [MenuItem("Menu/OptionsMenu")]
    private static void SwapToOptionsMenu() => MenuSwap.SwapMenu("OptionsMenu");
    [MenuItem("Menu/OptionsMenu", true)]
    private static bool SwapToOptionsMenuValidation() => MenuSwap.SwapMenuValidation("OptionsMenu");

    [MenuItem("Menu/LobbySelectMenu")]
    private static void SwapToLobbySelectMenu() => MenuSwap.SwapMenu("LobbySelectMenu");
    [MenuItem("Menu/LobbySelectMenu", true)]
    private static bool SwapToLobbySelectMenuValidation() => MenuSwap.SwapMenuValidation("LobbySelectMenu");

    [MenuItem("Menu/LobbyCharacterMenu")]
    private static void SwapToLobbyCharacterMenu() => MenuSwap.SwapMenu("LobbyCharacterMenu");
    [MenuItem("Menu/LobbyCharacterMenu", true)]
    private static bool SwapToLobbyCharacterMenuValidation() => MenuSwap.SwapMenuValidation("LobbyCharacterMenu");

    [MenuItem("Menu/PrivateMatchMenu")]
    private static void SwapToPrivateMatchMenu() => MenuSwap.SwapMenu("PrivateMatchMenu");
    [MenuItem("Menu/PrivateMatchMenu", true)]
    private static bool SwapToPrivateMatchMenuValidation() => MenuSwap.SwapMenuValidation("PrivateMatchMenu");

    [MenuItem("Menu/GameStartMenu")]
    private static void SwapToGameStartMenu() => MenuSwap.SwapMenu("GameStartMenu");
    [MenuItem("Menu/GameStartMenu", true)]
    private static bool SwapToGameStartMenuValidation() => MenuSwap.SwapMenuValidation("GameStartMenu");

    [MenuItem("Menu/CharacterSelectMenu")]
    private static void SwapToCharacterSelectMenu() => MenuSwap.SwapMenu("CharacterSelectMenu");
    [MenuItem("Menu/CharacterSelectMenu", true)]
    private static bool SwapToCharacterSelectMenuValidation() => MenuSwap.SwapMenuValidation("CharacterSelectMenu");


    }
}
