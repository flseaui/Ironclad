using System;
using System.Collections.Generic;
using DATA;
using MANAGERS;
using UnityEngine;
using Types = DATA.Types;
    
namespace PLAYER
{
    public delegate void OnActionEndCallback();
    public delegate void OnActionBeginCallback();

    [RequireComponent(typeof(PlayerData))]
    public class PlayerScript : MonoBehaviour
    {
        public static event OnActionEndCallback OnActionEnd;
        public static event OnActionBeginCallback OnActionBegin;

        private PlayerData _data;

        private SpriteRenderer _spriteRenderer;

        private ActionInfo _currentAction;

        private List<Sprite[]> _sprites;

        private int _currentActionFrame;


        private void Start ()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();

            _data = GetComponent<PlayerData>();
            _data.Direction = Types.Direction.RIGHT;
            _data.Grounded = true;

            _sprites = AssetManager.Instance.GetSprites(Types.Character.TEST_CHARACTER);
        }

        private void Update()
        {
            Debug.Log($"{_data.CurrentAction} {_data.Direction}");

            ExecuteAction();
        }

        private void ExecuteAction()
        {
            // first frame of action
            if (_currentActionFrame == 0)
            {
                _currentAction = AssetManager.Instance.GetAction(Types.Character.TEST_CHARACTER, _data.CurrentAction);
                OnActionBegin?.Invoke();
            }

            // last frame of action
            if (_currentActionFrame > 12)//_currentAction.FrameCount)
            {
                _currentActionFrame = 0;
                OnActionEnd?.Invoke();
            }

            ++_currentActionFrame;

            UpdateBoxes(_currentActionFrame);
            UpdateSprite(_currentActionFrame);
        }

        private void UpdateBoxes(int frame)
        {
            // TODO disable old boxes and enable new ones
        }

        private void UpdateSprite(int frame)
        {
            // TODO update player sprite with action manually to avoid using anims
            if (_sprites[(int) _currentAction.Type - 1][frame] != null)
                _spriteRenderer.sprite = _sprites[(int) _currentAction.Type - 1][frame];

            _spriteRenderer.flipX = _data.Direction == Types.Direction.LEFT;
        }
    }
}
