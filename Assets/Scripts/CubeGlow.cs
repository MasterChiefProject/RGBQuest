using UnityEngine;

public class CubeGlow : MonoBehaviour
{
    Renderer renderer;
    Color baseColor;

    public float glowIntensity = 2f;

    private void Awake()
    {
        renderer = GetComponent<Renderer>();
        baseColor = renderer.material.color;
        renderer.material.EnableKeyword("_EMISSION");
        LightDown();
    }

    public void LightUp()
    {
        renderer.material.SetColor("_EmissionColor", baseColor * glowIntensity);
    }

    public void LightDown() {
        renderer.material.SetColor("_EmissionColor", Color.black);
    }

}
