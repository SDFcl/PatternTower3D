using UnityEngine;

public class TowerPlacer : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private LayerMask groundLayer = 1 << 6; // สร้าง Layer "Ground" แล้ว assign ให้ Plane
    public Transform basePoint;

    void Start()
    {
        if (mainCamera == null) mainCamera = Camera.main;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // คลิกซ้าย
        {
            PlaceTower();
        }
    }

    void PlaceTower()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, groundLayer))
        {
            BuildSlot _buildSlot = hit.collider.GetComponent<BuildSlot>();
            if (_buildSlot == null || _buildSlot.isOccupied)
            {
                Debug.Log("There is already a tower");
                return;
            }
            Vector3 placePos = hit.collider.transform.position;
            placePos.y = 0.15f; // ยกขึ้นนิดหน่อยให้ไม่จมพื้น (ปรับตาม scale ของ Tower)
            // ใช้เงิน
            if (GameManager.Instance.SpendMoney(50)) // Cost 50
            {
                GameObject newTower = TowerFactory.Create("Ballista", placePos, basePoint); // ทดสอบ Cannon ก่อน
                newTower.transform.parent = hit.collider.transform;
                _buildSlot.currentTower = newTower;
                _buildSlot.isOccupied = true;
                Debug.Log("Tower placed!");
            }
            else
            {
                Debug.Log("Not enough money!");
            }
        }
    }
}