using System;
using DATA;
using NETWORKING;

namespace PLAYER
{
    public class NetworkInput : InputSender
    {
        public void ParseInputs(P2PInputSet.InputChange[] inputs)
        {
            foreach (var input in inputs)
            {
                switch (input.InputType)
                {
                    case Types.Input.LightLeft:
                        Inputs[(int) Types.Input.LightLeft] = input.State;
                        break;
                    case Types.Input.StrongLeft:
                        Inputs[(int) Types.Input.StrongLeft] = input.State;
                        break;
                    case Types.Input.LightRight:
                        Inputs[(int) Types.Input.LightRight] = input.State;
                        break;
                    case Types.Input.StrongRight:
                        Inputs[(int) Types.Input.StrongRight] = input.State;
                        break;
                    case Types.Input.Up:
                        Inputs[(int) Types.Input.Up] = input.State;
                        break;
                    case Types.Input.Down:
                        Inputs[(int) Types.Input.Down] = input.State;
                        break;
                    case Types.Input.ShortHop:
                        Inputs[(int) Types.Input.ShortHop] = input.State;
                        break;
                    case Types.Input.FullHop:
                        Inputs[(int) Types.Input.FullHop] = input.State;
                        break;
                    case Types.Input.Neutral:
                        Inputs[(int) Types.Input.Neutral] = input.State;
                        break;
                    case Types.Input.Special:
                        Inputs[(int) Types.Input.Special] = input.State;
                        break;
                    case Types.Input.Shield:
                        Inputs[(int) Types.Input.Shield] = input.State;
                        break;
                    case Types.Input.Grab:
                        Inputs[(int) Types.Input.Grab] = input.State;
                        break;
                    case Types.Input.UpC:
                        Inputs[(int) Types.Input.UpC] = input.State;
                        break;
                    case Types.Input.DownC:
                        Inputs[(int) Types.Input.DownC] = input.State;
                        break;
                    case Types.Input.LeftC:
                        Inputs[(int) Types.Input.LeftC] = input.State;
                        break;
                    case Types.Input.RightC:
                        Inputs[(int) Types.Input.RightC] = input.State;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
    }
}