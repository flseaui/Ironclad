using System;
using DATA;
using MANAGERS;
using UnityEngine;
using Types = DATA.Types;
    
namespace PLAYER
{
    public delegate void OnActionEndCallback();

    [RequireComponent(typeof(PlayerData))]
    public class PlayerScript : MonoBehaviour
    {
        public static event OnActionEndCallback OnActionEnd;

        private PlayerData _data;

        private int _currentActionFrame;

        private ActionInfo _currentAction;

        private void Start ()
        {
            _data = GetComponent<PlayerData>();

            _data.Direction = Types.Direction.RIGHT;
            _data.Grounded = true;
        }

        private void Update()
        {
            Debug.Log($"{_data.CurrentAction} {_data.Direction}");

           ExecuteAction();
        }

        private void ExecuteAction()
        {
            if (_currentActionFrame == 0)
                _currentAction = AssetManager.GetAction(Types.Character.TEST_CHARACTER, _data.CurrentAction);

            ++_currentActionFrame;

            UpdateBoxes(_currentActionFrame);
            UpdateSprite(_currentActionFrame);

            if (_currentActionFrame >= _currentAction.FrameCount)
            {
                _currentActionFrame = 0;
                OnActionEnd?.Invoke();
            }
        }

        private void UpdateBoxes(int frame)
        {
            // TODO disable old boxes and enable new ones
        }

        private void UpdateSprite(int frame)
        {
            // TODO update player sprite with action manually to avoid using anims
        }

    }
}
