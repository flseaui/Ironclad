using System;
using UnityEngine;
using Types = DATA.Types;

namespace NETWORKING
{
    [Serializable]
    public struct InputChange
    {
        public Types.Input InputType;

        // false = down/left
        // true = up/right
        public bool State;

        // -1 = pressed
        // >0 = frames input held down for
        public int FramesHeld;

        public InputChange(Types.Input inputType, bool state, int framesHeld = -1)
        {
            InputType = inputType;
            State = state;
            FramesHeld = framesHeld;
        }
    }
    
    [Serializable]
    public struct P2PInputSet : IP2PAction
    {
        public InputChange[] Inputs;

        public Vector2 Angle;
        
        public int PacketNumber, LoopNumber;

        public P2PInputSet(InputChange[] inputs, Vector2 angle, int packetNumber, int loopNumber)
        {
            Inputs = inputs;
            PacketNumber = packetNumber;
            LoopNumber = loopNumber;
            Angle = angle;
        }

        public string Serialize()
        {
            return JsonUtility.ToJson(this);
        }
    }

    [Serializable]
    public struct P2PJoin : IP2PAction
    {
        public string Serialize()
        {
            return JsonUtility.ToJson(this);
        }
    }

    [Serializable]
    public struct P2PPing : IP2PAction
    {
        public int SentTime;

        public P2PPing(int sentTime)
        {
            SentTime = sentTime;
        }

        public string Serialize()
        {
            return JsonUtility.ToJson(this);
        }
    }
}