using UnityEditor;
using System;

public static class BuildCommand
{
    public static void BuildWebGL()
    {
        string[] scenes = { "Assets/Scenes/BootstrapScene.unity", "Assets/Scenes/MainScene.unity" };
        BuildPipeline.BuildPlayer(scenes, "build/WebGL/WebGL", BuildTarget.WebGL, BuildOptions.None);
    }
}