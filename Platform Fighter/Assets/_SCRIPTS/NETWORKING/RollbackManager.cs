using System;
using System.Collections.Generic;
using MANAGERS;
using MISC;
using PLAYER;
using UnityEngine;
using static MISC.MathUtils;

namespace NETWORKING
{
    public class RollbackManager : Singleton<RollbackManager>
    {
        private int _age;
        private List<List<Snapshot>> _snapshots;

        private void Start()
        {
            _snapshots = new List<List<Snapshot>>();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F1)) SaveGameState();

            if (Input.GetKeyDown(KeyCode.F2)) Rollback(0);
        }

        /// <summary>
        ///     Rollback the game state to a previous iteration
        /// </summary>
        /// <param name="distance"> Number of snapshots to rollback. </param>
        public void Rollback(int distance)
        {
            Debug.Log("ROLLBACK PART 1");

            if (distance < 0 || distance > _snapshots.Count) return;

            foreach (var snapshotPiece in _snapshots[distance])
            {
                var packet = JsonUtility.FromJson(snapshotPiece.JsonData, snapshotPiece.Type);

                if (snapshotPiece.BaseType.IsSubclassOf(typeof(Singleton)))
                    P2PHandler.Instance.SetData(packet);
                else
                    ((ISettable) MatchStateManager.Instance.GetPlayer(snapshotPiece.Player)
                        .GetComponent(snapshotPiece.BaseType)).SetData(packet);

                Debug.Log("ROLLBACK PART 2");
            }

            var snapshotAge = Mod(P2PHandler.Instance.InputPacketsSent - _age, 600) + 1;
            Debug.Log("SnapshotAge: " + snapshotAge);
            for (var i = 0; i < snapshotAge; ++i)
                foreach (var player in MatchStateManager.Instance.Players)
                {
                    foreach (var steppable in player.GetComponents(typeof(Steppable))) ((Steppable) steppable).ControlledStep();
                    player.GetComponent<InputSender>().ApplyArchivedInputSet(i);
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