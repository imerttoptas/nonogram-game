// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEditor;
// using System;
//
// public class LevelEditor : EditorWindow
// {
//     Texture2D map;
//     int levelIndex;
//     int gridSize;
//     CustomLevelData cellData;
//     [MenuItem("LevelEditor/LevelEditor")]
//     public static void ShowWindow()
//     {
//         GetWindow<LevelEditor>();
//     }
//     private void OnGUI()
//     {
//         levelIndex = EditorGUILayout.IntField("Level :", levelIndex);
//         gridSize = EditorGUILayout.IntField("Grid Size :", gridSize);
//         GUILayout.Space(40f);
//         for (int i = 0; i < gridSize; i++)
//         {
//             GUILayout.BeginHorizontal();
//             GUILayout.FlexibleSpace();
//             for (int j = 0; j < gridSize; j++)
//             {
//
//                 /*if (GUILayout.Button(cellData[(i*j)+j].isTouched ?  "X" : ""))
//                 {
//                     Debug.Log("Cell " + ((i * j) + j).ToString());
//                 }
//                 */
//             }
//             GUILayout.EndHorizontal();
//         }
//         if (GUILayout.Button("Generate Custom Map"))
//         {
//             Debug.Log("custom level " + levelIndex + "  generated");
//         }
//     }
//     void init()
//     {
//         for (int i = 0; i < gridSize * gridSize; i++)
//         {
//             //cellData[i].isTouched = false;
//         }
//     }
// }
//
//
