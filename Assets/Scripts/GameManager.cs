using UnityEngine;
using System; // สำหรับ Action

public class GameManager : MonoBehaviour
{
    // Singleton (Week 02)
    public static GameManager Instance { get; private set; }

    // ข้อมูลหลักของเกม
    public int money = 200;
    public int lives = 20;
    public int currentWave = 1;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // ฟังก์ชันใช้งานง่าย ๆ + Event Bus (Week 04)
    public bool SpendMoney(int cost)
    {
        if (money >= cost)
        {
            money -= cost;
            GameEvents.OnMoneyChanged(money); // UI อัพเดททันที!
            return true;
        }
        return false;
    }

    public void AddMoney(int amount)
    {
        money += amount;
        GameEvents.OnMoneyChanged(money);
    }

    public void LoseLife()
    {
        lives--;
        GameEvents.OnLivesChanged(lives);  // ✅ แก้แล้ว! รันได้
        if (lives <= 0) Debug.Log("GAME OVER!");
    }

    public void NextWave()
    {
        currentWave++;
        GameEvents.OnWaveChanged(currentWave);
    }
}