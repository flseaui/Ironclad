using System;
using DATA;

namespace PLAYER
{
    public class TonkyBehaviors : Behaviors
    {
        public override void RunAction()
        {
            Data.TargetVelocity = PlayerController.CurrentActionProperties.DetailedVelocity.Velocity *
                                  (Data.Direction == Types.Direction.Left ? -1 : 1);
            Data.Acceleration.x = .5f;

            switch (Data.CurrentAction)
            {
                case Types.ActionType.Walk:
                    break;
                case Types.ActionType.Run:
                    break;
                case Types.ActionType.Nothing:
                    break;
                case Types.ActionType.Idle:
                    break;
                case Types.ActionType.Jab:
                    break;
                case Types.ActionType.Ftilt:
                    break;
                case Types.ActionType.Dtilt:
                    break;
                case Types.ActionType.Utilt:
                    break;
                case Types.ActionType.Nair:
                    break;
                case Types.ActionType.Fair:
                    break;
                case Types.ActionType.Dair:
                    break;
                case Types.ActionType.Uair:
                    break;
                case Types.ActionType.Bair:
                    break;
                case Types.ActionType.Dashatk:
                    break;
                case Types.ActionType.Nspecial:
                    break;
                case Types.ActionType.Fspecial:
                    break;
                case Types.ActionType.Airfspecial:
                    break;
                case Types.ActionType.Dspecial:
                    break;
                case Types.ActionType.Uspecial:
                    break;
                case Types.ActionType.Grab:
                    break;
                case Types.ActionType.Fthrow:
                    break;
                case Types.ActionType.Dthrow:
                    break;
                case Types.ActionType.Uthrow:
                    break;
                case Types.ActionType.Bthrow:
                    break;
                case Types.ActionType.Shield:
                    break;
                case Types.ActionType.Roll:
                    break;
                case Types.ActionType.Airdodge:
                    break;
                case Types.ActionType.Spotdodge:
                    break;
                case Types.ActionType.Fhop:
                    break;
                case Types.ActionType.Shop:
                    break;
                case Types.ActionType.Dash:
                    break;
                case Types.ActionType.Assist:
                    break;
                case Types.ActionType.Turn:
                    //Data.Acceleration.x = 1f;
                    Data.TerminalVelocity.x = 2.5f;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(Data.CurrentAction), Data.CurrentAction, null);
            }
        }
    }
}