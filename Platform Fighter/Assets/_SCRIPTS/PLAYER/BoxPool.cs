using System;
using System.Collections.Generic;
using UnityEngine;
using Types = DATA.Types;

namespace PLAYER
{
    public class BoxPool
    {
        // List of actions of frames of boxes
        private List<List<List<BoxData>>> _boxes;
        private List<BoxData> _enabledBoxes;

        public BoxPool()
        {
            _boxes = new List<List<List<BoxData>>>(Enum.GetNames(typeof(Types.ActionType)).Length);
            for (var i = 0; i < Enum.GetNames(typeof(Types.ActionType)).Length; ++i)
            {
                _boxes.Add(new List<List<BoxData>>());
            }
            
            _enabledBoxes = new List<BoxData>();
        }

        public void AddBox(BoxData boxData)
        {                      
            if (_boxes[(int) boxData.ParentAction].Count <= boxData.ParentFrame)
                _boxes[(int) boxData.ParentAction].Add(new List<BoxData>());
            
            _boxes[(int) boxData.ParentAction][boxData.ParentFrame - 1].Add(boxData);
        }

        public void SwitchFrames(Types.ActionType action, int frame)
        {
            DisableEnabledBoxes();
            EnableBoxesOnFrame(action, frame);
        }
        
        private void DisableEnabledBoxes()
        {
            foreach (var box in _enabledBoxes)
                box.gameObject.SetActive(false);
        }
        
        private void EnableBoxesOnFrame(Types.ActionType action, int frame)
        {
            //Debug.Log($"count1: {_boxes.Count}, index1: {(int) action}");
            //Debug.Log($"count2: {_boxes[(int) action].Count}, index2: {frame}");
            //if (frame > _boxes[(int) action].Count) return;
            foreach (var box in _boxes[(int) action][frame])
            {
                box.gameObject.SetActive(true);
                _enabledBoxes.Add(box);
            }
        }

    }
}