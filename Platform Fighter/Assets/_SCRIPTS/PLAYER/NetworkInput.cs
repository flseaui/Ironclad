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
                Inputs[(int) input.InputType] = input.State;
            }
        }
    }
}