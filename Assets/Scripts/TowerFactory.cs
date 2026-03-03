using UnityEngine;

public static class TowerFactory
{
    public static GameObject Create(string towerType, Vector3 position, Transform basePoint)
    {
        GameObject towerPrefab = null;

        // Factory Logic – ไม่ต้อง if-else ยาว เพิ่ม type ใหม่แค่เพิ่มบรรทัด
        switch (towerType)
        {
            case "Cannon":
                towerPrefab = Resources.Load<GameObject>("Prefabs/Towers/Cannon_Tower");
                break;
            case "Turret":
                towerPrefab = Resources.Load<GameObject>("Prefabs/Towers/Turret-Tower");
                break;
            case "Ballista":
                towerPrefab = Resources.Load<GameObject>("Prefabs/Towers/Ballista_Tower");
                break;
            default:
                Debug.LogError("Unknown tower type: " + towerType);
                return null;
        }

        if (towerPrefab != null)
        {
            GameObject newTower = Object.Instantiate(towerPrefab, position, Quaternion.identity);
            Tower _tower = newTower.gameObject.GetComponent<Tower>();
            _tower.basePoint = basePoint;

            return newTower;
        }

        return null;
    }
}