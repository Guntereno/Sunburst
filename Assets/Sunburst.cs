using UnityEngine;
using UnityEngine.UI;

[AddComponentMenu("UI/Sunburst")]
[ExecuteAlways]
public class Sunburst : MaskableGraphic
{
    public enum UvMapping
    {
        PerSegment,
        Uniform
    }

    [Range(3, 64)]
    public int SegmentCount = 24;

    [Range(-1, 64)]
    public int BeamCap = 0;

    [Range(0, 1)]
    public float SegmentWidth = 0.5f;

    public UvMapping UvMappingMode;

    private void Update()
    {
		m_Material.SetFloat("_SegmentCount", SegmentCount);
		m_Material.SetFloat("_SegmentWidth", SegmentWidth);
	}

    protected override void OnRectTransformDimensionsChange()
    {
        base.OnRectTransformDimensionsChange();

        SetVerticesDirty();
        SetMaterialDirty();
    }

    protected override void OnPopulateMesh(VertexHelper vh)
    {
        vh.Clear();
        float radius = Mathf.Min(rectTransform.rect.width, rectTransform.rect.height) * 0.5f;
        GenerateMesh(vh, radius);
    }

    public void GenerateMesh(VertexHelper vh, float radius)
    {
        int numBeams = SegmentCount;
        if ((BeamCap > 0) && (BeamCap < SegmentCount))
        {
            numBeams = BeamCap;
        }

        UIVertex vert = new UIVertex();
        vert.color = this.color;

        const int vertsPerSegment = 3;

        int numVerts = SegmentCount * vertsPerSegment;

        float segmentTheta = (2.0f * Mathf.PI) / SegmentCount;
        float segmentHalfTheta = segmentTheta * 0.5f * SegmentWidth;
        for (int segmentIndex = 0; segmentIndex < numBeams; ++segmentIndex)
        {
            float centreTheta = segmentIndex * segmentTheta;
            float firstTheta = centreTheta - segmentHalfTheta;
            float secondTheta = centreTheta + segmentHalfTheta;

            int firstVertIndex = segmentIndex * vertsPerSegment;
            int vertIndex0 = firstVertIndex + 0;
            int vertIndex1 = firstVertIndex + 1;
            int vertIndex2 = firstVertIndex + 2;

            vert.position = Vector3.zero;
            switch(UvMappingMode)
            {
                case UvMapping.PerSegment:
                    vert.uv0 = new Vector2(0.5f, 0.0f);
                    break;

                case UvMapping.Uniform:
                    vert.uv0 = new Vector2(0.0f, 0.0f);
                    break;
            }
            vh.AddVert(vert);

            Vector3 dir = new Vector3(
                Mathf.Sin(firstTheta),
                Mathf.Cos(firstTheta),
                0.0f);
            vert.position = dir * radius;
            switch (UvMappingMode)
            {
                case UvMapping.PerSegment:
                    vert.uv0 = new Vector2(1.0f, 1.0f);
                    break;

                case UvMapping.Uniform:
                    vert.uv0 = new Vector2(dir.x, dir.y);
                    break;
            }
            vh.AddVert(vert);

            dir = new Vector3(
                Mathf.Sin(secondTheta),
                Mathf.Cos(secondTheta),
                0.0f);
            vert.position = dir * radius;
            switch (UvMappingMode)
            {
                case UvMapping.PerSegment:
                    vert.uv0 = new Vector2(0.0f, 1.0f);
                    break;

                case UvMapping.Uniform:
                    vert.uv0 = new Vector2(dir.x, dir.y);
                    break;
            }
            vh.AddVert(vert);

            vh.AddTriangle(vertIndex0, vertIndex1, vertIndex2);
        }
    }
}
