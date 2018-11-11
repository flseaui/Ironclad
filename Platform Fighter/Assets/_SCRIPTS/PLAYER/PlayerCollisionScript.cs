using DATA;
using UnityEngine;

namespace PLAYER
{
    public class PlayerCollisionScript : MonoBehaviour
    {
        private PlayerData _playerData;

        private void Awake()
        {
            _playerData = GetComponentInParent<PlayerData>();
        }
        
        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.CompareTag("Stage"))
            {
                Debug.Log("landed");
                if (GetComponent<BoxCollider2D>().bounds.center.y - GetComponent<BoxCollider2D>().bounds.size.y / 2 >=
                    other.transform.position.y)
                {
                    _playerData.DataPacket.RelativeLocation = PlayerDataPacket.PlayerLocation.Grounded;

                    _playerData.DataPacket.ArialActions =
                        _playerData.DataPacket.ArialActionsMax;
                }
            }
        }
        
        private void OnCollisionExit2D(Collision2D other)
        {
            if (other.gameObject.CompareTag("Stage"))
            {
                Debug.Log("FUCK MY ASSHOLE");

                _playerData.DataPacket.RelativeLocation = PlayerDataPacket.PlayerLocation.Airborne;
            }
        }
    }
}