using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MANAGERS;
using MISC;
using PLAYER;
using UnityEngine;

namespace NETWORKING
{
    public class RollbackManager : Singleton<RollbackManager>
    {
        private List<List<Snapshot>> _snapshots;

        private int _age;
        
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
            
            foreach (var snapshotPiece in _snapshots[distance])
            {
                var packet = JsonUtility.FromJson(snapshotPiece.JsonData, snapshotPiece.Type);
                
                if (snapshotPiece.BaseType.IsSubclassOf(typeof(Singleton)))
                {
                    P2PHandler.Instance.SetData(packet);
                    //var type = typeof(SettableSingleton<>).MakeGenericType(snapshotPiece.BaseType);
                    //(type.GetProperty("Instance", BindingFlags.Public | BindingFlags.Static)?.GetValue(null) as dynamic)?.SetData(packet);                              
                }
                else
                {
                    ((ISettable) MatchStateManager.Instance.GetPlayer(snapshotPiece.Player)
                        .GetComponent(snapshotPiece.BaseType)).SetData(packet);
                }

                Debug.Log("ROLLBACK PART 2");
            }

            var snapshotAge = (P2PHandler.Instance.InputPacketsSent - _age) % 600;
            Debug.Log("SnapshotAge: " + snapshotAge);
            for (var i = 0; i < snapshotAge; ++i)
            {
                for (var j = 0; j < MatchStateManager.Instance.Players.Count; j++)
                {
                    var player = MatchStateManager.Instance.Players[j];
                    if (j == 0)
                    {
                        P2PHandler.Instance.InputPacketsReceived = player.GetComponent<InputSender>()
                            .ArchivedInputSets.Last().PacketNumber;
                    }

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

            Debug.Log("SAVE GAME STATE");
        }

        public void TakeSnapshot<T>(int player, int depth, Type baseType, T structure)
        {
            var json = JsonUtility.ToJson(structure);
            _snapshots[depth].Add(new Snapshot(player, baseType, structure.GetType(), json));
            _age = P2PHandler.Instance.InputPacketsSent;
        }        
    }
}