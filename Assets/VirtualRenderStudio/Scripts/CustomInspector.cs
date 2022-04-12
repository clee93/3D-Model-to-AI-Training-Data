using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
[CustomEditor(typeof(Render))]
public class CustomInspector : Editor
{
    public override void OnInspectorGUI()
    {
        //EditorGUILayout.LabelField("Test");
        DrawDefaultInspector();

        Render render = (Render)target;

        EditorGUILayout.Space(20);

        if (GUILayout.Button("Start Render"))
        {
            render.StartScreenshot();
        }
        EditorGUILayout.HelpBox("All Usable Training Images: " + String.Format("{0:n0}", render.CalculateTotalImages()) + "\n" +
            "Images Per Model: " + String.Format("{0:n0}", render.CalculateImagesPerModel()) + "\n" +
            "Maxium Possible Training Set Size: " + String.Format("{0:n0}", render.EstimateTotalSizeInMB()) + " MB", MessageType.Info);
    }
}