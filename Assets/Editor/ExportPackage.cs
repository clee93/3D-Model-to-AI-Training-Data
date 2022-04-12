using UnityEngine;
using System.Collections;
using UnityEditor;

public static class ExportPackage
{


    [MenuItem("Export/Export with tags and layers, Input settings")]
    public static void export()
    {
        string[] projectContent = new string[] { "Assets/VirtualRenderStudio", "ProjectSettings/TagManager.asset", "ProjectSettings/InputManager.asset", "ProjectSettings/ProjectSettings.asset" };
        AssetDatabase.ExportPackage(projectContent, "VRS-2020-3-30-f1.unitypackage", ExportPackageOptions.Interactive | ExportPackageOptions.Recurse | ExportPackageOptions.IncludeDependencies);
        Debug.Log("Project Exported");
    }

}