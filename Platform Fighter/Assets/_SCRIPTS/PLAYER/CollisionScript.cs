using DATA;
using UnityEngine;

namespace PLAYER
{
    public class CollisionScript : MonoBehaviour
    {
        private BoxData _boxData;

        private PlayerData _playerData;

        private void Awake()
        {
            _playerData = GetComponentInParent<PlayerData>();
            _boxData = GetComponentInParent<BoxData>();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Box"))
            {
                var boxData = other.GetComponent<BoxData>();
                var playerBoxType = _boxData.Type;
                var opponentBoxType = boxData.Type;

                if (playerBoxType == ActionInfo.Box.BoxType.Hurt && opponentBoxType == ActionInfo.Box.BoxType.Hit)
                    GetComponentInParent<DamageScript>()
                        .ApplyDamage(boxData.Damage, boxData.KnockbackStrength, boxData.KnockbackAngle);
                //} else if (other.CompareTag("Ledge")) {
            }
            else if (other.CompareTag("Stage"))
            {
                if (GetComponent<BoxCollider2D>().bounds.center.y - GetComponent<BoxCollider2D>().bounds.size.y / 2 >=
                    other.transform.position.y)
                {
                    _playerData.DataPacket.RelativeLocation = PlayerDataPacket.PlayerLocation.Grounded;

                    _playerData.DataPacket.ArialActions =
                        _playerData.DataPacket.ArialActionsMax;
                }
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Stage"))
            {
                Debug.Log("FUCK MY ASSHOLE");

                _playerData.DataPacket.RelativeLocation = PlayerDataPacket.PlayerLocation.Airborne;
            }
        }
    }
}