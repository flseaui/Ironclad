using DATA;
using PLAYER;
using UnityEngine;

public class CollisionScript : MonoBehaviour {
    
    private void OnTriggerEnter2D(Collider2D col)
    {
        ActionInfo.Box.BoxType PlayerBoxType = GetComponent<BoxData>().Type;
        ActionInfo.Box.BoxType OppenantBoxType = col.GetComponent<BoxData>().Type;

        if (PlayerBoxType == ActionInfo.Box.BoxType.Hurt && OppenantBoxType == ActionInfo.Box.BoxType.Hit)
        {
            //Needs to determin direction

            GetComponent<DamageScript>().ApplyDamage(col.GetComponent<BoxData>().Damage, col.GetComponent<BoxData>().KnockbackStrength, col.GetComponent<BoxData>().KnockbackAngle);
        }
    }
}
