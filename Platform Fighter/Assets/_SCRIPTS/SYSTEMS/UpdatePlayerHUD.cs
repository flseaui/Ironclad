using System;
using Unity.Entities;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace PlatformFighter
{
    [AlwaysUpdateSystem]
    public class UpdatePlayerHUD : ComponentSystem
    {

        public TextMeshProUGUI NameText;

        public void SetupGameObjects()
        {
            NameText = GameObject.Find("NameText").GetComponent<TextMeshProUGUI>();
        }

        protected override void OnUpdate()
        {

        }
    }
}