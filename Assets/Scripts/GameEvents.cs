using System;
using UnityEngine;

public static class GameEvents
{
    // Observer: ส่ง int ไป UI อัพเดท
    public static event Action<int> MoneyChanged;
    public static event Action<int> LivesChanged;
    public static event Action<int> WaveChanged;

    // Raise Event (เรียกจาก GameManager)
    public static void OnMoneyChanged(int money) => MoneyChanged?.Invoke(money);
    public static void OnLivesChanged(int lives) => LivesChanged?.Invoke(lives);
    public static void OnWaveChanged(int wave) => WaveChanged?.Invoke(wave);
}