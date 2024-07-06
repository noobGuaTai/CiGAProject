using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potion : MonoBehaviour
{
    public GameObject player;
    public GameObject globalManager;
    public Tween tween;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        globalManager = GameObject.FindGameObjectWithTag("GlobalManager");
        Destroy(gameObject, 8f);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            globalManager.GetComponent<GlobalManager>().PlaySound(globalManager.GetComponent<GlobalManager>().audioSource4, "PickOption");
            //other.GetComponent<PlayerAttribute>().ATK += 1;
            player.GetComponent<PlayerAttribute>().gain_exp();
            Destroy(gameObject);
        }
    }

    public void Drop(Vector3 position, float dropRadius = 16f, float collectDelay = 0.6f)
    {
        if (tween != null)
        {
            var randomAngle = UnityEngine.Random.Range(0, 2 * Mathf.PI);
            var randomVec3 = new Vector3(Mathf.Cos(randomAngle), Mathf.Sin(randomAngle), 0);
            var targetPos = transform.position + randomVec3 * dropRadius;

            tween.Clear();
            tween.AddTween(_DropProcess, transform.position, targetPos, collectDelay, Tween.TransitionType.QUAD, Tween.EaseType.OUT);
            tween.AddTween(_DropCollect, 0f, 0f, 0);
            tween.Play();
        }

    }

    public void _DropProcess(Vector3 position)
    {
        transform.position = position;
    }

    public void _DropCollect(float _)
    {
        
    }
}
