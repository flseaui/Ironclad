using DATA;
using Rewired;
using UnityEngine;

namespace PLAYER
{
    public class CollisionScript : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Box"))
            {
                var boxData = other.GetComponent<BoxData>();
                var playerBoxType = GetComponent<BoxData>().Type;
                var oppenantBoxType = boxData.Type;

                if (playerBoxType == ActionInfo.Box.BoxType.Hurt && oppenantBoxType == ActionInfo.Box.BoxType.Hit)
                    GetComponentInParent<DamageScript>()
                        .ApplyDamage(boxData.Damage, boxData.KnockbackStrength, boxData.KnockbackAngle);
            //}else if (other.CompareTag("Ledge")){
                
            }else if (other.CompareTag("Stage"))
            {
                if (GetComponent<BoxCollider2D>().bounds.center.y - GetComponent<BoxCollider2D>().bounds.size.y / 2 >=
                    other.transform.position.y)
                {
                    GetComponent<PlayerData>().DataPacket.RelativeLocation = PlayerDataPacket.PlayerLocation.Grounded;

                    GetComponent<PlayerData>().DataPacket.ArialActions =
                        GetComponent<PlayerData>().DataPacket.ArialActionsMax;

                }
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Stage"))
            {
                Debug.Log("FUCK MY ASSHOLE");
                
                GetComponent<PlayerData>().DataPacket.RelativeLocation = PlayerDataPacket.PlayerLocation.Airborne;
            }
        }
    }
}