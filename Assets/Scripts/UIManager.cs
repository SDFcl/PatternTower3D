using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI moneyText;
    [SerializeField] private TextMeshProUGUI livesText;
    [SerializeField] private TextMeshProUGUI waveText;

    void Start()
    {
        GameEvents.MoneyChanged += UpdateMoneyUI;
        GameEvents.LivesChanged += UpdateLivesUI;
        GameEvents.WaveChanged += UpdateWaveUI;
    }

    private void UpdateMoney()
    {
        if (moneyText != null)
            moneyText.text = "Money: " + GameManager.Instance.money;
    }

    private void UpdateLives()
    {
        if (livesText != null)
            livesText.text = "Lives: " + GameManager.Instance.lives;
    }

    private void UpdateWave()
    {
        if (waveText != null)
            waveText.text = "Wave: " + GameManager.Instance.currentWave;
    }

    void UpdateMoneyUI(int money) { moneyText.text = "Money: " + money; }
    void UpdateLivesUI(int lives) { livesText.text = "Lives: " + lives; }
    void UpdateWaveUI(int wave) { waveText.text = "Wave: " + wave; }

    void OnDestroy()
    {  // Unsubscribe ป้องกัน Memory Leak
        GameEvents.MoneyChanged -= UpdateMoneyUI;
        GameEvents.LivesChanged -= UpdateLivesUI;
        GameEvents.WaveChanged -= UpdateWaveUI;
    }
}