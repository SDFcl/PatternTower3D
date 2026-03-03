using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Settings")]
    public float speed = 18f;          // เพิ่มความเร็วให้ชัดเจนขึ้น
    public int damage = 15;

    private Transform targetEnemy;     // เป้าหมายที่ตั้งไว้ตอนยิง
    private Vector3 moveDirection;     // ทิศทางที่คำนวณไว้แล้ว
    private bool isLaunched = false;

    private Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody>();
        }

        // ตั้งค่าพื้นฐานทุกครั้งที่เกิดใหม่
        rb.useGravity = false;
        rb.isKinematic = false;
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        rb.constraints = RigidbodyConstraints.FreezeRotation; // ป้องกันหมุนเอง
    }

    // ฟังก์ชันเรียกตอนยิง (เรียกจาก Tower)
    public void LaunchTowards(Transform target)
    {
        if (target == null)
        {
            ObjectPool.Instance.ReturnBullet(gameObject);
            return;
        }

        targetEnemy = target;
        moveDirection = (target.position - transform.position).normalized;

        // ตั้งค่า velocity ทันที
        rb.velocity = moveDirection * speed;

        // หมุนกระสุนให้ชี้ไปทางเป้า (ดูสวยขึ้น)
        transform.rotation = Quaternion.LookRotation(moveDirection);

        isLaunched = true;

        // Debug เพื่อเช็คว่าทำงานจริง
        Debug.Log($"Bullet launched towards {target.name} from {transform.position}");
    }

    void Update()
    {
        if (!isLaunched) return;

        // ถ้าเป้าหมายหาย หรือกระสุนไกลเกินไป → คืน Pool
        if (targetEnemy == null || Vector3.Distance(transform.position, targetEnemy.position) > 60f)
        {
            ObjectPool.Instance.ReturnBullet(gameObject);
            return;
        }

        // ถ้าใช้ Rigidbody แล้ว ไม่ควรใช้ MoveTowards อีก (จะขัดกัน)
        // แต่ถ้าอยากให้ตามเป้าแบบ homing นิด ๆ สามารถ uncomment ได้
        // rb.velocity = (targetEnemy.position - transform.position).normalized * speed;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
                Debug.Log($"Bullet hit {other.name} - Damage: {damage}");
            }
            ObjectPool.Instance.ReturnBullet(gameObject);
        }
    }

    // ถ้ากระสุนชนอะไรที่ไม่ใช่ Enemy → คืน Pool
    void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Enemy"))
        {
            ObjectPool.Instance.ReturnBullet(gameObject);
        }
    }
}