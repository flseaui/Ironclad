using System;
using System.Collections.Generic;
using DATA;
using MISC;

namespace MANAGERS
{
    public class TimeManager : Singleton<TimeManager>
    {
        public int FramesLapsed { get; private set; }

        public bool UpdatePaused { get; private set; }
        public bool FixedUpdatePaused { get; private set; }

        private int _updatePauseTimer;
        private int _fixedUpdatePauseTimer;
        
        private void Update()
        {
            FramesLapsed = ++FramesLapsed;
            
            if (UpdatePaused)
            {
                --_updatePauseTimer;
                if (_updatePauseTimer == 0)
                {
                    UnPause(Types.PauseType.Update);
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
                    UnPause(Types.PauseType.FixedUpdate);
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

            Pause(pauseType);
        }

        public void Pause(Types.PauseType pauseType)
        {
            switch (pauseType)
            {
                case Types.PauseType.Update:
                    UpdatePaused = true;
                    break;
                case Types.PauseType.FixedUpdate:
                    FixedUpdatePaused = true;
                    break;
                case Types.PauseType.Both:
                    UpdatePaused = true;
                    FixedUpdatePaused = true;
                    break;
            }
        }

        public void UnPause(Types.PauseType pauseType)
        {
            switch (pauseType)
            {
                case Types.PauseType.Update:
                    UpdatePaused = false;
                    break;
                case Types.PauseType.FixedUpdate:
                    FixedUpdatePaused = false;
                    break;
                case Types.PauseType.Both:
                    UpdatePaused = false;
                    FixedUpdatePaused = false;
                    break;
            }
        }

    }
}