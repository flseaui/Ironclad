﻿namespace DATA
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
            Nstrong,
            Fstrong,
            Dstrong,
            Ustrong,
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
            Jump,
            Fall,
            FreeFall,
            LedgeGrab,
            Walk,
            Run,
            Dash,
            Assist,
            Turn,
            Stun,
            KnockedDown
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

        public enum PlayerFlags
        {
            ResetAction
        }

        public enum GameFlags
        {
            DelayDecreased
        }
        
        public enum FlagState
        {
            Pending,
            Resolved
        }

        public enum Input
        {
            LightLeft,
            StrongLeft,
            LightRight,
            StrongRight,
            Up,
            Down,
            Jump,
            Neutral,
            Strong,
            Special,
            Shield,
            Grab,
            UpC,
            DownC,
            LeftC,
            RightC
        }

        public enum MatchType
        {
            OfflineSingleplayer,
            OnlineMultiplayer
        }

        public enum Menu
        {
            BlankMenu,
            MainMenu,
            SingleplayerMenu,
            MultiplayerMenu,
            OptionsMenu,
            LobbySelectMenu,
            LobbyCharacterMenu,
            PrivateMatchMenu,
            GameStartMenu,
            CharacterSelectMenu
        }

        public enum Stage
        {
            TestStage
        }

        public enum PauseType
        {
            Update,
            FixedUpdate,
            Both
        }
        
    }
}