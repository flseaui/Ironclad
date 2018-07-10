using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHUD : MonoBehaviour
{

    public TextMeshProUGUI NameText;

    public void SetupGameObjects()
    {
        NameText = GameObject.Find("NameText").GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {

    }
}