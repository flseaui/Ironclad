using System;
using System.Collections;
using System.Collections.Generic;
using MISC;
using UnityEngine;
using Types = DATA.Types;

namespace MANAGERS
{
    public class TimeManager : Singleton<TimeManager>
    {
        public int FramesLapsed { get; private set; }

        public float GameTime { get; private set; }

        public bool UpdatePaused { get; private set; }
        public bool FixedUpdatePaused { get; private set; }

        private int _updatePauseTimer;
        private int _fixedUpdatePauseTimer;
        
        private void Update()
        {
            GameTime += Time.unscaledDeltaTime;
            FramesLapsed = ++FramesLapsed;
            
            if (UpdatePaused)
            {
                --_updatePauseTimer;
                if (_updatePauseTimer == 0)
                {
                    PauseToggle(Types.PauseType.Update, false);
                }
            }
        }

        private void FixedUpdate()
        {
            if (FixedUpdatePaused)
            {
                --_fixedUpdatePauseTimer;
                if (_fixedUpdatePauseTimer == 0)
                {
                    PauseToggle(Types.PauseType.FixedUpdate, false);
                }
            }
        }

        public void PauseForFrames(int frames, Types.PauseType pauseType)
        {
            if (frames <= 0) return;

            switch (pauseType)
            {
                case Types.PauseType.Update:
                    _updatePauseTimer = frames;
                    break;
                case Types.PauseType.FixedUpdate:
                    _fixedUpdatePauseTimer = frames;
                    break;
                case Types.PauseType.Both:
                    _updatePauseTimer = frames;
                    _fixedUpdatePauseTimer = frames;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(pauseType), pauseType, null);
            }

            PauseToggle(pauseType, true);
        }

        public void PauseToggle(Types.PauseType pauseType, bool paused)
        {
            switch (pauseType)
            {
                case Types.PauseType.Update:
                    UpdatePaused = true;
                    break;
                case Types.PauseType.FixedUpdate:
                    StartCoroutine(InternalFixedPauseToggle(paused));
                    break;
                case Types.PauseType.Both:
                    UpdatePaused = true;
                    StartCoroutine(InternalFixedPauseToggle(paused));
                    break;
            }
        }

        private IEnumerator InternalFixedPauseToggle(bool paused)
        {
            yield return new WaitForFixedUpdate();
            
            FixedUpdatePaused = paused;
        }


    }
}