#if UNITY_EDITOR
using System.IO;
using JetBrains.Annotations;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace SceneNavigation
{
    [InitializeOnLoad]
    public static class SceneNavigationGUI
    {
        private static bool Redirect {
            get => EditorPrefs.GetBool("RedirectScene");
            set => EditorPrefs.SetBool("RedirectScene", value);
        }
        
        static SceneNavigationGUI()
        {
            ToolbarExtender.LeftToolbarGUI.Add(OnToolbarGUI);
        }

        private static void OnToolbarGUI()
        {
            GUILayout.FlexibleSpace();
            GUILayout.BeginHorizontal();

            foreach (EditorBuildSettingsScene scene in EditorBuildSettings.scenes)
            {
                string path = scene.path;
                string name = Path.GetFileNameWithoutExtension(path);
                
                if (GUILayout.Button(new GUIContent(name, $"Open {name} Scene")))
                {
                    // create the menu and add items to it
                    GenericMenu menu = new ();

                    menu.AddItem(new GUIContent("Single"), false, () => Open(OpenSceneMode.Single));
                    menu.AddItem(new GUIContent("Additive"), false, () => Open(OpenSceneMode.Additive));

                    // display the menu
                    menu.ShowAsContext();
                    
                    void Open(OpenSceneMode osm)
                    {
                        if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
                            return;

                        EditorSceneManager.OpenScene(path, osm);
                    }
                }
            }
            
            Redirect = GUILayout.Toggle(Redirect, new GUIContent("Redirect", "Redirect to the first scene in build on play"));
            SetPlayModeStartScene(Redirect ? EditorBuildSettings.scenes[0].path : null);
            
            GUILayout.EndHorizontal();
        }

        private static void SetPlayModeStartScene([CanBeNull]string scenePath)
        {
            SceneAsset startScene = string.IsNullOrEmpty(scenePath) ? null : AssetDatabase.LoadAssetAtPath<SceneAsset>(scenePath);
            EditorSceneManager.playModeStartScene = startScene;
        }
    }
}
#endif