using UnityEngine;

namespace PLAYER
{
    [RequireComponent(typeof(PlayerData))]
    public abstract class Behaviors : MonoBehaviour
    {
        protected PlayerDataPacket Data { get; private set; }

        protected PlayerController PlayerController { get; private set; }

        private void Awake()
        {
            Data = GetComponent<PlayerData>().DataPacket;
            PlayerController = GetComponent<PlayerController>();
        }

        private void FixedUpdate()
        {
            RunAction();
        }

        public abstract void RunAction();
    }
}