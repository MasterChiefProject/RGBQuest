using UnityEngine;
using UnityEngine.UI;

public sealed class WebGLHealthHeartsGraphic : MaskableGraphic
{
    private const int MaxHealth = 3;
    private int lastHealth = int.MinValue;

    protected override void Awake()
    {
        base.Awake();
        raycastTarget = false;
    }

    private void Update()
    {
        if (lastHealth == Globals.health)
        {
            return;
        }

        lastHealth = Globals.health;
        SetVerticesDirty();
    }

    protected override void OnPopulateMesh(VertexHelper vertexHelper)
    {
        vertexHelper.Clear();

        int hearts = Mathf.Clamp(Globals.health, 0, MaxHealth);
        if (hearts == 0)
        {
            return;
        }

        Rect rect = rectTransform.rect;
        float heartSize = Mathf.Min(40f, rect.height * 0.35f);
        float spacing = heartSize * 0.28f;
        float startX = rect.xMin + heartSize * 0.5f;
        float centerY = rect.center.y;
        Color32 vertexColor = color;

        for (int index = 0; index < hearts; index++)
        {
            Vector2 center = new Vector2(
                startX + index * (heartSize + spacing),
                centerY);

            AddHeart(vertexHelper, center, heartSize, vertexColor);
        }
    }

    private static void AddHeart(
        VertexHelper vertexHelper,
        Vector2 center,
        float size,
        Color32 vertexColor)
    {
        float circleRadius = size * 0.245f;
        float circleY = size * 0.15f;
        float circleX = size * 0.215f;

        AddCircle(
            vertexHelper,
            center + new Vector2(-circleX, circleY),
            circleRadius,
            vertexColor);

        AddCircle(
            vertexHelper,
            center + new Vector2(circleX, circleY),
            circleRadius,
            vertexColor);

        AddTriangle(
            vertexHelper,
            center + new Vector2(-size * 0.46f, size * 0.10f),
            center + new Vector2(size * 0.46f, size * 0.10f),
            center + new Vector2(0f, -size * 0.48f),
            vertexColor);
    }

    private static void AddCircle(
        VertexHelper vertexHelper,
        Vector2 center,
        float radius,
        Color32 vertexColor)
    {
        const int segments = 16;
        int centerIndex = vertexHelper.currentVertCount;

        vertexHelper.AddVert(center, vertexColor, Vector2.zero);

        for (int index = 0; index <= segments; index++)
        {
            float angle = index * Mathf.PI * 2f / segments;
            Vector2 point = center + new Vector2(
                Mathf.Cos(angle),
                Mathf.Sin(angle)) * radius;

            vertexHelper.AddVert(point, vertexColor, Vector2.zero);
        }

        for (int index = 0; index < segments; index++)
        {
            vertexHelper.AddTriangle(
                centerIndex,
                centerIndex + index + 1,
                centerIndex + index + 2);
        }
    }

    private static void AddTriangle(
        VertexHelper vertexHelper,
        Vector2 a,
        Vector2 b,
        Vector2 c,
        Color32 vertexColor)
    {
        int start = vertexHelper.currentVertCount;
        vertexHelper.AddVert(a, vertexColor, Vector2.zero);
        vertexHelper.AddVert(b, vertexColor, Vector2.zero);
        vertexHelper.AddVert(c, vertexColor, Vector2.zero);
        vertexHelper.AddTriangle(start, start + 1, start + 2);
    }
}
