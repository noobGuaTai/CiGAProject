using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleField : MonoBehaviour
{
    public GameObject player;
    public GameObject globalManager;
    public float topDownBorder = 200f;
    public float leftRightBorder = 300f;
    public float moveDistance = 50f;
    public float moveDuration = 20f; // 持续时间
    public int ATK = 1;
    public Vector3 targetPoint;
    private Coroutine moveCoroutine;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        globalManager = GameObject.FindGameObjectWithTag("GlobalManager");
    }

    public void StartMove()
    {
        SelectRandomTargetPoint();
        StartMoving();
    }

    void Update()
    {
        if (globalManager.GetComponent<GlobalManager>().isStart && Vector3.Distance(transform.position, targetPoint) < 0.1f)
        {
            SelectRandomTargetPoint();
            StartMoving();
        }
    }

    void SelectRandomTargetPoint()
    {
        Vector2 randomDirection = Random.insideUnitCircle.normalized;
        Vector3 potentialTarget = transform.position + (Vector3)(randomDirection * moveDistance);


        potentialTarget.x = Mathf.Clamp(potentialTarget.x, -leftRightBorder, leftRightBorder);
        potentialTarget.y = Mathf.Clamp(potentialTarget.y, -topDownBorder, topDownBorder);

        targetPoint = potentialTarget;
    }

    void StartMoving()
    {
        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
        }
        moveCoroutine = StartCoroutine(MoveOverTime(targetPoint, moveDuration));
    }

    IEnumerator MoveOverTime(Vector3 destination, float duration)
    {
        Vector3 start = transform.position;
        float elapsedTime = 0f;
        
        while (elapsedTime < duration && globalManager.GetComponent<GlobalManager>().isStart)
        {
            elapsedTime += Time.deltaTime;
            float ratio = elapsedTime / duration;
            transform.position = Vector3.Lerp(start, destination, ratio);
            yield return null;
        }
        if(globalManager.GetComponent<GlobalManager>().isStart)
            transform.position = destination;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            player.GetComponent<PlayerAttribute>().isInCircleField = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            player.GetComponent<PlayerAttribute>().isInCircleField = false;
        }
    }
}
