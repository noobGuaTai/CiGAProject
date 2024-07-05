using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleField : MonoBehaviour
{
    public GameObject player;
    public float topDownBorder = 200f;
    public float leftRightBorder = 300f;
    public float moveSpeed = 5f;
    public float moveDistance = 50f;
    public int ATK = 1;
    private Vector3 targetPoint;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        SelectRandomTargetPoint();
    }

    void Update()
    {
        MoveToTargetPoint();
    }

    void SelectRandomTargetPoint()
    {
        Vector2 randomDirection = Random.insideUnitCircle.normalized; // 随机方向
        Vector3 potentialTarget = transform.position + (Vector3)(randomDirection * moveDistance);

        // 确保目标点在边界内并与边界保持一定距离
        potentialTarget.x = Mathf.Clamp(potentialTarget.x, -leftRightBorder, leftRightBorder);
        potentialTarget.y = Mathf.Clamp(potentialTarget.y, -topDownBorder, topDownBorder);

        targetPoint = potentialTarget;
    }

    void MoveToTargetPoint()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPoint, moveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetPoint) < 0.1f)
        {
            SelectRandomTargetPoint();
        }
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
