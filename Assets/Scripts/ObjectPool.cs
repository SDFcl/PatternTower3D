using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool Instance { get; private set; }

    [Header("Enemy Pool")]
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private int enemyPoolSize = 20;
    [SerializeField] private Transform enemyPoolParent;  // optional แต่แนะนำ

    [Header("Bullet Pool")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private int bulletPoolSize = 50;
    [SerializeField] private Transform bulletPoolParent; // optional

    private Queue<GameObject> enemyPool = new Queue<GameObject>();
    private Queue<GameObject> bulletPool = new Queue<GameObject>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        if (enemyPrefab == null || bulletPrefab == null)
        {
            Debug.LogError("ObjectPool: ยังไม่ได้ลาก Prefab มา!");
            return;
        }

        // สร้าง Enemy Pool
        for (int i = 0; i < enemyPoolSize; i++)
        {
            GameObject enemy = Instantiate(enemyPrefab);
            enemy.SetActive(false);
            if (enemyPoolParent != null) enemy.transform.SetParent(enemyPoolParent);
            enemyPool.Enqueue(enemy);
        }

        // สร้าง Bullet Pool
        for (int i = 0; i < bulletPoolSize; i++)
        {
            GameObject bullet = Instantiate(bulletPrefab);
            bullet.SetActive(false);
            if (bulletPoolParent != null) bullet.transform.SetParent(bulletPoolParent);
            bulletPool.Enqueue(bullet);
        }
    }

    // Enemy
    public GameObject GetEnemy()
    {
        GameObject enemy;
        if (enemyPool.Count > 0)
        {
            enemy = enemyPool.Dequeue();
        }
        else
        {
            enemy = Instantiate(enemyPrefab);
            if (enemyPoolParent != null) enemy.transform.SetParent(enemyPoolParent);
        }
        enemy.SetActive(true);
        return enemy;
    }

    public void ReturnEnemy(GameObject enemy)
    {
        if (enemy == null) return;
        enemy.SetActive(false);
        enemy.transform.localPosition = Vector3.zero;
        enemy.transform.localRotation = Quaternion.identity;

        Rigidbody rb = enemy.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        enemyPool.Enqueue(enemy);
    }

    // Bullet
    public GameObject GetBullet()
    {
        GameObject bullet;
        if (bulletPool.Count > 0)
        {
            bullet = bulletPool.Dequeue();
        }
        else
        {
            bullet = Instantiate(bulletPrefab);
            if (bulletPoolParent != null) bullet.transform.SetParent(bulletPoolParent);
        }
        bullet.SetActive(true);
        return bullet;
    }

    public void ReturnBullet(GameObject bullet)
    {
        if (bullet == null) return;
        bullet.SetActive(false);
        bullet.transform.localPosition = Vector3.zero;
        bullet.transform.localRotation = Quaternion.identity;

        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        bulletPool.Enqueue(bullet);
    }
}