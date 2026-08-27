using System;
using System.IO;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

public static class RGBQuestWebGLBuild
{
    private const string OutputPath = "docs";
    private const string StagingPath = "Builds/RGBQuestWebGLStaging";

    private static readonly string[] ProductionScenes =
    {
        "Assets/Scenes/MainMenu.unity",
        "Assets/Scenes/Level1.unity",
        "Assets/Scenes/Level2.unity",
        "Assets/Scenes/Level3.unity",
        "Assets/Scenes/DeathMenu.unity",
        "Assets/Scenes/WinMenu.unity"
    };

    [MenuItem("RGBQuest/Validate Production Setup")]
    public static void ValidateProductionSetup()
    {
        ValidateProductionScenes();
        ValidateWebGLTemplate();

        if (EditorUserBuildSettings.activeBuildTarget != BuildTarget.WebGL)
        {
            Debug.LogWarning(
                "RGBQuest production files are valid, but WebGL is not " +
                "the active Build Profile. Switch to WebGL manually and " +
                "verify the game in Play Mode before building.");
            return;
        }

        Debug.Log(
            "RGBQuest production setup is valid and WebGL is active.");
    }

    [MenuItem("RGBQuest/Apply Safe WebGL Metadata")]
    public static void ApplySafeWebGLMetadata()
    {
        PlayerSettings.companyName = "MasterChiefProject";
        PlayerSettings.productName = "RGBQuest";
        PlayerSettings.bundleVersion = "1.0.0";

        PlayerSettings.WebGL.template = "PROJECT:RGBQuest";
        PlayerSettings.WebGL.compressionFormat =
            WebGLCompressionFormat.Gzip;
        PlayerSettings.WebGL.decompressionFallback = true;
        PlayerSettings.WebGL.dataCaching = true;

        Debug.Log(
            "RGBQuest WebGL metadata and static-hosting settings applied. " +
            "No quality or render-pipeline setting was changed.");
    }

    [MenuItem("RGBQuest/Build WebGL for GitHub Pages")]
    public static void BuildWebGLForGitHubPages()
    {
        ValidateProductionScenes();
        ValidateWebGLTemplate();

        if (EditorUserBuildSettings.activeBuildTarget != BuildTarget.WebGL)
        {
            throw new InvalidOperationException(
                "WebGL is not the active Build Profile. " +
                "Switch to WebGL manually in File > Build Profiles, " +
                "wait for the platform import to finish, verify MainMenu " +
                "and gameplay in Play Mode, then run this command again.");
        }

        ApplySafeWebGLMetadata();

        DeleteDirectoryIfPresent(StagingPath);
        Directory.CreateDirectory(StagingPath);

        try
        {
            Debug.Log(
                "[RGBQuest Build] Building six production scenes to staging...");

            BuildPlayerOptions options = new BuildPlayerOptions
            {
                scenes = ProductionScenes,
                locationPathName = StagingPath,
                target = BuildTarget.WebGL,
                options = BuildOptions.None
            };

            BuildReport report = BuildPipeline.BuildPlayer(options);

            if (report == null)
            {
                throw new InvalidOperationException(
                    "Unity returned no BuildReport.");
            }

            BuildSummary summary = report.summary;

            if (summary.result != BuildResult.Succeeded)
            {
                throw new InvalidOperationException(
                    $"RGBQuest WebGL build failed: {summary.result}. " +
                    $"Errors: {summary.totalErrors}, " +
                    $"warnings: {summary.totalWarnings}.");
            }

            Debug.Log(
                "[RGBQuest Build] Unity build succeeded. Publishing docs/...");

            ReplacePublishedBuild(StagingPath, OutputPath);

            File.WriteAllText(
                Path.Combine(OutputPath, ".nojekyll"),
                string.Empty);

            AssetDatabase.Refresh();

            double megabytes =
                summary.totalSize / (1024d * 1024d);

            Debug.Log(
                $"[RGBQuest Build] SUCCESS. Published to '{OutputPath}'. " +
                $"Unity build size: {megabytes:F1} MB.");
        }
        finally
        {
            DeleteDirectoryIfPresent(StagingPath);
        }
    }

    private static void ValidateProductionScenes()
    {
        foreach (string scene in ProductionScenes)
        {
            if (!File.Exists(scene))
            {
                throw new FileNotFoundException(
                    $"Required production scene was not found: {scene}",
                    scene);
            }
        }
    }

    private static void ValidateWebGLTemplate()
    {
        if (!AssetDatabase.IsValidFolder(
                "Assets/WebGLTemplates/RGBQuest"))
        {
            throw new DirectoryNotFoundException(
                "The RGBQuest WebGL template is missing at " +
                "'Assets/WebGLTemplates/RGBQuest'.");
        }
    }

    private static void ReplacePublishedBuild(
        string source,
        string destination)
    {
        if (!Directory.Exists(source))
        {
            throw new DirectoryNotFoundException(
                $"Successful staging build is missing: {source}");
        }

        DeleteDirectoryIfPresent(destination);
        Directory.CreateDirectory(destination);

        foreach (string directory in Directory.GetDirectories(
                     source,
                     "*",
                     SearchOption.AllDirectories))
        {
            string relative =
                Path.GetRelativePath(source, directory);

            Directory.CreateDirectory(
                Path.Combine(destination, relative));
        }

        foreach (string file in Directory.GetFiles(
                     source,
                     "*",
                     SearchOption.AllDirectories))
        {
            string relative =
                Path.GetRelativePath(source, file);

            string target =
                Path.Combine(destination, relative);

            string targetDirectory =
                Path.GetDirectoryName(target) ?? destination;

            Directory.CreateDirectory(targetDirectory);
            File.Copy(file, target, true);
        }
    }

    private static void DeleteDirectoryIfPresent(string path)
    {
        if (!Directory.Exists(path))
        {
            return;
        }

        Directory.Delete(path, true);
    }
}
