using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;

public class CircleField : MonoBehaviour
{
    public GameObject player;
    public GameObject globalManager;
    public float topDownBorder = 300f;
    public float leftRightBorder = 400f;
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
        bool foundValidPoint = false;
        for (int i = 0; i < 10; i++)
        {
            Vector2 randomDirection = Random.insideUnitCircle.normalized;
            Vector3 potentialTarget = transform.position + (Vector3)(randomDirection * moveDistance);

            potentialTarget.x = Mathf.Clamp(potentialTarget.x, -leftRightBorder, leftRightBorder);
            potentialTarget.y = Mathf.Clamp(potentialTarget.y, -topDownBorder, topDownBorder);

            if (Vector3.Distance(transform.position, potentialTarget) >= moveDistance)
            {
                targetPoint = potentialTarget;
                foundValidPoint = true;
                break;
            }
        }

        if (!foundValidPoint)
        {
            Vector2 randomDirection = Random.insideUnitCircle.normalized;
            Vector3 potentialTarget = transform.position + (Vector3)(randomDirection * moveDistance);

            potentialTarget.x = Mathf.Clamp(potentialTarget.x, -leftRightBorder, leftRightBorder);
            potentialTarget.y = Mathf.Clamp(potentialTarget.y, -topDownBorder, topDownBorder);

            targetPoint = potentialTarget;
        }
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
        if (globalManager.GetComponent<GlobalManager>().isStart)
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
