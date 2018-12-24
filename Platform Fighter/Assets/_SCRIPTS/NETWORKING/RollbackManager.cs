using System;
using System.Collections.Generic;
using MANAGERS;
using MISC;
using PLAYER;
using UnityEngine;

namespace NETWORKING
{
    public class RollbackManager : Singleton<RollbackManager>
    {
        private List<List<Snapshot>> _snapshots;

        private struct Snapshot
        {
            /// <summary>
            /// >=0 : player id
            /// -1 : not associated with player
            /// </summary>
            public int Player;
            public Type BaseType;
            public Type Type;
            public string JsonData;
            
            public Snapshot(int player, Type baseType, Type type, string json)
            {
                Player = player;
                BaseType = baseType;
                Type = type;
                JsonData = json;
            }
        }
        
        private void Start()
        {
            _snapshots = new List<List<Snapshot>>();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F1))
            {
                SaveGameState();
            }

            if (Input.GetKeyDown(KeyCode.F2))
            {
                Rollback(0);
            }
            
        }

        /// <summary>
        /// Rollback the game state to a previous iteration 
        /// </summary>
        /// <param name="distance"> Number of snapshots to rollback. </param>
        public void Rollback(int distance)
        {
            Debug.Log("ROLLBACK PART 1");
           
            if (distance < 0 || distance > _snapshots.Count)
            {
                return;
            }

            P2PHandler.Instance.Threshold = 0;

            var prevInputPacketsSent = P2PHandler.Instance.DataPacket.InputPacketsSent;
            
            foreach (var snapshotPiece in _snapshots[distance])
            {
                var packet = JsonUtility.FromJson(snapshotPiece.JsonData, snapshotPiece.Type);
                
                if (snapshotPiece.BaseType.IsSubclassOf(typeof(Singleton)))
                {
                    dynamic type = typeof(SettableSingleton<>).MakeGenericType(snapshotPiece.BaseType);
                    type.Instance.SetData(packet);
                }
                else
                {
                    ((ISettable) MatchStateManager.Instance.GetPlayer(snapshotPiece.Player)
                        .GetComponent(snapshotPiece.BaseType)).SetData(packet);
                }

                Debug.Log("ROLLBACK PART 2");
            }

            var snapshotAge = prevInputPacketsSent - P2PHandler.Instance.DataPacket.InputPacketsSent;
            for (var i = 0; i < snapshotAge; ++i)
            {
                foreach (var player in MatchStateManager.Instance.Players)
                {
                    player.GetComponent<InputSender>().ApplyArchivedInputSet(i);
                    foreach (var steppable in player.GetComponents(typeof(ISteppable)))
                    {
                        ((ISteppable) steppable).Step();
                    }
                }   
            }
        }

        public void SaveGameState()
        {
            _snapshots.Clear();
            _snapshots.Add(new List<Snapshot>());
            foreach (var player in MatchStateManager.Instance.Players)
            {
                TakeSnapshot(player.GetComponent<NetworkIdentity>().Id, 0, typeof(PlayerData), player.GetComponent<PlayerData>().DataPacket);
            }
            TakeSnapshot(-1, 0, typeof(P2PHandler), P2PHandler.Instance.DataPacket);

            foreach (var player in MatchStateManager.Instance.Players)
            {
                player.GetComponent<InputSender>().ArchivedInputSets.Clear();
            }
        }

        public void TakeSnapshot<T>(int player, int depth, Type baseType, T structure)
        {
            var json = JsonUtility.ToJson(structure);
            _snapshots[depth].Add(new Snapshot(player, baseType, structure.GetType(), json));
        }        
    }
}