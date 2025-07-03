using UnityEditor;
using UnityEngine;
using System.IO;

public static class SpriteToPNG
{
    [MenuItem("Assets/Save Sprite As PNG", true)]
    static bool Validate() => Selection.activeObject is Sprite;

    [MenuItem("Assets/Save Sprite As PNG")]
    static void Save()
    {
        var sprite = (Sprite)Selection.activeObject;

        // Ask where to save
        string path = EditorUtility.SaveFilePanelInProject(
            "Save PNG",
            sprite.name + ".png",
            "png",
            "Choose a location");
        if (string.IsNullOrEmpty(path)) return;

        // Ensure the atlas is temporarily readable
        var atlasPath = AssetDatabase.GetAssetPath(sprite.texture);
        var importer = (TextureImporter)TextureImporter.GetAtPath(atlasPath);
        bool prevReadable = importer.isReadable;
        if (!prevReadable)
        {
            importer.isReadable = true;
            importer.SaveAndReimport();
        }

        // Copy pixels from sprite rect
        var tex = new Texture2D(
            (int)sprite.rect.width, (int)sprite.rect.height,
            TextureFormat.RGBA32, false);
        tex.SetPixels(sprite.texture.GetPixels(
            (int)sprite.rect.x, (int)sprite.rect.y,
            (int)sprite.rect.width, (int)sprite.rect.height));
        tex.Apply();

        // Write PNG
        File.WriteAllBytes(path, tex.EncodeToPNG());
        AssetDatabase.Refresh();

        // Auto-import as Default texture so Terrain can see it
        var newImporter = (TextureImporter)TextureImporter.GetAtPath(path);
        newImporter.textureType = TextureImporterType.Default;
        newImporter.isReadable = false;
        newImporter.SaveAndReimport();

        // Restore original readable state on atlas
        if (!prevReadable)
        {
            importer.isReadable = false;
            importer.SaveAndReimport();
        }
    }
}
