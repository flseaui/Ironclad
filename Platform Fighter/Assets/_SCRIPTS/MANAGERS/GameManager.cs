using DATA;
using MISC;

namespace MANAGERS
{
    public class GameManager : Singleton<GameManager>
    {
        public Types.MatchType MatchType { get; set; }

        public Types.Character[] Characters { get; set; } =
        {
            Types.Character.TestCharacter,
            Types.Character.None
        };

        public ulong[] SteamIds { get; set; }

        public Types.Stage Stage { get; set; } = Types.Stage.TestStage;

        private void StartGame()
        {
        }
    }
}