using System.Collections;
using System.Collections.Generic;
using TOOLS;
using PLAYER;
using UnityEngine;

public class DamageScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ApplyDamage(double Damage, double KnockbackStrength, double KnockbackAngle)
    {
        GetComponent<PlayerData>().Percent += Damage;

        //Find some way to determine direction

        GetComponent<PlayerData>().KnockbackVelocity.x = (float)((KnockbackStrength + KnockbackStrength * GetComponent<PlayerData>().Percent) * Mathf.Cos((float)KnockbackAngle)); //* positive or negitive for left or right
        GetComponent<PlayerData>().KnockbackVelocity.x = (float)((KnockbackStrength + KnockbackStrength * GetComponent<PlayerData>().Percent) * Mathf.Sin((float)KnockbackAngle));


        NLog.Log(NLog.LogType.Message, "DAMAGE BITCH");
    }
}
