using UnityEngine;

public class Tower : MonoBehaviour
{
    [SerializeField] private LayerMask enemyLayer;
    public float range = 5f;
    public float fireRate = 1f;
    private float fireTimer = 0f;
    public int maxHP = 100;
    private int currentHP;

    // เพิ่มตัวแปรอ้างอิง Base (จุดจบทางเดิน)
    public Transform basePoint;  // ลาก Base/EndPoint จาก Scene เข้า slot นี้ใน Inspector

    void Start()
    {
        currentHP = maxHP;
        if (basePoint == null)
        {
            Debug.LogWarning("Tower ไม่มี basePoint กำหนด! ยิงแบบ closest แทน");
        }
    }

    void Update()
    {
        fireTimer += Time.deltaTime;
        if (fireTimer >= fireRate)
        {
            Collider[] hits = Physics.OverlapSphere(transform.position, range, enemyLayer);

            if (hits.Length > 0)
            {
                Collider targetEnemy = null;

                if (basePoint != null)
                {
                    // ยิงตัวหน้าสุด = ตัวที่ไกลจาก Base ที่สุด
                    float maxDistToBase = -1f;
                    foreach (var col in hits)
                    {
                        float distToBase = Vector3.Distance(col.transform.position, basePoint.position);
                        if (distToBase > maxDistToBase)
                        {
                            maxDistToBase = distToBase;
                            targetEnemy = col;
                        }
                    }
                }
                else
                {
                    // ถ้าไม่มี basePoint ใช้ closest (fallback)
                    float minDistToTower = float.MaxValue;
                    foreach (var col in hits)
                    {
                        float dist = Vector3.Distance(transform.position, col.transform.position);
                        if (dist < minDistToTower)
                        {
                            minDistToTower = dist;
                            targetEnemy = col;
                        }
                    }
                }

                if (targetEnemy != null)
                {
                    GameObject bulletObj = ObjectPool.Instance.GetBullet();
                    if (bulletObj != null)
                    {
                        bulletObj.transform.position = transform.position + Vector3.up * 1.5f;
                        bulletObj.transform.rotation = Quaternion.LookRotation(targetEnemy.transform.position - transform.position);

                        Bullet bullet = bulletObj.GetComponent<Bullet>();
                        if (bullet != null)
                        {
                            bullet.LaunchTowards(targetEnemy.transform);
                            Debug.Log("Tower ยิง Enemy ตัวหน้าสุด: " + targetEnemy.name);
                        }
                        fireTimer = 0f;
                    }
                }
            }
        }
    }
}