// using External;
// using Managers;
// using UnityEditor;
// using UnityEngine;
// using Game = GameData.GameData;
//
// namespace EditorHelper
// {
//      [CustomEditor(typeof(Building))]
//     public class BuildingEditor : Editor
//     {
//         private int _floors;
//         private int _length;
//         private int _width;
//         private Building parent;
//         public void OnEnable()
//         {
//             parent = (Building) target;
//             _floors = parent.Floors;
//             _length =(int) parent.Scale.x;
//             _width = (int) parent.Scale.y;
//         }
//
//         public override void OnInspectorGUI()
//         {
//             DrawDefaultInspector();
//
//             GUILayout.Space(30);
//             GUILayout.BeginVertical("Auto Editor", "window");
//
//             if (parent.transform.localScale != Game.TEMPLATE_SCALE)
//             {
//                 EditorGUILayout.HelpBox($"parent trnsform scale needed = {Game.TEMPLATE_SCALE.x}, {Game.TEMPLATE_SCALE.y}, {Game.TEMPLATE_SCALE.z},", MessageType.Error);
//             }
//             
//             if (parent.isHasRefs())
//             {
//                 EditorGUILayout.HelpBox($"Height: {parent.Floors}m\nWidth: {parent.Scale.x}m\nLength: {parent.Scale.y}", MessageType.None, true);
//
//                 _floors = EditorGUILayout.IntSlider("floors",_floors, 1, 150);
//
//                 _length = EditorGUILayout.IntSlider("length",_length, 1, 150);
//
//                 _width = EditorGUILayout.IntSlider("width",_width, 1, 150);
//                      parent.Floors = _floors;
//                      parent.Scale = new PointMap.Point(_length, _width);
//
//                 
//                     if (GUILayout.Button("BuildSize / 100",GUILayout.Height(40)))
//                 {
//                     _floors /= 100;
//                     _length /= 100;
//                     _width /= 100;
//                 }
//                 
//                 if (GUILayout.Button("BuildSize * 100",GUILayout.Height(40)))
//                 {
//                     _floors *= 100;
//                     _length *= 100;
//                     _width *= 100;
//                 }
//                 if(GUILayout.Button("Rotate 90", GUILayout.Height(40))){
//                     parent.Rotate90();
//                 }
//             }
//             else
//             {
//                 EditorGUILayout.HelpBox("Pls set GO to child", MessageType.Error);
//             }
//             
//             GUILayout.EndVertical();
//         }
//     }
// }
