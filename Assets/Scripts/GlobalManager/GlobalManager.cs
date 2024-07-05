using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalManager : MonoBehaviour
{
    public GameObject player;
    public GameObject enemyPrefab;
    public float spawnPointDistance = 100f;
    public GameObject enemySet;
    public float enemySpawnInterval = 1f;
    public float groundTime; // 当前时间片时间
    public float groundStartTime; // 当前时间片开始时间
    public float groundTotalTime = 20f; // 时间片总时间
    public float globalTime = 0f; // 总生存时间
    public float gameStartTime = 0f; // 游戏开始时间
    public int timeSlice = 0; // 经历的时间片个数
    public bool isStart = false;

    void Start()
    {
        isStart = true;
        gameStartTime = Time.time;
        player = GameObject.FindGameObjectWithTag("Player");
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
            groundTime = groundTotalTime - (Time.time - groundStartTime);
            if (groundTime <= 0)
            {
                groundStartTime = Time.time;
                timeSlice += 1;
            }

            if (groundTime <= 5)
            {
                player.GetComponent<PlayerMove>().canChangeState = true;
            }
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
        float angle = Random.Range(0f, Mathf.PI * 2);
        Vector3 spawnDirection = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0);
        Vector3 spawnPoint = player.transform.position + spawnDirection * spawnPointDistance;
        GameObject enemy = Instantiate(enemyPrefab, spawnPoint, Quaternion.identity);
        enemy.transform.SetParent(enemySet.transform);
    }

    public void ResetGame()
    {
        groundStartTime = Time.time;
        groundTotalTime = 20f;
        globalTime = 0f;
        gameStartTime = Time.time;
        timeSlice = 0;
    }
}
