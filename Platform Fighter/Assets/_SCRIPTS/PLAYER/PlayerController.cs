using System.Collections.Generic;
using System.Linq;
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
    public class PlayerController : MonoBehaviour
    {
        public static event OnActionEndCallback OnActionEnd;
        public static event OnActionBeginCallback OnActionBegin;

        private int _currentActionFrame;
        
        private PlayerData _data;
        private SpriteRenderer _spriteRenderer;
        private ActionInfo _currentAction;

        private List<List<List<GameObject>>> _boxPool;
        
        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();

            _data = GetComponent<PlayerData>();

            _boxPool = new List<List<List<GameObject>>>();
            
        }

        private void Start()
        {
            _data.Direction = Types.Direction.Right;
            PlayerData.PlayerLocation Position = PlayerData.PlayerLocation.Grounded;
            
            PoolBoxes();
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

        private void PoolBoxes()
        {
            var actionSet = AssetManager.Instance.GetActionSet(Types.Character.TestCharacter);
            foreach (var action in actionSet.Values)
            {
                var frameList = new List<List<GameObject>>();
                foreach (var frame in action.Hitboxes.
                    Concat(action.Hurtboxes).
                    Concat(action.Grabboxes).
                    Concat(action.Armorboxes).
                    Concat(action.Collisionboxes).
                    Concat(action.Databoxes) 
                )
                {
                    var boxList = new List<GameObject>();
                    foreach (var hitbox in frame)
                    {
                        var box = Instantiate(new GameObject());
                        box.AddComponent<BoxCollider2D>();
                        box.GetComponent<BoxCollider2D>().size = new Vector2(hitbox.Width, hitbox.Height);
                        
                        box.AddComponent<BoxData>();
                        box.GetComponent<BoxData>().setData(hitbox);
                        
                        box.transform.position = new Vector2(hitbox.X, hitbox.Y);
                        box.tag = hitbox.Type + "box";
                        
                        boxList.Add(box);
                    }
                    frameList.Add(boxList);   
                }
                _boxPool.Add(frameList);
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