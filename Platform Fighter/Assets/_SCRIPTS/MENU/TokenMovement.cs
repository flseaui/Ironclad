﻿using Rewired;
using UnityEngine;
using UnityEngine.UI;

namespace MENU
{
    [RequireComponent(typeof(RectTransform))]
    public class TokenMovement : MonoBehaviour
    {
        [SerializeField] private Sprite _droppedSprite;

        private Image _image;
        private Player _player;

        [SerializeField] private int _speed = 1000;

        private RectTransform _transform;
        public int Id;

        [HideInInspector] public bool Select, Back, Dropped;

        private void Awake()
        {
            _player = ReInput.players.GetPlayer(0);
            _transform = GetComponent<RectTransform>();
            _image = GetComponent<Image>();
        }

        private void Update()
        {
            UpdateInput();

            if (Dropped)
                _image.overrideSprite = _droppedSprite;
            else
                _image.overrideSprite = null;

            if (Back) Dropped = false;
        }

        private void UpdateInput()
        {
            Select = _player.GetButton("Select");

            Back = _player.GetButton("Back");

            if (Dropped) return;

            var anchoredPosition = _transform.anchoredPosition;
            _transform.anchoredPosition = Vector2.Lerp
            (
                anchoredPosition,
                new Vector2
                (
                    anchoredPosition.x + _player.GetAxis("Horizontal") * _speed * Time.deltaTime,
                    _transform.anchoredPosition.y + _player.GetAxis("Vertical") * _speed * Time.deltaTime
                ),
                _speed * Time.deltaTime
            );
        }
    }
}