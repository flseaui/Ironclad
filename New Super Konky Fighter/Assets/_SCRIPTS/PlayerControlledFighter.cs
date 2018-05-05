using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Fighters/Player Controlled")]
public class PlayerControlledFighter : FighterBase
{
    public int playerNumber;
    private string movementAxisName;

    public void OnEnable()
    {
        movementAxisName = "Horizontal" + playerNumber;
    }

    public override void Think(FighterDetails fighter)
    {
        var movement = fighter.GetComponent<FighterMovement>();

        movement.Horizontal(Input.GetAxis(movementAxisName));
    }

}
