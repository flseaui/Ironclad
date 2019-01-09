using System;

namespace NETWORKING
{
    public static class Events
    {
        public static Action<NetworkIdentity, P2PInputSet.InputChange[], bool> OnInputsChanged;
        public static Action<NetworkIdentity> OnMatchJoined;

        // Users steam id, Recievers steam id, current frame
        public static Action<ulong, ulong, int> OnPingSent;

        public static Action<ulong, ulong, int> OnFirstNetworkLatencyCalculated;
        public static Action<int> OnFinalNetworkLatencyCalculated;
    }
}