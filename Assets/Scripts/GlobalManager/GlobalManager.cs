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
    public bool isStart = false;
    public Dictionary<string, AudioClip> soundClip;
    public GameObject sliderBGM;

    public AudioSource audioSource1;// BGM
    public AudioSource audioSource2;// 倒计时音效
    public AudioSource audioSource3;// 射击音效/子弹耗尽音效

    private bool isPlayCountDown = false;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        soundClip = new Dictionary<string, AudioClip>();
        StartCoroutine(LoadSound("Effect"));
        StartCoroutine(LoadSound("BackgroundMusic"));
    }

    public void StartGame()
    {
        isStart = true;
        gameStartTime = Time.time;
        PlaySound(audioSource1, "battleBGM");
        StartCoroutine(SpawnEnemyCoroutine());
    }

    void Update()
    {
        audioSource1.volume = sliderBGM.GetComponent<Slider>().value;
        if (isStart)
        {
            groundTime = groundTotalTime - (Time.time - groundStartTime);
            if (groundTime <= 0)
            {
                ReStartNextTimeSlice();
            }

            if (groundTime <= 5)
            {
                player.GetComponent<PlayerMove>().canChangeState = true;
                if (!isPlayCountDown)
                {
                    PlaySound(audioSource2, "CountDown");
                    isPlayCountDown = true;
                }

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
    }

    public void ResetGame()
    {
        groundStartTime = Time.time;
        groundTotalTime = 20f;
        globalTime = 0f;
        gameStartTime = Time.time;
        timeSlice = 0;
        StopSound(audioSource2);
        PlaySound(audioSource1, "battleBGM");
    }
}
