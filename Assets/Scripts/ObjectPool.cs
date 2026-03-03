using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool Instance;  // Singleton: เรียกใช้ทุกที่

    [Header("Pool Settings")]
    public GameObject enemyPrefab;
    public int poolSize = 50;  // Pre-create 50 Enemies
    private Queue<GameObject> enemyPool = new Queue<GameObject>();

    void Awake()
    {
        Instance = this;  // Singleton Init
        CreatePool();
    }

    public GameObject bulletPrefab;
    private Queue<GameObject> bulletPool = new Queue<GameObject>();

    void CreatePool()
    {
        // ... Enemy pool เดิม
        for (int i = 0; i < 100; i++)
        {  // Pool Bullet 100 ลูก
            GameObject bullet = Instantiate(bulletPrefab);
            bullet.SetActive(false);
            bulletPool.Enqueue(bullet);
        }
    }

    public GameObject GetBullet()
    {
        if (bulletPool.Count > 0)
        {
            GameObject b = bulletPool.Dequeue();
            b.SetActive(true);
            return b;
        }
        return Instantiate(bulletPrefab); // Fallback
    }

    public void ReturnBullet(GameObject bullet)
    {
        bullet.SetActive(false);
        bulletPool.Enqueue(bullet);
    }

    public GameObject GetEnemy()
    {
        if (enemyPool.Count > 0)
        {
            GameObject enemy = enemyPool.Dequeue();
            enemy.SetActive(true);
            return enemy;
        }
        // ถ้า Pool หมด: สร้างใหม่ (Fallback)
        return Instantiate(enemyPrefab);
    }

    public void ReturnEnemy(GameObject enemy)
    {
        enemy.SetActive(false);
        enemyPool.Enqueue(enemy);  // กลับ Pool รอ Reuse
    }


}