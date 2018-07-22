using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using MISC;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using Types = DATA.Types;

namespace Editor
{
    public static class MenuSwap
    {
        public static void SwapMenu(string menuName)
        {
            var previousMenu = GameObject.FindGameObjectsWithTag("MenuPanel").FirstOrDefault(x => x.activeSelf);
            if (previousMenu != null) previousMenu.SetActive(false);
            
            var menu = GameObject.Find(menuName).transform.FindObjectsWithTag("MenuPanel").FirstOrDefault();
            if (menu != null) menu.SetActive(true);

            EditorUtils.Collapse(menu, false);
            EditorUtils.SelectObject(menu);
        }

        public static bool SwapMenuValidation(string menuName)
        {
            if (!SceneManager.GetActiveScene().name.Equals("_MENU")) return false;
            
            var first = GameObject.Find(menuName).transform.FindObjectsWithTag("MenuPanel").FirstOrDefault();

            return first != null
                   && !Application.isPlaying
                   && !first.activeSelf;
        }
        
        /// <summary>
        ///     Generates a script containing MenuItems for each <code>Types.Menu</code>
        /// </summary>
        [MenuItem("Assets/PFighter/Generate Menu Items")]
        private static void GenerateMenuItems()
        {
            // The generated filepath
            var scriptPath = Application.dataPath + "/Editor/Generated/GMenuSwapMenuItems.cs";

            // List of elements in Types.Menu excluding BlankMenu
            var menuNames = Enum.GetNames(typeof(Types.Menu)).Where(name => !name.Equals("BlankMenu"));

            // Boilerplate
            var sb = new StringBuilder();
            sb.AppendLine("/** This class is Auto-Generated **/");
            sb.AppendLine("");
            sb.AppendLine("using UnityEditor;");
            sb.AppendLine("");
            sb.AppendLine("namespace Editor.Generated");
            sb.AppendLine("{");
            sb.AppendLine("    public static class GMenuSwapMenuItems {");
            sb.AppendLine("");

            // Generate MenuItems
            foreach (var menuName in menuNames)
            {
                sb.AppendLine($"    [MenuItem(\"Menu/{ menuName }\")]");
                sb.AppendLine($"    private static void SwapTo{ menuName }() => MenuSwap.SwapMenu(\"{ menuName }\");");
                sb.AppendLine($"    [MenuItem(\"Menu/{ menuName }\", true)]");
                sb.AppendLine($"    private static bool SwapTo{ menuName }Validation() => MenuSwap.SwapMenuValidation(\"{ menuName }\");");
                sb.AppendLine("");
            }

            sb.AppendLine("");
            sb.AppendLine("    }");
            sb.AppendLine("}");

            // Writes the class and imports it so it is visible in the Project window
            if (!Directory.Exists(Application.dataPath + "/Editor/Generated/"))
                Directory.CreateDirectory(Application.dataPath + "/Editor/Generated/");
            if (File.Exists(scriptPath))
                File.Delete(scriptPath);
            File.WriteAllText(scriptPath, sb.ToString(), Encoding.UTF8);
            AssetDatabase.ImportAsset("Assets/Editor/Generated/GMenuSwapMenuItems.cs");
        }
    }
}