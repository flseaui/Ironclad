using TMPro;
using UnityEngine;

namespace PLAYER
{
    public class PlayerHUD : MonoBehaviour
    {
        public TextMeshProUGUI NameText;

        public void SetupGameObjects()
        {
            NameText = GameObject.Find("NameText").GetComponent<TextMeshProUGUI>();
        }

        private void Update() { }
    }
}