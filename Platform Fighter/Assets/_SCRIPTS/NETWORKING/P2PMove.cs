using System.Collections.Generic;
using MISC;
using Newtonsoft.Json;
using UnityEngine;

namespace NETWORKING
{
    [System.Serializable]
    public struct P2PMove : IP2PAction
    {
        public int NetworkId;
        
        public Vector2 AddedForce;
        
        public P2PMove(int networkId, Vector2 addedForce)
        {
            NetworkId = networkId;
            AddedForce = addedForce;
        }
        
        public string Serialize()
        {
            return JsonUtility.ToJson(this);
        } 
    }
}