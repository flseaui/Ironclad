namespace DATA
{
    public static class Types
    {
        public enum Character
        {
            NONE,
            TEST_CHARACTER
        }
        public enum Direction
        {
            LEFT,
            RIGHT
        }

        public enum Menu
        {
            BLANK_MENU,
            MAIN_MENU,
            SINGLEPLAYER_MENU,
            MULTIPLAYER_MENU,
            OPTIONS_MENU,
            LOBBY_MENU
        }
        public enum ActionType
        {
            NOTHING,
            IDLE,
            JAB,
            FTILT,
            DTILT,
            UTILT,
            NAIR,
            FAIR,
            DAIR,
            UAIR,
            BAIR,
            DASHATK,
            NSPECIAL,
            FSPECIAL,
            AIRFSPECIAL,
            DSPECIAL,
            USPECIAL,
            GRAB,
            FTHROW,
            DTHROW,
            UTHROW,
            BTHROW,
            SHIELD,
            ROLL,
            AIRDODGE,
            SPOTDODGE,
            FHOP,
            SHOP,
            WALK,
            RUN,
            DASH,
            ASSIST,		
            TURN
        }
    }
}