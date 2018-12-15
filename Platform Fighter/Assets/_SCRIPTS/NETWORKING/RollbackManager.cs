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
            public Type BaseType;
            public Type Type;
            public string JsonData;
            public int Frame;
            
            public Snapshot(int player, Type baseType, Type type, string json, int frame)
            {
                Player = player;
                BaseType = baseType;
                Type = type;
                JsonData = json;
                frame = frame;
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
            
            foreach (var snapshot in _snapshots[distance])
            {
                var packet = JsonUtility.FromJson(snapshot.JsonData, snapshot.Type);
                P2PHandler.Instance.FramesLapsed = snapshot.Frame;
                ((ISettable) MatchStateManager.Instance.GetPlayer(snapshot.Player).GetComponent(snapshot.BaseType)).SetData(packet);
                Debug.Log("ROLLBACK PART 2");
            }
        }

        public void SaveGameState()
        {
            _snapshots.Clear();
            _snapshots.Add(new List<Snapshot>());
            foreach (var player in MatchStateManager.Instance.GetPlayers())
            {
                TakeSnapshot(player.GetComponent<NetworkIdentity>().Id, 0, typeof(PlayerData), player.GetComponent<PlayerData>().DataPacket, P2PHandler.Instance.FramesLapsed);
            }
        }

        public void TakeSnapshot<T>(int player, int depth, Type baseType, T structure, int frame)
        {
            var json = JsonUtility.ToJson(structure);
            _snapshots[depth].Add(new Snapshot(player, baseType, structure.GetType(), json, frame));
        }        
    }
}