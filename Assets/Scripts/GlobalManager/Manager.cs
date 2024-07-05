using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Transform[] spawnPoints;
    public float enemySpawnInterval = 1f;
    public float groundTime;// 当前时间片时间
    public float groundStartTime;// 当前时间片开始时间
    public float groundTotalTime = 20f;// 时间片总时间
    public float globalTime = 0f;// 总生存时间
    public float gameStartTime = 0f;// 游戏开始时间
    public bool isStart = false;
    void Start()
    {
        isStart = true;
        gameStartTime = Time.time;
        StartCoroutine(SpawnEnemyCoroutine());
    }

    public void StartGame()
    {
        isStart = true;
        gameStartTime = Time.time;
        StartCoroutine(SpawnEnemyCoroutine());
    }


    void Update()
    {
        if (isStart)
        {
            groundTime = Time.time - groundStartTime;
            if (groundTime > groundTotalTime)
                groundStartTime = Time.time;
            globalTime = Time.time - gameStartTime;
        }
    }

    IEnumerator SpawnEnemyCoroutine()
    {
        while (true)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(enemySpawnInterval);
        }
    }

    void SpawnEnemy()
    {
        int randomIndex = Random.Range(0, spawnPoints.Length);
        Transform selectedSpawnPoint = spawnPoints[randomIndex];

        Instantiate(enemyPrefab, selectedSpawnPoint.position, selectedSpawnPoint.rotation);
    }
    
}
