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
        private Dictionary<int, List<Snapshot>> _snapshots;
        
        private List<(int, Steppable)> _steppables;

        private readonly int MAX_SNAPSHOTS = 20;

        public void AddSteppable(Steppable steppable, int stepOrder)
        {
            _steppables.Add((stepOrder, steppable));
            Debug.Log($"Added Steppable {steppable.name} with stepOrder {stepOrder}");
        }

        private void Awake()
        {
            _snapshots = new Dictionary<int, List<Snapshot>>();
            _steppables = new List<(int, Steppable)>();
        }

        private void Start()
        {
            _steppables = _steppables.OrderBy(x => x.Item2.GetComponent<NetworkIdentity>().Id).ThenBy(x => x.Item1).ToList();
            //_steppables = _steppables.OrderBy(steppable => steppable.Key).ThenBy(steppable => steppable.Value.Item1.GetComponent<NetworkIdentity>().Id)
              //  .ToDictionary(x => x.Key, x => x.Value);

            foreach (var s in _steppables)
            {
                Debug.Log($"[STEPPABLE] stepOrder: {s.Item1}, networkId: {s.Item2.GetComponent<NetworkIdentity>().Id}");
            }
        }

        /// <summary>
        ///     Rollback the game state to a previous iteration
        /// </summary>
        public void Rollback(int distance)
        {
            var closestKey = 0;
            foreach (var snapshot in _snapshots)
            {
                if (snapshot.Key > distance) continue;
                if (Math.Abs(closestKey - snapshot.Key) < closestKey)
                    closestKey = snapshot.Key;
            }
            foreach (var snapshotPiece in _snapshots[closestKey])
            {
                var packet = JsonUtility.FromJson(snapshotPiece.JsonData, snapshotPiece.Type);

                if (snapshotPiece.BaseType.IsSubclassOf(typeof(Singleton)))
                    P2PHandler.Instance.SetData(packet);
                else
                    ((ISettable) MatchStateManager.Instance.GetPlayer(snapshotPiece.Player)
                        .GetComponent(snapshotPiece.BaseType)).SetData(packet);
            }

            //var snapshotAge = Mod(P2PHandler.Instance.InputPacketsSent - _age, 600) + 1;
            /*var player0 = MatchStateManager.Instance.GetPlayer(0);
            var snapshotAge = player0.GetComponent<NetworkInput>()
                ? player0.GetComponent<NetworkInput>().ArchivedInputSets.Count
                : player0.GetComponent<NetworkUserInput>().ArchivedInputSets.Count;
            for (var index = 1; index < MatchStateManager.Instance.Players.Count; index++)
            {
                var player = MatchStateManager.Instance.Players[index];
                var count = player.GetComponent<NetworkInput>()
                    ? player.GetComponent<NetworkInput>().ArchivedInputSets.Count
                    : player.GetComponent<NetworkUserInput>().ArchivedInputSets.Count;
                if (count <= snapshotAge) snapshotAge = count;
            }*/

            var snapshotAge = distance;
            
            Debug.Log($"ROLLED BACK TO {P2PHandler.Instance.DataPacket.FrameCounter}");
            
            Debug.Log("SnapshotAge: " + snapshotAge);

            int lastOrder = _steppables[0].Item1;
            for (var i = 0; i < snapshotAge; ++i)
            {
                for (var j = 0; j < _steppables.Count; j++)
                {
                    if (_steppables[j].Item1 < lastOrder || _steppables[j].Item1 == 0)
                    {
                        Debug.Log($"Applying on {P2PHandler.Instance.DataPacket.FrameCounter}");
                        _steppables[j].Item2.GetComponent<InputSender>().ApplyArchivedInputSet(i);
                    }

                    Debug.Log($"[STEPPED - {_steppables[j].Item1}]: {_steppables[j].Item2.GetType()}");
                    _steppables[j].Item2.ControlledStep();
                    
                    lastOrder = _steppables[j].Item1;
                }

                P2PHandler.Instance.IncrementFrameCounter();
            }
        }

        public void SaveGameState(int frame)
        {
            Debug.Log($"[SaveGameState] on: {P2PHandler.Instance.DataPacket.FrameCounter}, from: {frame}");
            if (_snapshots.Count > MAX_SNAPSHOTS)
            {
                _snapshots.Remove(0);
            }
            _snapshots.Add(frame, new List<Snapshot>());
            foreach (var player in MatchStateManager.Instance.Players)
                TakeSnapshot(player.GetComponent<NetworkIdentity>().Id, _snapshots.Count - 1, typeof(PlayerData),
                    player.GetComponent<PlayerData>().DataPacket);
            TakeSnapshot(-1, _snapshots.Count - 1, typeof(P2PHandler), P2PHandler.Instance.DataPacket);

            foreach (var player in MatchStateManager.Instance.Players)
            {
                if (frame < 0)
                {
                    player.GetComponent<InputSender>().ArchivedInputSets.Clear();
                }
                else
                {
                    var sets = player.GetComponent<InputSender>().ArchivedInputSets;
                    var setCount = sets.Count;
                    for (var i = 0; i < setCount; i++)
                    {
                        if (sets[0].PacketNumber > frame)
                            break;
                        else
                            player.GetComponent<InputSender>().ArchivedInputSets.RemoveAt(0);

                        if (i == setCount)
                            player.GetComponent<NetworkUserInput>()?.ApplyLastInputSet();
                    }

                    
                    string temp = "";
                    foreach (var input in player.GetComponent<InputSender>().ArchivedInputSets)
                    {
                        temp += input.PacketNumber + Environment.NewLine;
                    }
                    Debug.Log("ArchivedInputSets contains: " + temp);
                }
            }
        }

        public void TakeSnapshot<T>(int player, int depth, Type baseType, T structure)
        {
            var json = JsonUtility.ToJson(structure);
            _snapshots[depth].Add(new Snapshot(player, baseType, structure.GetType(), json));
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