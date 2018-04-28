using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[UnitySingleton(UnitySingletonAttribute.Type.LoadedFromResources, false, "Managers/Level Manager")]
public class GameManager : MonoBehaviour {

    #region Singleton

    private static StageManager _instance;

    public static StageManager Instance
    {
        get { return _instance; }
    }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    #endregion

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
