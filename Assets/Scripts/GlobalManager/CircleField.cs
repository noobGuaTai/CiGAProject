using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleField : MonoBehaviour
{
    public GameObject player;
    public Transform[] movePoints;
    public float moveSpeed = 5f;
    public int ATK = 1;
    private int currentPointIndex;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if (movePoints.Length > 0)
        {
            // currentPointIndex = Random.Range(0, movePoints.Length);
            currentPointIndex = 0;
            transform.position = movePoints[currentPointIndex].position;
        }
    }

    void Update()
    {
        if (movePoints.Length > 0)
        {
            MoveToNextPoint();
        }
    }

    void MoveToNextPoint()
    {
        Transform targetPoint = movePoints[currentPointIndex];
        transform.position = Vector3.MoveTowards(transform.position, targetPoint.position, moveSpeed * Time.deltaTime);
        if (Vector3.Distance(transform.position, targetPoint.position) < 0.1f)
        {
            int newPointIndex;
            do
            {
                newPointIndex = Random.Range(0, movePoints.Length);
            } while (newPointIndex == currentPointIndex);

            currentPointIndex = newPointIndex;
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
