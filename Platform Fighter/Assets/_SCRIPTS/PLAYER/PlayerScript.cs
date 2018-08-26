using DATA;
using MANAGERS;
using TOOLS;
using UnityEngine;
using Types = DATA.Types;

namespace PLAYER
{
    public delegate void OnActionEndCallback();

    public delegate void OnActionBeginCallback();

    [RequireComponent(typeof(PlayerData))]
    public class PlayerScript : MonoBehaviour
    {
        private ActionInfo _currentAction;

        private int _currentActionFrame;

        private PlayerData _data;

        private SpriteRenderer _spriteRenderer;
        public static event OnActionEndCallback OnActionEnd;
        public static event OnActionBeginCallback OnActionBegin;

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();

            _data = GetComponent<PlayerData>();
            _data.Direction = Types.Direction.Right;
            _data.Grounded = true;
        }

        private void Update()
        {
            NLog.Log(NLog.LogType.Message, $"{_data.CurrentAction} {_data.Direction}");

            ExecuteAction();
        }

        private void ExecuteAction()
        {
            // first frame of action
            if (_currentActionFrame == 0)
            {
                _currentAction = AssetManager.Instance.GetAction(Types.Character.TestCharacter, _data.CurrentAction);
                OnActionBegin?.Invoke();
            }

            ++_currentActionFrame;

            UpdateBoxes(_currentActionFrame);

            UpdateSprite();

            // last frame of action
            if (_currentActionFrame > _currentAction.FrameCount)
            {
                _currentActionFrame = 0;
                OnActionEnd?.Invoke();
            }
        }

        private void UpdateBoxes(int frame)
        {
            // TODO disable old boxes and enable new ones
        }

        private void UpdateSprite()
        {
            // TODO update player sprite with action manually to avoid using anims
            transform.localScale =
                _data.Direction == Types.Direction.Left ? new Vector3(-1, 1, 1) : new Vector3(1, 1, 1);
        }
    }
}