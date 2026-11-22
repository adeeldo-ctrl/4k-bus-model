#if UNITY_EDITOR
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

[InitializeOnLoad]
public static class KarachiProjectSetup
{
    const string ScenePath = "Assets/Scenes/Karachi4KBus.unity";

    static KarachiProjectSetup()
    {
        EditorApplication.delayCall += EnsureProjectReady;
    }

    static void EnsureProjectReady()
    {
        if (EditorApplication.isPlayingOrWillChangePlaymode) return;

        Directory.CreateDirectory("Assets/Scenes");
        if (!File.Exists(ScenePath))
        {
            var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
            var root = new GameObject("Karachi 4K Bus Simulator");
            root.AddComponent<Karachi4KBusBootstrap>();
            EditorSceneManager.SaveScene(scene, ScenePath);
        }

        var scenes = new System.Collections.Generic.List<EditorBuildSettingsScene>(EditorBuildSettings.scenes);
        bool found = false;
        foreach (var item in scenes)
            if (item.path == ScenePath) found = true;
        if (!found)
        {
            scenes.Add(new EditorBuildSettingsScene(ScenePath, true));
            EditorBuildSettings.scenes = scenes.ToArray();
        }

        PlayerSettings.companyName = "Adeel Games";
        PlayerSettings.productName = "Karachi 4K Bedford Bus Simulator";
        PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.Android, "com.adeeldo.karachi4kbus");
        PlayerSettings.defaultInterfaceOrientation = UIOrientation.LandscapeLeft;
    }
}
#endif
