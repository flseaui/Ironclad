using System;
using DATA;

namespace PLAYER
{
    public class TonkyBehaviors : Behaviors
    {
        public override void RunAction()
        {
            Data.VelocityModifier = PlayerController.CurrentActionProperties.DetailedVelocity.Modification;

            Data.TargetVelocity.x =
                PlayerController.CurrentActionProperties.DetailedVelocity.Velocity.x *
                (Data.Direction == Types.Direction.Left ? -1 : 1);

            Data.TargetVelocity.y =
                PlayerController.CurrentActionProperties.DetailedVelocity.Velocity.y;

            Data.Acceleration.x = 5f;

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
                case Types.ActionType.Nstrong:
                    break;
                case Types.ActionType.Fstrong:
                    break;
                case Types.ActionType.Dstrong:
                    break;
                case Types.ActionType.Ustrong:
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
                case Types.ActionType.Jump:
                    ApplyArielMovement(Data.MovementStickAngle.x,
                        Data.VelocityModifier == ActionInfo.VelocityModifier.ModificationType.IgnoreBoth ||
                        Data.VelocityModifier == ActionInfo.VelocityModifier.ModificationType.IgnoreY);
                    break;
                case Types.ActionType.Fall:
                    ApplyArielMovement(Data.MovementStickAngle.x,
                        Data.VelocityModifier == ActionInfo.VelocityModifier.ModificationType.IgnoreBoth ||
                        Data.VelocityModifier == ActionInfo.VelocityModifier.ModificationType.IgnoreY);
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

        public void ApplyArielMovement(float amount, bool modAsIgnoreBoth)
        {
            Data.TargetVelocity.x += amount * 0.025f;

            if (modAsIgnoreBoth)
                Data.VelocityModifier = ActionInfo.VelocityModifier.ModificationType.IgnoreY;
            else
                Data.VelocityModifier = ActionInfo.VelocityModifier.ModificationType.Target;
        }
    }
}