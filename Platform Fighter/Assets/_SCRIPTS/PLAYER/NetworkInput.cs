using System;
using DATA;
using NETWORKING;

namespace PLAYER
{
    public class NetworkInput : InputSender
    {
        public bool ShouldPredictInputs { get; set; }
        
        private void Update()
        {
            if (ShouldPredictInputs)
                ParseInputs(PredictInputs());
        }

        private P2PInputSet.InputChange[] PredictInputs()
        {
            return new P2PInputSet.InputChange[] { new P2PInputSet.InputChange(Types.Input.LightRight, true) };
        }
        
        public void ParseInputs(P2PInputSet.InputChange[] inputs)
        {
            foreach (var input in inputs)
            {
                Inputs[(int) input.InputType] = input.State;
            }

            ShouldPredictInputs = true;
        }
    }
}