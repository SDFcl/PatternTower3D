using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed = 12f;     // ความเร็วกระสุน (ปรับได้)

    private Transform target;
    private int damage;

    public void SetTarget(Transform t) => target = t;
    public void SetDamage(int dmg) => damage = dmg;

    private void Update()
    {
        if (target == null)
        {
            Return();
            return;
        }

        transform.position = Vector3.MoveTowards(transform.position, target.position + Vector3.up * 0.5f, speed * Time.deltaTime);
        transform.LookAt(target.position + Vector3.up * 0.5f);

        if (Vector3.Distance(transform.position, target.position) < 0.35f)
        {
            Enemy enemy = target.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }
            Return();
        }
    }

    private void Return()
    {
        if (ObjectPool.Instance != null)
            ObjectPool.Instance.ReturnBullet(gameObject);
        else
            Destroy(gameObject);
    }

    // ถ้าใช้ Collider Trigger
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null) enemy.TakeDamage(damage);
            Return();
        }
    }
}