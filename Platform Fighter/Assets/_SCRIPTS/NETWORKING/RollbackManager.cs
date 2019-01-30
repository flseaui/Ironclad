using System;
using System.Collections;
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
        
        private List<(int id, Steppable steppable)> _steppables;

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
        public void Rollback(int targetFrame)
        {
            var closestFrame = _snapshots[_snapshots.Count - 1].Key;

            if (closestFrame > targetFrame)
            {
                for (var i = _snapshots.Count - 1; i >= 0; i--)
                {
                    var snapshot = _snapshots[i];
                 
                    Debug.Log($"[SnapshotFrame]: {snapshot.Key}");
                    
                    if (snapshot.Key > targetFrame) continue;

                    closestFrame = snapshot.Key;
                    break;
                }
            }

            var snapshotAge = P2PHandler.Instance.DataPacket.FrameCounter - (closestFrame + 1);
            
            Debug.Log($"targetFrame: {targetFrame}, closestFrame: {closestFrame}");
            
            Debug.Log($"Rolling back from ({P2PHandler.Instance.DataPacket.FrameCounter})");
            
            foreach (var snapshotPiece in _snapshots.FirstOrDefault(x => x.Key == closestFrame).Value)
            {
                var packet = JsonUtility.FromJson(snapshotPiece.JsonData, snapshotPiece.Type);

                if (snapshotPiece.BaseType.IsSubclassOf(typeof(Singleton)))
                    P2PHandler.Instance.SetData(packet);
                else
                    ((ISettable) MatchStateManager.Instance.GetPlayer(snapshotPiece.Player)
                        .GetComponent(snapshotPiece.BaseType)).SetData(packet);
            }

            Debug.Log("snapshotAge: " + snapshotAge);
            Debug.Log($"ROLLED BACK TO ({P2PHandler.Instance.DataPacket.FrameCounter})");
            
            foreach (var player in MatchStateManager.Instance.Players)
            {
                var sets = player.GetComponent<InputSender>().ArchivedInputSets;
                var setCount = sets.Count;
                Debug.Log($"player: {player.name}, setCount: {setCount}");
                for (var i = 0; i < setCount; i++)
                {
                    //Debug.Log("sets packetnum: " + sets[0].PacketNumber);

                    if (sets[0].PacketNumber > P2PHandler.Instance.DataPacket.FrameCounter)
                        break;
                    
                    player.GetComponent<InputSender>().ArchivedInputSets.RemoveAt(0);
                }
            }
            
            var lastOrder = _steppables[0].id;
            for (var i = 0; i < snapshotAge; ++i)
            {
                for (var j = 0; j < _steppables.Count; j++)
                {
                    if (_steppables[j].id < lastOrder || _steppables[j].id == 0)
                    {
                        if (_steppables[j].steppable.GetComponent<NetworkInput>() != null)
                        {
                            _steppables[j].steppable.GetComponent<NetworkInput>().ApplyArchivedInputSet(i);
                        }
                        else
                        {   
                            _steppables[j].steppable.GetComponent<InputSender>().ApplyArchivedInputSet(i);
                        }
                    }

                    _steppables[j].steppable.ControlledStep();
                    
                    lastOrder = _steppables[j].id;
                }

                if (i != snapshotAge)
                    P2PHandler.Instance.IncrementFrameCounter();
            }
        }

        public void SaveGameState(int frame, bool force)
        {
            if (force)
                InternalSaveGameState(frame);
            else
                StartCoroutine(ScheduleSaveGameState(frame));
        }

        private IEnumerator ScheduleSaveGameState(int frame)
        {
            yield return new WaitForFixedUpdate();
            InternalSaveGameState(frame);
        }
        
        private void InternalSaveGameState(int frame)
        {
             Debug.Log($"[SaveGameState] on: ({P2PHandler.Instance.DataPacket.FrameCounter}), from: {frame}");
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