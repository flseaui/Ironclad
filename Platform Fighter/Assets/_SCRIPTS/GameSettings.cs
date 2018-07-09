using UnityEngine;

public class GameSettings : Singleton<GameSettings>
{
    [SerializeField]
    private float playerInitialHealth;

    [Header("Input")]
    public float runThreshold;
    public float crouchThreshold;
    public float upThreshold;
}