namespace DATA
{
    public static class Types
    {
        public enum ActionType
        {
            Nothing,
            Idle,
            Jab,
            Ftilt,
            Dtilt,
            Utilt,
            Nair,
            Fair,
            Dair,
            Uair,
            Bair,
            Dashatk,
            Nspecial,
            Fspecial,
            Airfspecial,
            Dspecial,
            Uspecial,
            Grab,
            Fthrow,
            Dthrow,
            Uthrow,
            Bthrow,
            Shield,
            Roll,
            Airdodge,
            Spotdodge,
            Fhop,
            Shop,
            Walk,
            Run,
            Dash,
            Assist,
            Turn
        }

        public enum Character
        {
            None,
            TestCharacter
        }

        public enum Direction
        {
            Left,
            Right
        }

        public enum Menu
        {
            BlankMenu,
            MainMenu,
            SingleplayerMenu,
            MultiplayerMenu,
            OptionsMenu,
            LobbySelectMenu,
            LobbyCharacterMenu
        }
    }
}