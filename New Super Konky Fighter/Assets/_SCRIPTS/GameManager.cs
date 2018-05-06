using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private FighterDetails fighterPrefab;

    [SerializeField]
    private float startDelay = 3f; // The delay between the start of RoundStarting and RoundPlaying phases.

    [SerializeField]
    private float endDelay = 3f;   // The delay between the end of RoundPlaying and RoundEnding phases.

    private WaitForSeconds startWait;         // Used to have a delay whilst the round starts.
    private WaitForSeconds endWait;           // Used to have a delay whilst the round or game ends.

    private List<FighterDetails> fighters;

    public Transform[] spawnPoints;

    private void Start ()
    {

        startWait = new WaitForSeconds(startDelay);
        endWait = new WaitForSeconds(endDelay);

        SpawnFighters();

        StartCoroutine(GameLoop());
    }

    private void SpawnFighters()
    {
        var points = new List<Transform>(spawnPoints);

        fighters = new List<FighterDetails>();

        foreach (GameState.PlayerState state in GameState.Instance.players)
        {
            var spawnPointIndex = Random.Range(0, points.Count);

            var tank = Instantiate(fighterPrefab);
            tank.Setup(state, points[spawnPointIndex]);

            points.RemoveAt(spawnPointIndex);

            fighters.Add(tank);
        }
    }

    private void Update ()
    {
		
	}

    private IEnumerator GameLoop()
    {
        // Start off by running the 'RoundStarting' coroutine but don't return until it's finished.
        yield return StartCoroutine(RoundStarting());

        // Once the 'RoundStarting' coroutine is finished, run the 'RoundPlaying' coroutine but don't return until it's finished.
        yield return StartCoroutine(RoundPlaying());

        // Once execution has returned here, run the 'RoundEnding' coroutine, again don't return until it's finished.
        yield return StartCoroutine(RoundEnding());
    }

    private IEnumerator RoundStarting()
    {
        DisableFighterControl();

        yield return startWait;
    }


    private IEnumerator RoundPlaying()
    {
        EnableFighterControl();

        while (!GameSettings.Instance.ShouldFinishRound())
        {
            yield return null;
        }
    }

    private IEnumerator RoundEnding()
    {
        yield return endWait;
    }

    private void EnableFighterControl()
    {
        for (int i = 0; i < fighters.Count; i++)
        {
            if (fighters[i])
                fighters[i].enabled = true;
        }
    }


    private void DisableFighterControl()
    {
        for (int i = 0; i < fighters.Count; i++)
        {
            if (fighters[i])
                fighters[i].enabled = false;
        }
    }

}
