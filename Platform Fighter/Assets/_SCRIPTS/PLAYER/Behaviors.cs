using MISC;
using UnityEngine;

namespace PLAYER
{
    [RequireComponent(typeof(PlayerData))]
    public abstract class Behaviors : MonoBehaviour, ISteppable
    {
        protected PlayerDataPacket Data { get; private set; }

        protected PlayerController PlayerController { get; private set; }

        public void Step()
        {
            RunAction();
        }

        private void Awake()
        {
            Data = GetComponent<PlayerData>().DataPacket;
            PlayerController = GetComponent<PlayerController>();
        }

        private void Update()
        {
            Step();
        }

        public abstract void RunAction();
    }
}