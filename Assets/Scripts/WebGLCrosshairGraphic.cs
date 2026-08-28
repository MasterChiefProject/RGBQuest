using UnityEngine;
using UnityEngine.UI;

public sealed class WebGLCrosshairGraphic : MaskableGraphic
{
    protected override void Awake()
    {
        base.Awake();
        raycastTarget = false;
    }

    protected override void OnPopulateMesh(VertexHelper vertexHelper)
    {
        vertexHelper.Clear();

        Rect rect = rectTransform.rect;
        Vector2 center = rect.center;
        float length = Mathf.Min(24f, rect.height * 0.78f);
        float thickness = Mathf.Max(2f, length * 0.13f);
        Color32 vertexColor = color;

        AddQuad(
            vertexHelper,
            new Rect(
                center.x - length * 0.5f,
                center.y - thickness * 0.5f,
                length,
                thickness),
            vertexColor);

        AddQuad(
            vertexHelper,
            new Rect(
                center.x - thickness * 0.5f,
                center.y - length * 0.5f,
                thickness,
                length),
            vertexColor);
    }

    private static void AddQuad(
        VertexHelper vertexHelper,
        Rect rect,
        Color32 vertexColor)
    {
        int start = vertexHelper.currentVertCount;

        vertexHelper.AddVert(
            new Vector2(rect.xMin, rect.yMin),
            vertexColor,
            Vector2.zero);
        vertexHelper.AddVert(
            new Vector2(rect.xMin, rect.yMax),
            vertexColor,
            Vector2.zero);
        vertexHelper.AddVert(
            new Vector2(rect.xMax, rect.yMax),
            vertexColor,
            Vector2.zero);
        vertexHelper.AddVert(
            new Vector2(rect.xMax, rect.yMin),
            vertexColor,
            Vector2.zero);

        vertexHelper.AddTriangle(start, start + 1, start + 2);
        vertexHelper.AddTriangle(start, start + 2, start + 3);
    }
}
