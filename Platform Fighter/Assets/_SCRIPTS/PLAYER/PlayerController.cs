﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using DATA;
using MANAGERS;
using TOOLS;
using UnityEngine;
using Types = DATA.Types;

namespace PLAYER
{
    public delegate void OnActionEndCallback(int frameNumbber);

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

        private BoxPool _boxPool;

        [SerializeField] private GameObject _boxPrefab;
        
        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();

            _data = GetComponent<PlayerData>();

            _boxPool = new BoxPool();
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

            UpdateBoxes(_currentActionFrame);

            UpdateSprite();

            ++_currentActionFrame;
           
            // last frame of action
            if (_currentActionFrame >= _currentAction.FrameCount - 1)
            {
                OnActionEnd?.Invoke();
                _currentActionFrame = 0;
            }
        }

        private void PoolBoxes()
        {
            Action<IEnumerable<List<ActionInfo.Box>>, Types.ActionType> CreateBoxes =
                delegate(IEnumerable<List<ActionInfo.Box>> boxes, Types.ActionType actionType)
                {
                    var frameCount = 0;
                    foreach (var frame in boxes)
                    {
                        if (frame.Count > 0)
                        {
                            foreach (var hitbox in frame)
                            {
                                var box = Instantiate(_boxPrefab, transform);
                                box.transform.position = new Vector2(hitbox.X, hitbox.Y);
                                box.name = $"{hitbox.Type.ToString()}Box";

                                box.GetComponent<BoxCollider2D>().size = new Vector2(hitbox.Width, hitbox.Height);

                                var boxData = box.GetComponent<BoxData>();
                                boxData.SetData(hitbox, actionType, frameCount);

                                _boxPool.AddBox(boxData);
                            }
                        }
                        else
                        {
                            _boxPool.AddNullBox(actionType, frameCount).gameObject.transform.parent = transform;
                        }

                        ++frameCount;
                    }
                };
            
            var actionSet = AssetManager.Instance.GetActionSet(Types.Character.TestCharacter);
            foreach (var action in actionSet.Values)
            {
                CreateBoxes(action.Hitboxes, action.Type);
                CreateBoxes(action.Hurtboxes, action.Type);
                CreateBoxes(action.Grabboxes, action.Type);
                CreateBoxes(action.Armorboxes, action.Type);
                CreateBoxes(action.Collisionboxes, action.Type);
                CreateBoxes(action.Databoxes, action.Type);
            }
        }
        
        private void UpdateBoxes(int frame)
        {
            // TODO disable old boxes and enable new ones<w
            _boxPool.SwitchFrames(_currentAction.Type, frame);
        }

        private void UpdateSprite()
        {
            // TODO update player sprite with action manually to avoid using anims
            transform.localScale =
                _data.Direction == Types.Direction.Left ? new Vector3(-1, 1, 1) : new Vector3(1, 1, 1);
        }
    }
}