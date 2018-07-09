using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public int playerCount = 1;

    // Prefabs
    [SerializeField]
    private GameObject playerPrefab;

    public Transform[] spawnPoints;

    private void Start()
    {
		//AssetManager.Instance.PopulateActions(new Types.Character[] { Types.Character.TEST_CHARACTER });
		//AssetManager.Instance.GetAction(Types.Character.TEST_CHARACTER, Types.ActionType.JAB);
		
        //for (int i = 0; i < playerCount; ++i)
        //{
            //Instantiate(playerPrefab, spawnPoints[i].position, spawnPoints[i].rotation);
        //}
    }

    private void Update()
    {

    }
}