using System;
using UnityEngine;
using Types = DATA.Types;

namespace NETWORKING
{
    [Serializable]
    public struct P2PInputSet : IP2PAction
    {
        public int NetworkId;

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
        
        public P2PInputSet(int networkId, InputChange[] inputs)
        {
            NetworkId = networkId;
            Inputs = inputs;
        }

        public string Serialize() => JsonUtility.ToJson(this);
    }
    
    [Serializable]
    public struct P2PMove : IP2PAction
    {
        public int NetworkId;

        public Vector2 AddedForce;

        public P2PMove(int networkId, Vector2 addedForce)
        {
            NetworkId = networkId;
            AddedForce = addedForce;
        }

        public string Serialize() => JsonUtility.ToJson(this);
    }

    [Serializable]
    public struct P2PSpawn : IP2PAction
    {
        public int NetworkId;
        
        public P2PSpawn(int networkId)
        {
            NetworkId = networkId;
        }
        
        public string Serialize() => JsonUtility.ToJson(this);
    }
}