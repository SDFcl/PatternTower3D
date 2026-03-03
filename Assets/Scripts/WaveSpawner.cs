using System.Collections;
using UnityEngine;
using UnityEngine.UI;  // สำหรับปุ่ม

public class WaveSpawner : MonoBehaviour
{
    public Transform spawnPoint;  // Drag จุด Spawn (Path Start)
    public Transform endPoint;
    public Button spawnButton;    // Drag UI Button "Spawn Wave"

    [Header("Wave Config")]
    public int enemiesPerWave = 10;
    public float spawnDelay = 0.5f;

    void Start()
    {
        spawnButton.onClick.AddListener(() => StartCoroutine(SpawnWave(1)));
    }

    IEnumerator SpawnWave(int waveNum)
    {
        for (int i = 0; i < enemiesPerWave * waveNum; i++)
        {
            GameObject enemy = ObjectPool.Instance.GetEnemy();
            enemy.transform.position = spawnPoint.position;
            Enemy _enemy = enemy.GetComponent<Enemy>();
            _enemy.endPoint = endPoint;

            yield return new WaitForSeconds(spawnDelay);
        }
    }
}