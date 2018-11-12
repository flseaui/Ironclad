using System;
using System.Collections.Generic;
using DATA;
using UnityEngine;
using Types = DATA.Types;

namespace PLAYER
{
    public class BoxPool
    {
        // List of actions of frames of boxes
        private readonly List<List<List<BoxData>>> _boxes;

        public BoxPool()
        {
            _boxes = new List<List<List<BoxData>>>(Enum.GetNames(typeof(Types.ActionType)).Length);
            for (var i = 0; i < Enum.GetNames(typeof(Types.ActionType)).Length; ++i)
                _boxes.Add(new List<List<BoxData>>());
        }

        public void AddBox(BoxData boxData)
        {
            if (_boxes[(int) boxData.ParentAction].Count <= boxData.ParentFrame)
                _boxes[(int) boxData.ParentAction].Add(new List<BoxData>());

            _boxes[(int) boxData.ParentAction][boxData.ParentFrame].Add(boxData);
        }

        public BoxData AddNullBox(Types.ActionType action, int frame)
        {
            var nullBox = new GameObject();
            nullBox.SetActive(false);
            nullBox.name = "NullBox";
            nullBox.AddComponent<BoxInfo>();
            nullBox.AddComponent<BoxData>();
            nullBox.GetComponent<BoxData>().SetAsNull();

            if (_boxes[(int) action].Count <= frame)
                _boxes[(int) action].Add(new List<BoxData>());

            _boxes[(int) action][frame].Add(nullBox.GetComponent<BoxData>());
            return nullBox.GetComponent<BoxData>();
        }

        public void SwitchFrames(Types.ActionType action, int frame)
        {
            DisableEnabledBoxes();
            EnableBoxesOnFrame(action, frame);
        }

        private void DisableEnabledBoxes()
        {
            foreach (var box in GameObject.FindGameObjectsWithTag("EnabledBox"))
            {
                var info = box.GetComponent<BoxInfo>();
                if (info.Lifespan > 0)
                {
                    info.Lifespan--;
                }
                if (info.Lifespan <= 0)
                {
                    box.tag = "DisabledBox";
                    box.SetActive(false);
                    //_enabledBoxes.Remove(box);
                }
            }
        }

        private void EnableBoxesOnFrame(Types.ActionType action, int frame)
        {
            //Debug.Log($"count1: {_boxes.Count}, index1: {(int) action}");
            //Debug.Log($"count2: {_boxes[(int) action].Count}, index2: {frame}");
            //if (frame > _boxes[(int) action].Count) return;
            foreach (var box in _boxes[(int) action][frame])
            {
                if (box.Type == ActionInfo.Box.BoxType.Null) continue;

                if (box.gameObject.activeSelf) continue;

                box.gameObject.SetActive(true);
                box.gameObject.tag = "EnabledBox";
                box.GetComponent<BoxInfo>().Lifespan = box.GetComponent<BoxData>().Lifespan;
            }
        }
    }
}