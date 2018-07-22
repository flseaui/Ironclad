/** This class is Auto-Generated **/

using UnityEditor;

namespace Editor.Generated
{
    public static class GMenuSwapMenuItems {

    [MenuItem("Menu/BlankMenu")]
    private static void SwapToBlankMenu() => MenuSwap.SwapMenu("BlankMenu");

    [MenuItem("Menu/MainMenu")]
    private static void SwapToMainMenu() => MenuSwap.SwapMenu("MainMenu");

    [MenuItem("Menu/SingleplayerMenu")]
    private static void SwapToSingleplayerMenu() => MenuSwap.SwapMenu("SingleplayerMenu");

    [MenuItem("Menu/MultiplayerMenu")]
    private static void SwapToMultiplayerMenu() => MenuSwap.SwapMenu("MultiplayerMenu");

    [MenuItem("Menu/OptionsMenu")]
    private static void SwapToOptionsMenu() => MenuSwap.SwapMenu("OptionsMenu");

    [MenuItem("Menu/LobbySelectMenu")]
    private static void SwapToLobbySelectMenu() => MenuSwap.SwapMenu("LobbySelectMenu");

    [MenuItem("Menu/LobbyCharacterMenu")]
    private static void SwapToLobbyCharacterMenu() => MenuSwap.SwapMenu("LobbyCharacterMenu");


    }
}
