using System;
using DATA;

namespace PLAYER
{
    public class TonkyBehaviors : Behaviors
    {
        protected override void RunAction()
        {
            switch (Data.CurrentAction)
            {
                case Types.ActionType.WALK:
                    Data.Acceleration.x = 2f;
                    Data.TerminalVelocity.x = 2.5f;
                    break;

                case Types.ActionType.RUN:
                    Data.Acceleration.x = 4f;
                    Data.TerminalVelocity.x = 5f;
                    break;

                case Types.ActionType.NOTHING:
                    Data.Acceleration.x = 0f;
                    Data.TerminalVelocity.x = 0f;
                    break;
                case Types.ActionType.IDLE:
                    Data.Acceleration.x = 0f;
                    Data.TerminalVelocity.x = 0f;
                    break;
                case Types.ActionType.JAB:
                    break;
                case Types.ActionType.FTILT:
                    break;
                case Types.ActionType.DTILT:
                    break;
                case Types.ActionType.UTILT:
                    break;
                case Types.ActionType.NAIR:
                    break;
                case Types.ActionType.FAIR:
                    break;
                case Types.ActionType.DAIR:
                    break;
                case Types.ActionType.UAIR:
                    break;
                case Types.ActionType.BAIR:
                    break;
                case Types.ActionType.DASHATK:
                    break;
                case Types.ActionType.NSPECIAL:
                    break;
                case Types.ActionType.FSPECIAL:
                    break;
                case Types.ActionType.AIRFSPECIAL:
                    break;
                case Types.ActionType.DSPECIAL:
                    break;
                case Types.ActionType.USPECIAL:
                    break;
                case Types.ActionType.GRAB:
                    break;
                case Types.ActionType.FTHROW:
                    break;
                case Types.ActionType.DTHROW:
                    break;
                case Types.ActionType.UTHROW:
                    break;
                case Types.ActionType.BTHROW:
                    break;
                case Types.ActionType.SHIELD:
                    break;
                case Types.ActionType.ROLL:
                    break;
                case Types.ActionType.AIRDODGE:
                    break;
                case Types.ActionType.SPOTDODGE:
                    break;
                case Types.ActionType.FHOP:
                    break;
                case Types.ActionType.SHOP:
                    break;
                case Types.ActionType.DASH:
                    break;
                case Types.ActionType.ASSIST:
                    break;
                case Types.ActionType.TURN:
                    Data.Acceleration.x = 1f;
                    Data.TerminalVelocity.x = 2.5f;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(Data.CurrentAction), Data.CurrentAction, null);
            }
        }
    }
}