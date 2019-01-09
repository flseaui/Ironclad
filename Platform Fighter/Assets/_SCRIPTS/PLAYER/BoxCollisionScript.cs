using DATA;
using UnityEngine;

namespace PLAYER
{
    public class BoxCollisionScript : MonoBehaviour
    {
        private BoxData _boxData;
        private DamageScript _damageScript;

        private void Awake()
        {
            _boxData = GetComponentInParent<BoxData>();
            _damageScript = GetComponentInParent<DamageScript>();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Box"))
            {
                var boxData = other.GetComponent<BoxData>();
                var playerBoxType = _boxData.Type;
                var opponentBoxType = boxData.Type;

                if (playerBoxType == ActionInfo.Box.BoxType.Hurt && opponentBoxType == ActionInfo.Box.BoxType.Hit)
                    _damageScript.ApplyDamage(boxData.Damage, boxData.KnockbackStrength, boxData.KnockbackAngle);
                //} else if (other.CompareTag("Ledge")) {
            }
        }
    }
}