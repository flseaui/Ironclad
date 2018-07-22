using System;
using System.IO;
using System.Linq;
using System.Text;
using MISC;
using UnityEditor;
using UnityEngine;
using Types = DATA.Types;

namespace Editor
{
    public static class MenuSwap
    {
        public static void SwapMenu(string menuName)
        {
            GameObject.FindGameObjectsWithTag("MenuPanel").FirstOrDefault(x => x.activeSelf)?.SetActive(false);
            GameObject.Find(menuName).transform.FindObjectsWithTag("MenuPanel").FirstOrDefault()?.SetActive(true);
        }

        /// <summary>
        ///     Generates a list of menuitems from an array
        /// </summary>
        [MenuItem("Assets/PFighter/Generate Menu Items")]
        private static void GenerateMenuItems()
        {
            // the generated filepath
            var scriptFile = Application.dataPath + "/Editor/Generated/GMenuSwapMenuItems.cs";

            // an example string array used to generate the items
            string[] ar = Enum.GetNames(typeof(Types.Menu));

            // The class string
            var sb = new StringBuilder();
            sb.AppendLine("/** This class is Auto-Generated **/");
            sb.AppendLine("");
            sb.AppendLine("using UnityEditor;");
            sb.AppendLine("");
            sb.AppendLine("namespace Editor.Generated");
            sb.AppendLine("{");
            sb.AppendLine("    public static class GMenuSwapMenuItems {");
            sb.AppendLine("");

            // loops though the array and generates the menu items
            for (var i = 0; i < ar.Length; i++)
            {
                sb.AppendLine($"    [MenuItem(\"Menu/{ ar[i] }\")]");
                sb.AppendLine($"    private static void SwapTo{ ar[i] }() => MenuSwap.SwapMenu(\"{ ar[i] }\");");
                sb.AppendLine("");
            }

            sb.AppendLine("");
            sb.AppendLine("    }");
            sb.AppendLine("}");

            // writes the class and imports it so it is visible in the Project window
            if (!Directory.Exists(Application.dataPath + "/Editor/Generated/"))
                Directory.CreateDirectory(Application.dataPath + "/Editor/Generated/");
            if (File.Exists(scriptFile))
                File.Delete(scriptFile);
            File.WriteAllText(scriptFile, sb.ToString(), Encoding.UTF8);
            AssetDatabase.ImportAsset("Assets/Editor/Generated/GMenuSwapMenuItems.cs");
        }
    }
}