using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public static class WebGLVisualFallbacks
{
#if UNITY_WEBGL && !UNITY_EDITOR
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void RegisterSceneHook()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private static void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Text[] legacyTexts = Object.FindObjectsByType<Text>(
            FindObjectsInactive.Include,
            FindObjectsSortMode.None);

        foreach (Text legacyText in legacyTexts)
        {
            if (legacyText.gameObject.scene != scene)
            {
                continue;
            }

            if (legacyText.gameObject.name == "Health")
            {
                InstallHealthFallback(legacyText);
            }
            else if (legacyText.gameObject.name == "Crosshair")
            {
                InstallCrosshairFallback(legacyText);
            }
        }
    }

    private static void InstallHealthFallback(Text legacyText)
    {
        if (legacyText.transform.Find("WebGLHealthHearts") != null)
        {
            return;
        }

        WebGLHealthHeartsGraphic graphic =
            CreateGraphicChild<WebGLHealthHeartsGraphic>(
                legacyText,
                "WebGLHealthHearts");

        graphic.color = legacyText.color;
        legacyText.enabled = false;
    }

    private static void InstallCrosshairFallback(Text legacyText)
    {
        if (legacyText.transform.Find("WebGLCrosshair") != null)
        {
            return;
        }

        WebGLCrosshairGraphic graphic =
            CreateGraphicChild<WebGLCrosshairGraphic>(
                legacyText,
                "WebGLCrosshair");

        graphic.color = legacyText.color;
        legacyText.enabled = false;
    }

    private static T CreateGraphicChild<T>(
        Text source,
        string childName)
        where T : Graphic
    {
        GameObject child = new GameObject(
            childName,
            typeof(RectTransform),
            typeof(CanvasRenderer),
            typeof(T));

        RectTransform childRect = child.GetComponent<RectTransform>();
        childRect.SetParent(source.rectTransform, false);
        childRect.anchorMin = Vector2.zero;
        childRect.anchorMax = Vector2.one;
        childRect.offsetMin = Vector2.zero;
        childRect.offsetMax = Vector2.zero;
        childRect.pivot = new Vector2(0.5f, 0.5f);

        T graphic = child.GetComponent<T>();
        graphic.raycastTarget = false;
        return graphic;
    }
#endif
}
