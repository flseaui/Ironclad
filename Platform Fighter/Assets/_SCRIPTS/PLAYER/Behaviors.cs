using UnityEngine;
using static DATA.Types;

namespace PLAYER
{
    [RequireComponent(typeof(PlayerData))]
    public abstract class Behaviors : MonoBehaviour
    {
        protected PlayerData Data { get; set; }
        
        private void Start()
        {
            Data = GetComponent<PlayerData>();

            PlayerScript.OnActionEnd += RunAction;
        }

        protected abstract void RunAction();

    }
}
