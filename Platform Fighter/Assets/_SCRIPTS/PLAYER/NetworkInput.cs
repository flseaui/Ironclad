using System;
using DATA;
using MANAGERS;
using NETWORKING;

namespace PLAYER
{
    public class NetworkInput : InputSender
    {
        public bool HasInputs { get; set; }

        private bool _predicting;
        private int _framesOfPrediction;
        
        private P2PInputSet.InputChange[] ChangedInputs { get; set; }

        public void GiveInputs(P2PInputSet.InputChange[] changedInputs)
        {
            ChangedInputs = changedInputs;
            HasInputs = true;
        }
        
        private void Update()
        {
            if (!MatchStateManager.Instance.ReadyToFight)
                return;
            
            if (HasInputs)
            {
                RollbackManager.Instance.SaveGameState();
                
                if (_predicting)
                {
                    _predicting = false;
                    RollbackManager.Instance.Rollback(0);
                }
                ParseInputs(ChangedInputs);
                HasInputs = false;
            }
            else
            {
                _predicting = true;
                ++_framesOfPrediction;
                ParseInputs(PredictInputs());
            }
        }

        private P2PInputSet.InputChange[] PredictInputs()
        {
            return new P2PInputSet.InputChange[] {  };
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