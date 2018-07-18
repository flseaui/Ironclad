using DATA;
using MISC;

namespace MANAGERS
{
    public class MainMenuManager : Singleton<MainMenuManager>
    {
        public Types.Character Player0Character, Player1Character;

        public Types.Character GetCharacter(int player)
        {
            switch (player)
            {
                case 0:
                    return Player0Character;
                case 1:
                    return Player1Character;
                default:
                    return Player0Character;
            }
        }

        public void SetCharacter(int player, Types.Character character)
        {
            switch (player)
            {
                case 0:
                    Player0Character = character;
                    break;
                case 1:
                    Player1Character = character;
                    break;
            }
        }
    }
}