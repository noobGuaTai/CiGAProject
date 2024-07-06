using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    public int timeSliceRank = 0; // 游戏中所经历的最长生存的时间片
    public bool isStart = false;// 游戏是否开始
    public Dictionary<string, AudioClip> soundClip;
    public GameObject sliderBGM;
    public GameObject mainCamera;
    public GameObject circleField;
    public Vector3 circleFieldInitLocalScale;
    public int enemyHP = 5;

    public AudioSource audioSource1;// BGM
    public AudioSource audioSource2;// 倒计时音效
    public AudioSource audioSource3;// 射击音效/子弹耗尽音效
    public AudioSource audioSource4;// 捡起道具音效

    public bool isPlayCountDown = false;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        soundClip = new Dictionary<string, AudioClip>();
        circleFieldInitLocalScale = circleField.transform.localScale;
        StartCoroutine(LoadSound("Effect"));
        StartCoroutine(LoadSound("BackgroundMusic"));
    }

    public void StartGame()
    {
        isStart = true;
        gameStartTime = Time.time;
        groundStartTime = Time.time;
        PlaySound(audioSource1, "battleBGM");
        StartCoroutine(SpawnEnemyCoroutine());
    }

    void Update()
    {
        audioSource1.volume = sliderBGM.GetComponent<Slider>().value;
        if (isStart)
        {
            if (groundTime <= 0)
            {
                ReStartNextTimeSlice();
            }
            groundTime = groundTotalTime - (Time.time - groundStartTime);
            if (groundTime <= 5)
            {
                player.GetComponent<PlayerMove>().canChangeState = true;
                if (!isPlayCountDown)
                {
                    PlaySound(audioSource2, "CountDown");
                    isPlayCountDown = true;
                }

            }
            else
            {
                player.GetComponent<PlayerMove>().canChangeState = false;
            }
            globalTime = Time.time - gameStartTime;

            if (player.GetComponent<PlayerAttribute>().HP <= 0)
            {
                GameOver();
            }
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
        enemy.GetComponent<EnemyMove>().HP = enemyHP;
    }

    IEnumerator LoadSound(string path)
    {
        yield return null;
        AudioClip[] clips = Resources.LoadAll<AudioClip>("sound/" + path);

        foreach (AudioClip clip in clips)
        {
            if (!soundClip.ContainsKey(clip.name))
            {
                soundClip[clip.name] = clip;
                print(clip.name);
            }
            else
            {
                Debug.LogWarning("Duplicate effect prefab name found in 'sound/Effect': " + clip.name);
            }
        }
    }

    public void PlaySound(AudioSource audioSource, string clipName)
    {
        audioSource.clip = soundClip[clipName];
        audioSource.Play();
    }

    public void StopSound(AudioSource audioSource)
    {
        if (audioSource != null)
        {
            audioSource.Stop();
        }
    }

    public void ReStartNextTimeSlice()
    {
        groundStartTime = Time.time;
        timeSlice += 1;
        isPlayCountDown = false;
        StopSound(audioSource2);
        if (timeSlice % 5 == 0)// 每5次时间片就增加一次难度
        {
            float localScaleX = circleField.GetComponent<CircleField>().transform.localScale.x;
            float localScaleY = circleField.GetComponent<CircleField>().transform.localScale.y;
            circleField.GetComponent<CircleField>().transform.localScale = new Vector3(localScaleX - 50, localScaleY - 50, circleField.GetComponent<CircleField>().transform.localScale.z);
            enemyHP += 5;
        }
    }

    public void ResetGame()
    {
        groundStartTime = Time.time;
        groundTotalTime = 20f;
        globalTime = 0f;
        gameStartTime = Time.time;
        timeSlice = 0;
        circleField.transform.localScale = circleFieldInitLocalScale;
        enemyHP = 5;
        StopSound(audioSource2);
        PlaySound(audioSource1, "battleBGM");
    }

    public void GameOver()
    {
        Time.timeScale = 0f;
        timeSliceRank = timeSliceRank < timeSlice ? timeSlice : timeSliceRank;
    }
}
