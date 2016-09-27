﻿using UnityEditor;
using UnityEngine;
using System.Collections;

namespace Systems.EntitySystem.Editor
{
    public class EntityEditor : EditorWindow
    {
        [MenuItem("Window/Systems/Entity Database %#E")]
        static public void ShowWindow()
        {
            var window = GetWindow<EntityEditor>();
            window.minSize = new Vector2(800, 600);
            window.titleContent.text = "Entity System";
            window.Show();
        }

        private Vector2 scrollPosition;
        private int activeID;

        private GUIStyle _toggleButtonStyle;
        private GUIStyle ToggleButtonStyle
        {
            get
            {
                if (_toggleButtonStyle == null)
                {
                    _toggleButtonStyle = new GUIStyle(EditorStyles.toolbarButton);
                    ToggleButtonStyle.alignment = TextAnchor.MiddleLeft;
                }
                return _toggleButtonStyle;
            }
        }

        public void OnEnable()
        {

        }

        public void OnGUI()
        {
            scrollPosition = GUILayout.BeginScrollView(scrollPosition);

            for (int i = 0; i < EntityDatabase.GetAssetCount(); i++)
            {
                var asset = EntityDatabase.GetAt(i);
                if(asset != null)
                {
                    GUILayout.BeginHorizontal(EditorStyles.toolbar);
                    GUILayout.Label(string.Format("ID: {0}", asset.ID.ToString("D3")), GUILayout.Width(60));

                    bool clicked = GUILayout.Toggle(asset.ID == activeID, asset.Name, ToggleButtonStyle);

                    if(clicked != (asset.ID == activeID))
                    {
                        if (clicked)
                        {
                            activeID = asset.ID;
                            GUI.FocusControl(null);
                        }
                        else
                        {
                            activeID = -1;
                        }
                    }

                    if(GUILayout.Button("-", EditorStyles.toolbarButton, GUILayout.Width(30)) && EditorUtility.DisplayDialog("Delete Entity", "Are you sure you want to delete " + asset.Name + " Entity?", "Delete", "Cancel"))
                    {
                        EntityDatabase.Instance.RemoveAt(i);
                    }

                    GUILayout.EndHorizontal();

                    if(activeID == asset.ID)
                    {
                        EditorGUI.BeginChangeCheck();

                        //START OF SELECTED VIEW
                        GUILayout.BeginVertical("Box");  //a
                        GUILayout.BeginHorizontal();     //b
                        //SPRITE ON LEFT OF HORIZONTAL
                        GUILayout.BeginVertical(GUILayout.Width(75)); //c
                        GUILayout.Label("Entity Icon", GUILayout.Width(72));
                        asset.Icon = (Sprite)EditorGUILayout.ObjectField(asset.Icon, typeof(Sprite), false, GUILayout.Width(72), GUILayout.Height(72));
                        GUILayout.EndVertical();   //c

                        GUILayout.BeginVertical(); //d

                        GUILayout.BeginHorizontal();  //e
                        GUILayout.Label("Name", GUILayout.Width(80));
                        asset.Name = EditorGUILayout.TextField(asset.Name);
                        GUILayout.EndHorizontal();   //e
                        GUILayout.BeginHorizontal(); //f
                        GUILayout.Label("Description", GUILayout.Width(80));
                        asset.Description = EditorGUILayout.TextArea(asset.Description, GUILayout.MinHeight(50));
                        GUILayout.EndHorizontal(); //f

                        GUILayout.EndVertical();  //d

                        GUILayout.EndHorizontal();  //b
                        GUILayout.EndVertical();  //a

                        if (EditorGUI.EndChangeCheck())
                        {
                            EditorUtility.SetDirty(EntityDatabase.Instance);
                        }
                    }
                }
            }

            GUILayout.EndScrollView();

            GUILayout.BeginHorizontal();
            if(GUILayout.Button("New Entity", EditorStyles.toolbarButton))
            {
                var newAsset = new EntityAsset(EntityDatabase.Instance.GetNextId());
                EntityDatabase.Instance.Add(newAsset);
            }

            GUILayout.EndHorizontal();
        }
    }
}