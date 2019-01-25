using System;
using System.Collections.Generic;
using MISC;

namespace MANAGERS
{
    public class TimeManager : Singleton<TimeManager>
    {
        [NonSerialized]
        public int FramesLapsed;
        
        private void Update()
        {         
            FramesLapsed = ++FramesLapsed % 600;
        }
    }
}