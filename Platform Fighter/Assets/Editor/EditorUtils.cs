using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    public static class EditorUtils
    {
        public static void CollapseSelection(bool value)
        {
            var selection = Selection.GetFiltered(typeof(GameObject), SelectionMode.TopLevel);
            foreach (var go in selection)
                Collapse(go, value);
        }

        public static void CollapseAllExcept(GameObject go, bool value = true)
        {
            // get the toplevel GOs (whose parents are null) - exclude 'go'
            var toplevelGos = Object.FindObjectsOfType<GameObject>().Where(g => g.transform.parent == null && g != go);
            foreach (var g in toplevelGos)
                Collapse(g, value);
            Collapse(go, false);
        }

        public static void Collapse(Object obj, bool collapse, string window = "General/Hierarchy") // by default, it's the "Hierarchy" window
        {
            // get a reference to the wanted window
            var hierarchy = GetFocusedWindow(window);
            // select our object
            SelectObject(obj);
            // create a new key event (RightArrow for collapsing, LeftArrow for folding)
            var key = new Event { keyCode = collapse ? KeyCode.RightArrow : KeyCode.LeftArrow, type = EventType.KeyDown };
            // finally, send the event to the window
            hierarchy.SendEvent(key);
        }


        public static void SelectObject(Object obj)
        {
            Selection.activeObject = obj;
        }

        public static EditorWindow GetFocusedWindow(string window)
        {
            FocusOnWindow(window);
            return EditorWindow.focusedWindow;
        }

        public static void FocusOnWindow(string window)
        {
            EditorApplication.ExecuteMenuItem("Window/" + window);
        }
    }
}