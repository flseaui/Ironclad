using System;
using UnityEngine;
using Types = DATA.Types;

namespace NETWORKING
{
    [Serializable]
    public struct P2PInputSet : IP2PAction
    {
        [Serializable]
        public struct InputChange
        {
            public Types.Input InputType;
            
            // false = down/left
            // true = up/right
            public bool State;

            public InputChange(Types.Input inputType, bool state)
            {
                InputType = inputType;
                State = state;
            }
            
        }

        public InputChange[] Inputs;
        
        public P2PInputSet(InputChange[] inputs)
        {
            Inputs = inputs;
        }

        public string Serialize() => JsonUtility.ToJson(this);
    }

    [Serializable]
    public struct P2PJoin : IP2PAction
    {
        public string Serialize() => JsonUtility.ToJson(this);
    }
}