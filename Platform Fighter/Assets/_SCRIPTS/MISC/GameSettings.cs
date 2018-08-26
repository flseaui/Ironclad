using UnityEngine;

namespace MISC
{
    public class GameSettings : Singleton<GameSettings>
    {
        public float crouchThreshold;

        [SerializeField] private float playerInitialHealth;

        [Header("Input")] public float runThreshold;

        public float upThreshold;
    }
}