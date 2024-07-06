using System.Collections;
using UnityEngine;

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

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        duration = globalManager.GetComponent<GlobalManager>().groundTotalTime;
        initTransform = rectTransform.anchoredPosition;
        player = player = GameObject.FindGameObjectWithTag("Player");
        
    }

    void Start()
    {
        player.GetComponent<PlayerMove>().OnPlayerChangeState += ResetMove;
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
            float ratio = elapsedTime / duration;

            Vector3 position = rectTransform.anchoredPosition;
            position.x = Mathf.Lerp(start_x, end_x, ratio);
            rectTransform.anchoredPosition = position;

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
