using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class TowerRangeVisualizer : MonoBehaviour
{
    [SerializeField] private float range = 5f;
    [SerializeField] private Color editColor = new Color(0, 1, 0, 0.5f);   // เขียวใส ๆ ตอน Edit
    [SerializeField] private Color playColor = new Color(1, 0.5f, 0, 0.5f); // ส้มใส ๆ ตอน Play
    [SerializeField] private int segments = 64;  // ความละเอียดวงกลม

    private LineRenderer lineRenderer;

    void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = segments + 1;
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
        lineRenderer.loop = true;
        lineRenderer.useWorldSpace = true;
    }

    void OnValidate() // อัพเดทตอนแก้ range ใน Inspector (Edit Mode)
    {
        DrawRangeCircle(editColor);
    }

    void Start()
    {
        DrawRangeCircle(playColor);
    }

    void DrawRangeCircle(Color color)
    {
        if (lineRenderer == null) return;

        lineRenderer.startColor = color;
        lineRenderer.endColor = color;

        float angleStep = 360f / segments;
        for (int i = 0; i <= segments; i++)
        {
            float angle = i * angleStep * Mathf.Deg2Rad;
            Vector3 point = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * range;
            point += transform.position; // ยกขึ้นจากพื้นเล็กน้อยถ้าต้องการ
            lineRenderer.SetPosition(i, point);
        }
    }

    // ถ้าอยากให้ range เปลี่ยนตามตัวแปร Tower.range ได้
    public void UpdateRange(float newRange)
    {
        range = newRange;
        DrawRangeCircle(Application.isPlaying ? playColor : editColor);
    }
}