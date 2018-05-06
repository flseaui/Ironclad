using System;
using System.Collections.Generic;
using UnityEngine;

public class FighterDetails : MonoBehaviour
{
    [SerializeField]
    private FighterBase fighterBase;

    [NonSerialized]
    public GameState.PlayerState player;

    private void Awake()
    {
        enabled = false;
    }

    private void OnEnable()
    {
        if (!fighterBase)
        {
            enabled = false;
            return;
        }

        fighterBase.Initialize(this);
    }

    private void Update()
    {
        fighterBase.Think(this);
	}

    public void Setup(GameState.PlayerState playerState, Transform spawnPoint)
    {
        transform.position = spawnPoint.position;
        transform.rotation = spawnPoint.rotation;

        fighterBase = playerState.playerInfo.fighterBase; 

        player = playerState;
        playerState.fighter = this;

        enabled = true;
    }

}
