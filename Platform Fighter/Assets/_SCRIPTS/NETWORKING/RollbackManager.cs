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
        private List<List<Snapshot>> _snapshots;
        
        private List<(int, Steppable)> _steppables;

        public void AddSteppable(Steppable steppable, int stepOrder)
        {
            _steppables.Add((stepOrder, steppable));
            Debug.Log($"Added Steppable {steppable.name} with stepOrder {stepOrder}");
        }

        
        private void Awake()
        {
            _snapshots = new List<List<Snapshot>>();
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
        
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F1)) SaveGameState();

            if (Input.GetKeyDown(KeyCode.F2)) Rollback();
        }

        /// <summary>
        ///     Rollback the game state to a previous iteration
        /// </summary>
        public void Rollback()
        {
            Debug.Log("ROLLBACK PART 1");

            foreach (var snapshotPiece in _snapshots[0])
            {
                var packet = JsonUtility.FromJson(snapshotPiece.JsonData, snapshotPiece.Type);

                if (snapshotPiece.BaseType.IsSubclassOf(typeof(Singleton)))
                    P2PHandler.Instance.SetData(packet);
                else
                    ((ISettable) MatchStateManager.Instance.GetPlayer(snapshotPiece.Player)
                        .GetComponent(snapshotPiece.BaseType)).SetData(packet);

                Debug.Log("ROLLBACK PART 2");
            }

            //var snapshotAge = Mod(P2PHandler.Instance.InputPacketsSent - _age, 600) + 1;
            var snapshotAge = 0;
            foreach (var player in MatchStateManager.Instance.Players)
            {
                if (player.GetComponent<NetworkInput>())
                    snapshotAge = player.GetComponent<NetworkInput>().ArchivedInputSets.Count;
                else
                    snapshotAge = player.GetComponent<NetworkUserInput>().ArchivedInputSets.Count;
            }
            Debug.Log("SnapshotAge: " + snapshotAge);

            int lastOrder = _steppables[0].Item1;
            for (var i = 0; i < snapshotAge; ++i)
            {
                for (var j = 0; j < _steppables.Count; j++)
                {
                    _steppables[j].Item2.ControlledStep();
                    
                    if (_steppables[j].Item1 < lastOrder || _steppables[j].Item1 == 0)
                        _steppables[j].Item2.GetComponent<InputSender>().ApplyArchivedInputSet(i);
                    
                    lastOrder = _steppables[j].Item1;
                }
            }
        }

        public void SaveGameState()
        {
            _snapshots.Clear();
            _snapshots.Add(new List<Snapshot>());
            foreach (var player in MatchStateManager.Instance.Players)
                TakeSnapshot(player.GetComponent<NetworkIdentity>().Id, 0, typeof(PlayerData),
                    player.GetComponent<PlayerData>().DataPacket);
            TakeSnapshot(-1, 0, typeof(P2PHandler), P2PHandler.Instance.DataPacket);

            foreach (var player in MatchStateManager.Instance.Players)
            {
                player.GetComponent<InputSender>().ArchivedInputSets.Clear();
                player.GetComponent<NetworkUserInput>()?.ApplyLastInputSet();
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