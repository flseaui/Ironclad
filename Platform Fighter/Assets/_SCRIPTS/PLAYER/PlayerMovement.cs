using UnityEngine;
using UnityEngine.Networking;
using Types = DATA.Types;

namespace PLAYER
{
    [RequireComponent(typeof(PlayerData))]
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerMovement : NetworkBehaviour
    {
        private PlayerData Data { get; set; }

        private Rigidbody2D Rigidbody { get; set; }

        private void Awake()
        {
            Data = GetComponent<PlayerData>();
            Rigidbody = GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate()
        {
            CmdMovePlayer();
        }

        [Command]
        private void CmdMovePlayer()
        {
            if (Data.Direction == Types.Direction.LEFT)
                Rigidbody.AddForce(Vector2.left * Data.Acceleration, ForceMode2D.Impulse);
            else if (Data.Direction == Types.Direction.RIGHT)
                Rigidbody.AddForce(Vector2.right * Data.Acceleration, ForceMode2D.Impulse);
        }
    }
}