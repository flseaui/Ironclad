using UnityEngine;

namespace PLAYER
{
    [RequireComponent(typeof(PlayerData))]
    public abstract class Behaviors : MonoBehaviour
    {
        protected PlayerData Data { get; set; }

        private void Awake()
        {
            Data = GetComponent<PlayerData>();
        }

        private void Start()
        {
            PlayerScript.OnActionEnd += RunAction;
        }

        protected abstract void RunAction();
    }
}