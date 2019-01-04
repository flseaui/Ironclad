using MANAGERS;
using UnityEngine;
using UnityEngine.UI;
using Types = DATA.Types;

namespace MENU
{
    public class CharacterButton : MonoBehaviour
    {
        [SerializeField] private Types.Character _character;
        [SerializeField] private Sprite _highlightedSprite;
        private Image _image;

        [SerializeField] private Sprite _neutralSprite;
        [SerializeField] private Sprite _selectedSprite;
        private RectTransform _transform;

        private void Awake()
        {
            _transform = GetComponent<RectTransform>();
            _image = GetComponent<Image>();
        }

        private void Update()
        {
            foreach (var token in GameObject.FindGameObjectsWithTag("Token"))
                if (RectTransformUtility.RectangleContainsScreenPoint(_transform, token.transform.position))
                {
                    var movement = token.GetComponent<TokenMovement>();
                    if (movement.Select && !movement.Dropped)
                    {
                        movement.Dropped = true;
                    }

                    if (movement.Dropped)
                        _image.overrideSprite = _selectedSprite;
                    else
                        _image.overrideSprite = _highlightedSprite;
                }
                else
                {
                    _image.overrideSprite = null;
                }
        }
    }
}