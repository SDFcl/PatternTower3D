using UnityEngine;

public class TowerFactory : MonoBehaviour
{
    public static TowerFactory Instance { get; private set; }

    [Header("Prefabs")]
    [SerializeField] private GameObject basicTowerPrefab;
    [SerializeField] private GameObject sniperTowerPrefab;
    [SerializeField] private GameObject thirdTowerPrefab;     // Tower แบบที่ 3

    [Header("Preview Material (optional)")]
    [SerializeField] private Material previewMaterial;        // Material โปร่งแสง

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public GameObject CreateTower(string type, Vector3 position)
    {
        GameObject prefab = GetPrefabByType(type);
        if (prefab == null) return null;

        position.y -= 0.2f;

        GameObject tower = Instantiate(prefab, position, Quaternion.identity);
        tower.name = type + " Tower";
        return tower;
    }

    public GameObject CreateTowerPreview(string type)
    {
        GameObject prefab = GetPrefabByType(type);
        if (prefab == null) return null;

        GameObject preview = Instantiate(prefab, Vector3.zero, Quaternion.identity);
        preview.name = type + "_Preview";

        // ทำให้เป็น Preview (โปร่งแสง + ไม่ชน)
        SetPreviewAppearance(preview);

        return preview;
    }

    private GameObject GetPrefabByType(string type)
    {
        return type switch
        {
            "Basic" => basicTowerPrefab,
            "Sniper" => sniperTowerPrefab,
            "Third" => thirdTowerPrefab,     // เปลี่ยนชื่อจริงของคุณ
            _ => null
        };
    }

    private void SetPreviewAppearance(GameObject preview)
    {
        Renderer[] renderers = preview.GetComponentsInChildren<Renderer>();
        foreach (var renderer in renderers)
        {
            if (previewMaterial != null)
            {
                renderer.material = previewMaterial;
            }
            else
            {
                Color c = renderer.material.color;
                c.a = 0.5f;
                renderer.material.color = c;
            }
        }

        // ปิด Collider ชั่วคราว (ไม่ให้ Raycast ชน)
        Collider[] colliders = preview.GetComponentsInChildren<Collider>();
        foreach (var col in colliders) col.enabled = false;
    }

    // ฟังก์ชันช่วยเหลือ (สมมติว่ามีระบบเงิน)
    public bool CanAfford(string type)
    {
        int cost = GetTowerCost(type);
        return GameManager.Instance.money >= cost;
    }

    public int GetTowerCost(string type)
    {
        return type switch
        {
            "Basic" => 50,
            "Sniper" => 120,
            "Third" => 80,     // ปรับตามจริง
            _ => 0
        };
    }
}