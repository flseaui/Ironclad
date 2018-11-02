using System;
using DATA;
using NETWORKING;

namespace PLAYER
{
    public class NetworkInput : InputSender
    {
        public bool HasInputs { get; set; }

        private P2PInputSet.InputChange[] ChangedInputs { get; set; }

        public void GiveInputs(P2PInputSet.InputChange[] changedInputs)
        {
            ChangedInputs = changedInputs;
            HasInputs = true;
        }
        
        private void Update()
        {
            if (HasInputs)
            {
                ParseInputs(ChangedInputs);
                HasInputs = false;
            }
            else
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
        }
    }
}