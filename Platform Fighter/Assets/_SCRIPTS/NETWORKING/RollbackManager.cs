using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using ATTRIBUTES;
using Boo.Lang.Environments;
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
            public Type Type;
            public string JsonData;

            public Snapshot(int player, Type type, string json)
            {
                Player = player;
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
            if (Input.GetKey(KeyCode.F1))
            {
                SaveGameState();
            }

            if (Input.GetKey(KeyCode.F2))
            {
                Rollback(1);
            }
            
        }
        
        /// <summary>
        /// Rollback the game state to a previous iteration 
        /// </summary>
        /// <param name="distance"> Number of snapshots to rollback. </param>
        public void Rollback(int distance)
        {
            if (distance <= 0 || distance > _snapshots.Count)
            {
                return;
            }

            foreach (var snapshot in _snapshots[distance])
            {
                var component = JsonUtility.FromJson(snapshot.JsonData, snapshot.Type);
                ((ISettable) MatchStateManager.Instance.GetPlayer(snapshot.Player).GetComponent(snapshot.Type)).SetData(component);
            }
            
        }

        public void SaveGameState()
        {
            foreach (var player in MatchStateManager.Instance.GetPlayers())
            {
                TakeSnapshot(player.GetComponent<NetworkIdentity>().Id, _snapshots.Count, player.GetComponent<PlayerData>());
            }
        }
        
        public void TakeSnapshot<T>(int player, int depth, T structure)
        {
            var json = JsonUtility.ToJson(structure);
            _snapshots[depth].Add(new Snapshot(player, structure.GetType(), json));
        }
        
    }
}