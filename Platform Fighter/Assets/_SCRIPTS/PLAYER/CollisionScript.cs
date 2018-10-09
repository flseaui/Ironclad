using DATA;
using UnityEngine;

namespace PLAYER 
{
    public class CollisionScript : MonoBehaviour 
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Box")) return;
            
            var boxData = other.GetComponent<BoxData>();
            var playerBoxType = GetComponent<BoxData>().Type;
            var oppenantBoxType = boxData.Type;
            
            if (playerBoxType == ActionInfo.Box.BoxType.Hurt && oppenantBoxType == ActionInfo.Box.BoxType.Hit)
            {
                //Needs to determine direction

                GetComponentInParent<DamageScript>().ApplyDamage(boxData.Damage, boxData.KnockbackStrength, boxData.KnockbackAngle);
            }
        }
    }
}
