using UnityEngine;

public class Tower : MonoBehaviour
{
    [Header("Stats ปรับตามประเภทป้อม")]
    [SerializeField] private float fireRate = 1.5f;     // Basic = เร็ว, Sniper = ช้าแต่แรง
    [SerializeField] private int damage = 25;
    [SerializeField] private float range = 6f;

    [Header("ส่วนประกอบ")]
    [SerializeField] private Transform firePoint;       // จุดปล่อยกระสุน (ลูกของ Tower)

    private float nextFireTime = 0f;

    private void Update()
    {
        if (Time.time < nextFireTime) return;

        Transform target = FindNearestEnemy();
        if (target != null)
        {
            Shoot(target);
            nextFireTime = Time.time + fireRate;
        }
    }

    private Transform FindNearestEnemy()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, range);
        Transform nearest = null;
        float minDist = float.MaxValue;

        foreach (var hit in hits)
        {
            if (hit.CompareTag("Enemy"))
            {
                float dist = Vector3.Distance(transform.position, hit.transform.position);
                if (dist < minDist)
                {
                    minDist = dist;
                    nearest = hit.transform;
                }
            }
        }
        return nearest;
    }

    private void Shoot(Transform target)
    {
        if (firePoint == null || ObjectPool.Instance == null) return;

        GameObject bullet = ObjectPool.Instance.GetBullet();
        if (bullet == null) return;

        bullet.transform.position = firePoint.position;
        bullet.transform.rotation = firePoint.rotation;

        Bullet bulletScript = bullet.GetComponent<Bullet>();
        if (bulletScript != null)
        {
            bulletScript.SetTarget(target);
            bulletScript.SetDamage(damage);  // ส่งดาเมจจากป้อมไป
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}