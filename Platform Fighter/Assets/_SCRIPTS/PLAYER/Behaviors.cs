using UnityEngine;

namespace PLAYER
{
    [RequireComponent(typeof(PlayerData))]
    public abstract class Behaviors : MonoBehaviour
    {
        protected PlayerData Data { get; private set; }

        protected PlayerController PlayerController { get; private set; }
        
        private void Awake()
        {
            Data = GetComponent<PlayerData>();
            PlayerController = GetComponent<PlayerController>();
        }

        private void Update()
        {
            RunAction();
        }

        public abstract void RunAction();
    }
}