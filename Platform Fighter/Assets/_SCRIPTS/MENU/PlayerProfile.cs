using UnityEngine;
using UnityEngine.UI;

namespace MENU
{
    public class PlayerProfile : MonoBehaviour
    {
        private Image _borderImage;
        private Text _nameText;

        private void Awake()
        {
            _borderImage = GetComponent<Image>();
            _nameText = GetComponentInChildren<Text>();
        }

        public void SetBorderColor(Color color)
        {
            _borderImage.color = color;
        }

        public void SetPlayerName(string playerName)
        {
            _nameText.text = playerName;
        }
    }
}