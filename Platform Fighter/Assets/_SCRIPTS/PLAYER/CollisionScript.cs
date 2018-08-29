using System.Collections;
using System.Collections.Generic;
using DATA;
using PLAYER;
using UnityEngine;

public class CollisionScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter2D(Collider2D col)
    {
        ActionInfo.BoxType PlayerBoxType = GetComponent<BoxData>().Type;
        ActionInfo.BoxType OppenantBoxType = col.GetComponent<BoxData>().Type;

        if (PlayerBoxType == ActionInfo.BoxType.Hurt && OppenantBoxType == ActionInfo.BoxType.Hit)
        {
            GetComponent<DamageScript>().ApplyDamage(col.GetComponent<BoxData>().Damage, col.GetComponent<BoxData>().KnockbackStrength, col.GetComponent<BoxData>().KnockbackAngle);
        }
    }
}
