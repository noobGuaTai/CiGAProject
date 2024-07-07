using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GlobalManager : MonoBehaviour
{
    public GameObject player;
    public GameObject enemyPrefab;
    public float spawnPointDistance = 220;
    public GameObject enemySet;
    public float enemySpawnInterval = 0.2f;
    public float enemySpawnIntervalInit = 2f;
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
    public GameObject circleFieldVisual;
    public int enemyHP = 5;
    public float enemySpeed = 5;
    public int ememyCount = 0;

    public float topDownBorder = 300f;//地图边界
    public float leftRightBorder = 400f;
    public GameObject backgroundObject1;
    public GameObject backgroundObject2;
    public int numberOfObjects = 10;
    public float minDistanceBetweenObjects = 50f;
    public List<Vector3> backgroundObjectSpawnPositions = new List<Vector3>(); // 存储生成的位置
    public GameObject timeFlag;
    public GameObject gameUI;
    public GameObject gameOverPanel;
    public GameObject bulletSet;
    public GameObject timeSliceUI;
    public GameObject timeSliceTotalUI;
    public float widthBorder = 200f;
    public float heightBorder = 300f;
    public GameObject tileMap;
    public float tileMapMoveSpeed = 40f;
    public GameObject mainPage;
    public GameObject totalTimeUI;

    public AudioSource audioSource1;// BGM
    public AudioSource audioSource2;// 倒计时音效
    public AudioSource audioSource3;// 射击音效/子弹耗尽音效
    public AudioSource audioSource4;// 捡起道具音效
    public AudioSource audioSource5;// 升级音效
    public bool isPlayCountDown = false;

    private Vector3 tileMapInitialPosition;
    private Vector3 tileMapTargetPosition;
    private int directionIndex = 0;
    private Vector3[] directions;


    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        soundClip = new Dictionary<string, AudioClip>();
        circleFieldInitLocalScale = circleField.transform.localScale;
        gameUI.SetActive(false);
        directions = new Vector3[]
        {
            new Vector3(1, 0, 0),
            new Vector3(0, -1, 0),
            new Vector3(-1, 0, 0),
            new Vector3(0, 1, 0)
        };
        // SpawnBackgroundObjects();
        circleFieldVisual.SetActive(false);
        StartCoroutine(LoadSound("Effect"));
        StartCoroutine(LoadSound("BackgroundMusic"));
       
    }

    public void StartGame()
    {
        isStart = true;
        gameStartTime = Time.time;
        groundStartTime = Time.time;
        gameUI.SetActive(true);
        circleField.GetComponent<CircleField>().StartMove();
        PlaySound(audioSource1, "battleBGM");
        StartCoroutine(SpawnEnemyCoroutine());
        timeFlag.GetComponent<ui_timeline_flag>().StartMove();
        tileMap.transform.position = tileMapInitialPosition;
        circleFieldVisual.SetActive(true);
    }

    public delegate void on_last_5_begin_type();
    public on_last_5_begin_type on_last_5_begin;

    public delegate void on_enter_next_timeslice_type();
    public on_enter_next_timeslice_type on_enter_next_timeslice;

    void Update()
    {
        audioSource1.volume = sliderBGM.GetComponent<Slider>().value;
        audioSource2.volume = sliderBGM.GetComponent<Slider>().value;
        audioSource3.volume = sliderBGM.GetComponent<Slider>().value;
        audioSource4.volume = sliderBGM.GetComponent<Slider>().value;
        audioSource5.volume = sliderBGM.GetComponent<Slider>().value;
        
        if (isStart)
        {
            if (groundTime <= 0)
            {
                ReStartNextTimeSlice();
            }
            groundTime = groundTotalTime - (Time.time - groundStartTime);
            if (groundTime <= 5f)
            {
                player.GetComponent<PlayerMove>().canChangeState = true;
                if (!isPlayCountDown)
                {
                    PlaySound(audioSource2, "CountDown");
                    isPlayCountDown = true;
                    on_last_5_begin.Invoke();
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
            totalTimeUI.GetComponent<TextMeshProUGUI>().text = timeSlice.ToString();
        }
        else
        {
            MoveTilemap();
        }
    }

    IEnumerator SpawnEnemyCoroutine()
    {
        while (isStart)
        {
            if (isStart && ememyCount < get_max_enemy())
                SpawnEnemy();
            yield return new WaitForSeconds(enemySpawnInterval);

        }
    }


    int get_max_enemy()
    {
        return (int)Unity.Mathematics.math.pow(timeSlice, 1.5) + timeSlice + 1;
    }



    void SpawnEnemy()
    {
        float angle = Random.Range(0f, Mathf.PI * 2);
        Vector3 spawnDirection = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0);
        Vector3 spawnPoint = player.transform.position + spawnDirection * spawnPointDistance;
        GameObject enemy = Instantiate(enemyPrefab, spawnPoint, Quaternion.identity);
        enemy.transform.SetParent(enemySet.transform);

        var em = enemy.GetComponent<EnemyMove>();
        // em.HP = enemyHP;
        em.globalManager = this;
        ememyCount++;
    }

    // public void on_enemy_dead(EnemyMove who)
    // {
    //     ememyCount--;
    // }

    IEnumerator LoadSound(string path)
    {
        yield return null;
        AudioClip[] clips = Resources.LoadAll<AudioClip>("sound/" + path);

        foreach (AudioClip clip in clips)
        {
            if (!soundClip.ContainsKey(clip.name))
            {
                soundClip[clip.name] = clip;
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

    void MoveTilemap()
    {
        float step = tileMapMoveSpeed * Time.deltaTime;
        tileMap.transform.position = Vector3.MoveTowards(tileMap.transform.position, tileMapTargetPosition, step);

        if (Vector3.Distance(tileMap.transform.position, tileMapTargetPosition) < 0.001f)
        {
            directionIndex = (directionIndex + 1) % directions.Length;
            switch (directionIndex)
            {
                case 0:
                    tileMapTargetPosition = new Vector3(tileMapInitialPosition.x + widthBorder, tileMap.transform.position.y, tileMap.transform.position.z);
                    break;
                case 1:
                    tileMapTargetPosition = new Vector3(tileMap.transform.position.x, tileMapInitialPosition.y - heightBorder, tileMap.transform.position.z);
                    break;
                case 2:
                    tileMapTargetPosition = new Vector3(tileMapInitialPosition.x - widthBorder, tileMap.transform.position.y, tileMap.transform.position.z);
                    break;
                case 3:
                    tileMapTargetPosition = new Vector3(tileMap.transform.position.x, tileMapInitialPosition.y + heightBorder, tileMap.transform.position.z);
                    break;
            }
        }
    }

    public void ReStartNextTimeSlice()
    {
        groundStartTime = Time.time;
        timeSlice += 1;
        isPlayCountDown = false;
        ememyCount = 0;
        StopSound(audioSource2);
        timeFlag.GetComponent<ui_timeline_flag>().ResetMove();
        if (timeSlice % 2 == 0 && circleField.GetComponent<CircleCollider2D>().radius > 100f)// 每2次时间片就增加一次难度
        {
            StartCoroutine(SmoothScaleCircleField(-10, 5f));
            enemySpawnInterval = enemySpawnInterval > 0.1f ? enemySpawnInterval - 0.1f : enemySpawnInterval;
            // player.GetComponent<PlayerMove>().moveSpeed += 2;
        }
        on_enter_next_timeslice.Invoke();
    }

    IEnumerator SmoothScaleCircleField(float radiusChange, float duration)
    {
        CircleCollider2D collider = circleField.GetComponent<CircleCollider2D>();
        float initialRadius = collider.radius;
        float targetRadius = initialRadius + radiusChange;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            collider.radius = Mathf.Lerp(initialRadius, targetRadius, elapsedTime / duration);
            yield return null;
        }

        // 确保最终radius精确
        collider.radius = targetRadius;
    }

    public void ResetGame()
    {
        Time.timeScale = 1f;
        isStart = true;
        gameOverPanel.SetActive(false);
        player.transform.position = Vector2.zero;
        player.GetComponent<PlayerAttribute>().HP = player.GetComponent<PlayerAttribute>().MAXHP;
        player.GetComponent<PlayerMove>().canChangeState = false;
        player.GetComponent<PlayerMove>().animator.speed = 1f;
        player.GetComponent<PlayerMove>().shootCoolDown = player.GetComponent<PlayerMove>().initShootCoolDown;
        player.GetComponent<PlayerAttribute>().Level = 0;
        player.GetComponent<PlayerAttribute>().ToNextLevelEXP = 0;
        player.GetComponent<PlayerAttribute>().ATK = player.GetComponent<PlayerAttribute>().initATK;
        player.GetComponent<PlayerAttribute>().MP = player.GetComponent<PlayerAttribute>().initMP;
        player.GetComponent<PlayerAttribute>().MAXMP = player.GetComponent<PlayerAttribute>().initMP;
        player.GetComponent<PlayerAttribute>().endurance = player.GetComponent<PlayerAttribute>().initEndurance;
        player.GetComponent<PlayerAttribute>().enduranceMAX = player.GetComponent<PlayerAttribute>().initEndurance;
        groundStartTime = Time.time;
        groundTotalTime = 20f;
        globalTime = 0f;
        gameStartTime = Time.time;
        groundTime = groundTotalTime;
        timeSlice = 0;
        circleField.transform.position = Vector2.zero;
        circleField.transform.localScale = circleFieldInitLocalScale;
        circleField.GetComponent<CircleField>().StartMove();
        enemyHP = 5;
        StopSound(audioSource2);
        PlaySound(audioSource1, "battleBGM");
        timeFlag.GetComponent<ui_timeline_flag>().ResetMove();
        circleFieldVisual.SetActive(true);
        foreach (Transform child in enemySet.transform)
        {
            Destroy(child.gameObject);
        }
        foreach (Transform child in bulletSet.transform)
        {
            Destroy(child.gameObject);
        }
        ememyCount = 0;
        StartCoroutine(SpawnEnemyCoroutine());
    }

    public void GameOver()
    {
        gameOverPanel.SetActive(true);
        Time.timeScale = 0f;
        timeSliceRank = timeSliceRank < timeSlice ? timeSlice : timeSliceRank;
        isStart = false;
        timeSliceUI.GetComponent<TextMeshProUGUI>().text = "总生存时长：" + timeSlice;
        timeSliceTotalUI.GetComponent<TextMeshProUGUI>().text = "历史最佳：" + timeSliceRank;
        circleFieldVisual.SetActive(false);
    }

    public void BackHome()
    {
        Time.timeScale = 1f;
        gameUI.SetActive(false);
        gameOverPanel.SetActive(false);
        player.GetComponent<PlayerAttribute>().HP = player.GetComponent<PlayerAttribute>().MAXHP;
        player.GetComponent<PlayerMove>().canChangeState = false;
        player.GetComponent<PlayerMove>().animator.speed = 1f;
        player.GetComponent<PlayerMove>().shootCoolDown = player.GetComponent<PlayerMove>().initShootCoolDown;
        player.GetComponent<PlayerAttribute>().ATK = player.GetComponent<PlayerAttribute>().initATK;
        player.GetComponent<PlayerAttribute>().MAXMP = player.GetComponent<PlayerAttribute>().initMP;
        player.GetComponent<PlayerAttribute>().Level = 0;
        player.GetComponent<PlayerAttribute>().ToNextLevelEXP = 0;
        player.GetComponent<PlayerAttribute>().MP = player.GetComponent<PlayerAttribute>().initMP;
        player.GetComponent<PlayerAttribute>().endurance = player.GetComponent<PlayerAttribute>().initEndurance;
        player.GetComponent<PlayerAttribute>().enduranceMAX = player.GetComponent<PlayerAttribute>().initEndurance;
        groundStartTime = Time.time;
        groundTotalTime = 20f;
        globalTime = 0f;
        gameStartTime = Time.time;
        groundTime = groundTotalTime;
        timeSlice = 0;
        circleField.transform.position = Vector2.zero;
        circleField.transform.localScale = circleFieldInitLocalScale;
        enemyHP = 5;
        ememyCount = 0;
        StopSound(audioSource2);
        PlaySound(audioSource1, "mainPageBGM");
        foreach (Transform child in enemySet.transform)
        {
            Destroy(child.gameObject);
        }
        foreach (Transform child in bulletSet.transform)
        {
            Destroy(child.gameObject);
        }


        mainPage.SetActive(true);
        player.transform.position = new Vector2(60, 0);
        mainCamera.GetComponent<CameraFollow>().isZoomIn = false;
        mainCamera.transform.position = new Vector3(0, 0, -10);
    }
}
