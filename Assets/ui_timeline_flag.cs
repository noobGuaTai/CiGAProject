using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ui_timeline_flag : MonoBehaviour
{
    public GameObject player;
    public float end_x;
    public float start_x;
    public float duration = 20f;
    public GameObject globalManager;

    private RectTransform rectTransform;
    private Vector3 initTransform;
    private Coroutine moveRightCoroutine;
    public Tween tween;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        duration = globalManager.GetComponent<GlobalManager>().groundTotalTime;
        initTransform = rectTransform.anchoredPosition;
        player = player = GameObject.FindGameObjectWithTag("Player");
        
    }

    void Start()
    {
        GetComponent<Image>().material.SetFloat("_white_ratio", 0);
        player.GetComponent<PlayerMove>().OnPlayerChangeState += ResetMove;
        tween = gameObject.AddComponent<Tween>();
        for (int i = 0; i < 5; i++) {
            tween.AddTween<float>((float a) => {
                GetComponent<Image>().material.SetFloat("_white_ratio", a);
            }, 1, 0, 1f, Tween.TransitionType.QUART, Tween.EaseType.OUT);
        }
        tween.clearWhenEnd = false;
        var gm = globalManager.GetComponent<GlobalManager>();
        gm.on_last_5_begin += () => {
            tween.Play();
        };
        gm.on_enter_next_timeslice += () => {
            tween.Stop();
        };
    }

    void on_last_5_begin() {
        tween.Play();
    }

    void on_enter_next_timeslice() {
        tween.Stop();
    }

    void Update()
    {
        
    }

    public void StartMove()
    {
        moveRightCoroutine = StartCoroutine(MoveRight());
    }

    public void ResetMove(PlayerMove pm)
    {
        // StopCoroutine(moveRightCoroutine);
        StopAllCoroutines();
        StartCoroutine(ResetMoveCoroutine());
    }

    public void ResetMove()
    {
        // StopCoroutine(moveRightCoroutine);
        StopAllCoroutines();
        StartCoroutine(ResetMoveCoroutine());
    }

    IEnumerator MoveRight()
    {
        float elapsedTime = 0f;
        rectTransform.anchoredPosition = initTransform;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float ratio = (20 - globalManager.GetComponent<GlobalManager>().groundTime) / duration;

            Vector3 position = rectTransform.anchoredPosition;
            position.x = Mathf.Lerp(start_x, end_x, ratio);
            rectTransform.anchoredPosition = position;
            if((position.x - start_x) / (end_x - start_x) >= 0.75f)
            {
                transform.localScale = new Vector3(2f, 2f, 1f);
            }
            else
            {
                transform.localScale = new Vector3(1f, 1f, 1f);
            }

            yield return null;
        }

        // Vector3 finalPosition = rectTransform.anchoredPosition;
        // finalPosition.x = end_x;
        // rectTransform.anchoredPosition = finalPosition;
    }

    IEnumerator ResetMoveCoroutine()
    {
        float elapsedTime = 0f;
        Vector3 currentPosition = rectTransform.anchoredPosition;

        while (elapsedTime < 1)
        {
            elapsedTime += Time.deltaTime;
            float ratio = elapsedTime / 1;

            Vector3 position = currentPosition;
            position.x = Mathf.Lerp(currentPosition.x, start_x, ratio);
            rectTransform.anchoredPosition = position;

            yield return null;
        }
        StartCoroutine(MoveRight());
    }
}
