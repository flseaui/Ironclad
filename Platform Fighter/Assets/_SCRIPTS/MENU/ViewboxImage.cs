using MANAGERS;
using UnityEngine;
using UnityEngine.UI;
using Types = DATA.Types;

namespace MENU
{
    public class ViewboxImage : MonoBehaviour
    {
        private Image _image;

        [SerializeField] private Sprite _noCharacterSprite;
        [SerializeField] private Sprite _testCharacterSprite;
        public int Id;

        private void Awake()
        {
            _image = transform.GetChild(0).GetComponent<Image>();
        }

        private void Update()
        {
            
        }
    }
}