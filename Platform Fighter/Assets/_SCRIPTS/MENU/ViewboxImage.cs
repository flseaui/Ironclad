using MANAGERS;
using UnityEngine;
using UnityEngine.UI;
using Types = DATA.Types;

namespace MENU
{
    public class ViewboxImage : MonoBehaviour
    {
        public int Id;

        private Image _image;

        [SerializeField] private Sprite _noCharacterSprite;
        [SerializeField] private Sprite _testCharacterSprite;

        private void Awake()
        {
            _image = transform.GetChild(0).GetComponent<Image>();
        }

        private void Update()
        {
            switch (MainMenuManager.Instance.GetCharacter(Id))
            {
                case Types.Character.TEST_CHARACTER:
                    _image.overrideSprite = _testCharacterSprite;
                    break;
                case Types.Character.NONE:
                    _image.overrideSprite = _noCharacterSprite;
                    break;
                default:
                    _image.overrideSprite = null;
                    break;
            }
        }
    }
}