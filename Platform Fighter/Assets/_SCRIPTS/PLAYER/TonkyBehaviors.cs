using System;
using UnityEngine;
using static DATA.Types;

namespace PLAYER
{
    public class TonkyBehaviors : Behaviors
    {
        protected override void RunAction(ActionType action)
        {
            switch (action)
            {

                case (ActionType.WALK):
                    Data.Acceleration = .25;
                    Data.TerminalVelocity = 2.5;
                    break;

                case (ActionType.RUN):
                    Data.Acceleration = .25;
                    Data.TerminalVelocity = 5;
                    break;

                case ActionType.NOTHING:
                    break;
                case ActionType.IDLE:
                    break;
                case ActionType.JAB:
                    break;
                case ActionType.FTILT:
                    break;
                case ActionType.DTILT:
                    break;
                case ActionType.UTILT:
                    break;
                case ActionType.NAIR:
                    break;
                case ActionType.FAIR:
                    break;
                case ActionType.DAIR:
                    break;
                case ActionType.UAIR:
                    break;
                case ActionType.BAIR:
                    break;
                case ActionType.DASHATK:
                    break;
                case ActionType.NSPECIAL:
                    break;
                case ActionType.FSPECIAL:
                    break;
                case ActionType.AIRFSPECIAL:
                    break;
                case ActionType.DSPECIAL:
                    break;
                case ActionType.USPECIAL:
                    break;
                case ActionType.GRAB:
                    break;
                case ActionType.FTHROW:
                    break;
                case ActionType.DTHROW:
                    break;
                case ActionType.UTHROW:
                    break;
                case ActionType.BTHROW:
                    break;
                case ActionType.SHIELD:
                    break;
                case ActionType.ROLL:
                    break;
                case ActionType.AIRDODGE:
                    break;
                case ActionType.SPOTDODGE:
                    break;
                case ActionType.FHOP:
                    break;
                case ActionType.SHOP:
                    break;
                case ActionType.DASH:
                    break;
                case ActionType.ASSIST:
                    break;
                case ActionType.TURN:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(action), action, null);
            }
        }
    }
}
