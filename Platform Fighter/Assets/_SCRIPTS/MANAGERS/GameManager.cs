using MISC;

namespace MANAGERS
{
    public class GameManager : Singleton<GameManager>
    {
        public DATA.Types.Character[] Characters { get; set; } = 
        {
            DATA.Types.Character.TestCharacter,
            DATA.Types.Character.TestCharacter
        };
        
        private void StartGame()
        {
            
        }
    }
}