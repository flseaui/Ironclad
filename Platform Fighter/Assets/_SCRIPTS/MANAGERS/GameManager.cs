using DATA;
using MISC;

namespace MANAGERS
{
    public class GameManager : Singleton<GameManager>
    {
        public Types.Character[] Characters { get; set; } =
        {
            Types.Character.TestCharacter,
            Types.Character.TestCharacter
        };

        private void StartGame() { }
    }
}