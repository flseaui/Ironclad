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

        public InputChange[] Inputs;

        public int PacketNumber;

        public P2PInputSet(InputChange[] inputs, int packetNumber)
        {
            Inputs = inputs;
            PacketNumber = packetNumber;
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
        public int LocalFrame;

        public P2PPing(int localFrame)
        {
            LocalFrame = localFrame;
        }

        public string Serialize()
        {
            return JsonUtility.ToJson(this);
        }
    }
}