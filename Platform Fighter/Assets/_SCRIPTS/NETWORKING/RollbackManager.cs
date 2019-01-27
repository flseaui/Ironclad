using System;
using System.Collections.Generic;
using System.Linq;
using MANAGERS;
using MISC;
using PLAYER;
using UnityEditor;
using UnityEngine;
using static MISC.MathUtils;

namespace NETWORKING
{
    public class RollbackManager : Singleton<RollbackManager>
    {
        private int _age;
        private RollingList<KeyValuePair<int, List<Snapshot>>> _snapshots;
        
        private List<(int, Steppable)> _steppables;

        private readonly int MAX_SNAPSHOTS = 20;

        public void AddSteppable(Steppable steppable, int stepOrder)
        {
            _steppables.Add((stepOrder, steppable));
        }

        protected override void OnAwake()
        {
            _snapshots = new  RollingList<KeyValuePair<int, List<Snapshot>>>(MAX_SNAPSHOTS);
            _steppables = new List<(int, Steppable)>();
        }

        private void Start()
        {
            _steppables = _steppables.OrderBy(x => x.Item2.GetComponent<NetworkIdentity>().Id).ThenBy(x => x.Item1).ToList();
        }

        /// <summary>
        ///     Rollback the game state to a previous iteration
        /// </summary>
        public void Rollback(int distance)
        {
            Debug.Log($"index: {_snapshots.Count - 1}, count: {_snapshots.Count}");
            var closestKey = _snapshots[_snapshots.Count - 1].Key;
            
            if (closestKey > distance)
            {
                for (var i = _snapshots.Count - 1; i >= 0; i--)
                {
                    var snapshot = _snapshots[i];
                    
                    if (snapshot.Key > distance) continue;

                    closestKey = snapshot.Key;
                    break;
                }
            }

            foreach (var snapshotPiece in _snapshots.FirstOrDefault(x => x.Key == closestKey).Value)
            {
                var packet = JsonUtility.FromJson(snapshotPiece.JsonData, snapshotPiece.Type);

                if (snapshotPiece.BaseType.IsSubclassOf(typeof(Singleton)))
                    P2PHandler.Instance.SetData(packet);
                else
                    ((ISettable) MatchStateManager.Instance.GetPlayer(snapshotPiece.Player)
                        .GetComponent(snapshotPiece.BaseType)).SetData(packet);
            }

            var snapshotAge = Math.Abs(distance - P2PHandler.Instance.DataPacket.FrameCounter);
                        
            Debug.Log("snapshotAge: " + snapshotAge);
            Debug.Log($"ROLLED BACK TO ({P2PHandler.Instance.DataPacket.FrameCounter}, {P2PHandler.Instance.DataPacket.FrameCounterLoops})");
            
            foreach (var player in MatchStateManager.Instance.Players)
            {
                var sets = player.GetComponent<InputSender>().ArchivedInputSets;
                var setCount = sets.Count;
                Debug.Log($"player: {player.name}, setCount: {setCount}");
                for (var i = 0; i < setCount; i++)
                {
                    if (sets[0].LoopNumber > P2PHandler.Instance.DataPacket.FrameCounterLoops)
                        break;
                    
                    if (sets[0].LoopNumber == P2PHandler.Instance.DataPacket.FrameCounterLoops)
                        if (sets[0].PacketNumber >= P2PHandler.Instance.DataPacket.FrameCounter)
                            break;
                    
                    player.GetComponent<InputSender>().ArchivedInputSets.RemoveAt(0);
                }
            }
            
            var lastOrder = _steppables[0].Item1;
            for (var i = 0; i <= snapshotAge; ++i)
            {
                for (var j = 0; j < _steppables.Count; j++)
                {
                    if (_steppables[j].Item1 < lastOrder || _steppables[j].Item1 == 0)
                    {
                        if (_steppables[j].Item2.GetComponent<NetworkInput>() != null)
                        {
                            _steppables[j].Item2.GetComponent<NetworkInput>().ApplyArchivedInputSet(i);
                        }
                        else
                        {   
                            _steppables[j].Item2.GetComponent<InputSender>().ApplyArchivedInputSet(i);
                        }
                    }

                    _steppables[j].Item2.ControlledStep();
                    
                    lastOrder = _steppables[j].Item1;
                }

                if (i != snapshotAge)
                    P2PHandler.Instance.IncrementFrameCounter();
            }
        }

        public void SaveGameState(int frame)
        {
            Debug.Log($"[SaveGameState] on: {P2PHandler.Instance.DataPacket.FrameCounter}, from: {frame}");
            _snapshots.Add(new KeyValuePair<int, List<Snapshot>>(frame, new List<Snapshot>()));
            foreach (var player in MatchStateManager.Instance.Players)
                TakeSnapshot(player.GetComponent<NetworkIdentity>().Id, _snapshots.Count - 1, typeof(PlayerData),
                    player.GetComponent<PlayerData>().DataPacket);
            TakeSnapshot(-1, _snapshots.Count - 1, typeof(P2PHandler), P2PHandler.Instance.DataPacket);
        }

        public void TakeSnapshot<T>(int player, int depth, Type baseType, T structure)
        {
            var json = JsonUtility.ToJson(structure);
            _snapshots[depth].Value.Add(new Snapshot(player, baseType, structure.GetType(), json));
            _age = P2PHandler.Instance.InputPacketsSent;
        }

        private struct Snapshot
        {
            /// <summary>
            ///     >=0 : player id
            ///     -1 : not associated with player
            /// </summary>
            public readonly int Player;

            public readonly Type BaseType;
            public readonly Type Type;
            public readonly string JsonData;

            public Snapshot(int player, Type baseType, Type type, string json)
            {
                Player = player;
                BaseType = baseType;
                Type = type;
                JsonData = json;
            }
        }
    }
}