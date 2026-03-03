using UnityEngine;

public class TowerPlacer : MonoBehaviour
{
    [SerializeField] private LayerMask buildLayer;          // Layer ที่วางได้
    [SerializeField] private float gridSize = 1f;           // ขนาด grid snap (ถ้ามี)

    private string selectedTowerType = null;
    private GameObject currentPreview = null;

    private void Update()
    {
        // ถ้ายังไม่เลือก Tower → ไม่ทำอะไร
        if (string.IsNullOrEmpty(selectedTowerType))
        {
            HidePreview();
            return;
        }

        // Raycast จากเมาส์ไปหาตำแหน่งวาง
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, buildLayer))
        {
            Vector3 pos = hit.point;

            // Snap to grid (ถ้าต้องการ)
            pos.x = Mathf.Round(pos.x / gridSize) * gridSize;
            pos.z = Mathf.Round(pos.z / gridSize) * gridSize;
            pos.y = 0.5f;  // สูงจากพื้นเล็กน้อย

            // แสดง Preview
            ShowPreview(pos);

            // คลิกซ้าย → วางจริง
            if (Input.GetMouseButtonDown(0))
            {
                PlaceTower(pos);
            }
        }
        else
        {
            HidePreview();
        }

        // กด Right Click หรือ Esc → ยกเลิกเลือก
        if (Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.Escape))
        {
            CancelPlacement();
        }
    }

    public void SetSelectedTowerType(string type)
    {
        selectedTowerType = type;
        UpdatePreview();
    }

    private void ShowPreview(Vector3 position)
    {
        if (currentPreview == null)
        {
            currentPreview = TowerFactory.Instance.CreateTowerPreview(selectedTowerType);
            if (currentPreview == null) return;
        }

        currentPreview.transform.position = position;
        currentPreview.SetActive(true);
    }

    private void HidePreview()
    {
        if (currentPreview != null)
        {
            currentPreview.SetActive(false);
        }
    }

    private void UpdatePreview()
    {
        if (currentPreview != null)
        {
            Destroy(currentPreview);
            currentPreview = null;
        }
    }

    private void PlaceTower(Vector3 position)
    {
        if (TowerFactory.Instance.CanAfford(selectedTowerType))
        {
            TowerFactory.Instance.CreateTower(selectedTowerType, position);
            GameManager.Instance.SpendMoney(TowerFactory.Instance.GetTowerCost(selectedTowerType));

            // ล้าง Preview และเลือกใหม่
            UpdatePreview();

            // แจ้ง UI ให้ clear selection (ถ้ามี TowerSelectorUI)
            var selector = FindObjectOfType<TowerSelectorUI>();
            if (selector != null) selector.ClearSelection();
        }
        else
        {
            Debug.Log("เงินไม่พอ!");
        }
    }

    private void CancelPlacement()
    {
        UpdatePreview();
        selectedTowerType = null;

        var selector = FindObjectOfType<TowerSelectorUI>();
        if (selector != null) selector.ClearSelection();
    }

    private void OnDestroy()
    {
        if (currentPreview != null)
        {
            Destroy(currentPreview);
        }
    }
}