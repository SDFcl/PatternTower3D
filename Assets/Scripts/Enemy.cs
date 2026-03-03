using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [Header("Components")]
    public NavMeshAgent agent;
    public Transform endPoint;

    [Header("Stats")]
    public int maxHP = 50;
    private int currentHP;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void OnEnable()
    {
        currentHP = maxHP;
        if (endPoint != null)
        {
            agent.SetDestination(endPoint.position);
        }
    }

    void Update()
    {
        // ถึงจุดปลาย → ลดชีวิต + คืน Pool
        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance + 0.1f)
        {
            if (!agent.hasPath || agent.velocity.sqrMagnitude < 0.01f)
            {
                GameManager.Instance.LoseLife();
                ObjectPool.Instance.ReturnEnemy(gameObject);
            }
        }
    }

    public void TakeDamage(int dmg)
    {
        currentHP -= dmg;
        if (currentHP <= 0)
        {
            GameManager.Instance.AddMoney(20);
            ObjectPool.Instance.ReturnEnemy(gameObject);
        }
    }
}