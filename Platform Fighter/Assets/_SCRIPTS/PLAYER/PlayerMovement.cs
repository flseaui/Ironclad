using UnityEngine;
using Types = DATA.Types;

namespace PLAYER
{
    [
        RequireComponent(typeof(PlayerData)),
        RequireComponent(typeof(Rigidbody2D))
    ]
    public class PlayerMovement : MonoBehaviour
    {
        private PlayerData Data { get; set; }

        private Rigidbody2D Rigidbody { get; set; }

        private void Start()
        {
            Data = GetComponent<PlayerData>();
            Rigidbody = GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate()
        {
            if (Data.Direction == Types.Direction.LEFT)
            {
                Rigidbody.AddForce(-Data.Acceleration, ForceMode2D.Impulse);
            }
            else if (Data.Direction == Types.Direction.RIGHT)
            {
                Rigidbody.AddForce(Data.Acceleration, ForceMode2D.Impulse);
            }
        }
    }
}