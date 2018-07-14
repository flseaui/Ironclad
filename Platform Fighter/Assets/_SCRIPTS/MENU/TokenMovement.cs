using MISC;
using Rewired;
using UnityEngine;

namespace MENU
{
    [RequireComponent(typeof(RectTransform))]
    public class TokenMovement : MonoBehaviour
    {
        private bool _select, _back;

        private RectTransform _transform;

        private Player _player;

        [SerializeField]
        private int _speed = 1000;

        private void Start()
        {
            _player = ReInput.players.GetPlayer(0);
            _transform = GetComponent<RectTransform>();
        }

        private void Update()
        {
            UpdateInput();
        }

        private void UpdateInput()
        {
            _transform.anchoredPosition = Vector2.Lerp
            (
                _transform.anchoredPosition,
                new Vector2
                (
                    _transform.anchoredPosition.x + _player.GetAxis("Horizontal") * _speed * Time.deltaTime,
                    _transform.anchoredPosition.y + _player.GetAxis("Vertical") * _speed * Time.deltaTime
                ),
                _speed * Time.deltaTime
            );


            if (_player.GetButtonDown("Select"))
                _select = true;

            if (_player.GetButtonDown("Back"))
                _back = true;
        }
    }
}