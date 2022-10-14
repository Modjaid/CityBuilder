using System;
using System.Collections;
using System.Collections.Generic;
using Managers;
using UnityEditor;
using UnityEngine;

namespace EditorHelper
{
    [CustomEditor(typeof(ExecuterAsset))]
    public class ExecuterAssetEditor : Editor
    {
        private ExecuterAsset obj;
        private int dirMoney = 36;

        public void OnEnable()
        {
            obj = (ExecuterAsset) target;
        }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

           dirMoney = EditorGUILayout.IntSlider("newDir Money", dirMoney, 1, 100);
           if (GUILayout.Button("Create New Director", GUILayout.Height(40f)))
           {
               var newDir = obj.CreateDirector();
               newDir.Money = dirMoney * 1000f;
           }
           
        }
    }

}
