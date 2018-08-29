using System;
using UnityEngine;
using Types = DATA.Types;

namespace PLAYER
{
    [RequireComponent(typeof(PlayerData)), RequireComponent(typeof(Rigidbody2D))]
    public class PlayerMovement : MonoBehaviour
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
            MovePlayer();
        }

        private void MovePlayer()
        {
            if (Data.Direction == Types.Direction.Left)
                Rigidbody.AddForce(Vector2.left * Data.Acceleration, ForceMode2D.Impulse);
            else if (Data.Direction == Types.Direction.Right)
                Rigidbody.AddForce(Vector2.right * Data.Acceleration, ForceMode2D.Impulse);
        }
    }
}