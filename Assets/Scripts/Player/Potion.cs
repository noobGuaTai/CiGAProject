using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potion : MonoBehaviour
{
    public GameObject player;
    public GameObject globalManager;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        globalManager = GameObject.FindGameObjectWithTag("GlobalManager");
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            globalManager.GetComponent<GlobalManager>().PlaySound(globalManager.GetComponent<GlobalManager>().audioSource4, "PickOption");
            other.GetComponent<PlayerAttribute>().ATK += 1;
            Destroy(gameObject);
        }
    }

}
