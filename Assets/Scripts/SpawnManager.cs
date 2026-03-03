using UnityEngine;
using System.Collections;

public class SpawnManager : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Transform spawnPos;
    public Transform targetBase;

    void Start() 
    { 
        StartCoroutine(SpawnWave1()); 
    }

    IEnumerator SpawnWave1()
    {
        for (int i = 0; i < 5; i++)
        {
            GameObject enemy = Instantiate(enemyPrefab, spawnPos.position, Quaternion.identity);
            Enemy enemyscript = enemy.GetComponent<Enemy>();

            yield return new WaitForSeconds(2f);
        }
    }
}