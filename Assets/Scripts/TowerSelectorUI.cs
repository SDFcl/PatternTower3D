using UnityEngine;
using UnityEngine.UI;

public class TowerSelectorUI : MonoBehaviour
{
    [Header("ปุ่มเลือก Tower")]
    [SerializeField] private Button basicTowerButton;
    [SerializeField] private Button sniperTowerButton;
    [SerializeField] private Button thirdTowerButton;     // Tower แบบที่ 3 ของคุณ

    [Header("UI แสดงว่ากำลังเลือกอะไร")]
    [SerializeField] private Text selectedText;           // Text UI แสดง "Selected: Basic Tower"

    private string selectedTowerType = null;
    private TowerPlacer towerPlacer;                      // จะหา auto

    private void Awake()
    {
        towerPlacer = FindObjectOfType<TowerPlacer>();
        if (towerPlacer == null)
        {
            Debug.LogError("ไม่พบ TowerPlacer ใน Scene!");
        }
    }

    private void Start()
    {
        // เชื่อมปุ่ม
        if (basicTowerButton != null)
            basicTowerButton.onClick.AddListener(() => SelectTower("Basic"));

        if (sniperTowerButton != null)
            sniperTowerButton.onClick.AddListener(() => SelectTower("Sniper"));

        if (thirdTowerButton != null)
            thirdTowerButton.onClick.AddListener(() => SelectTower("Third"));  // เปลี่ยนชื่อจริงของ Tower แบบที่ 3

        UpdateSelectedText();
    }

    private void SelectTower(string type)
    {
        selectedTowerType = type;
        towerPlacer.SetSelectedTowerType(type);   // ส่งให้ TowerPlacer รู้ว่าต้อง Preview แบบไหน
        UpdateSelectedText();
    }

    private void UpdateSelectedText()
    {
        if (selectedText != null)
        {
            selectedText.text = selectedTowerType != null
                ? $"Selected: {selectedTowerType} Tower"
                : "เลือกหอคอยก่อนวาง";
        }
    }

    // เรียกใช้ตอนวาง Tower สำเร็จ (หรือกด Esc เพื่อยกเลิก)
    public void ClearSelection()
    {
        selectedTowerType = null;
        towerPlacer.SetSelectedTowerType(null);
        UpdateSelectedText();
    }
}