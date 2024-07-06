using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public GameObject player;
    private Animator animator;
    private Collider2D collider2d;
    private Rigidbody2D rb;
    private bool isExploding = false;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        collider2d = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy") && !isExploding)
        {
            isExploding = true;
            other.GetComponent<EnemyMove>().ChangeHP(-player.GetComponent<PlayerAttribute>().ATK);
            rb.velocity = Vector2.zero;
            rb.isKinematic = true;
            collider2d.enabled = false;
            animator.Play("Boom");
        }
    }

    void Update()
    {
        if (isExploding)
        {
            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
            if (stateInfo.IsName("Boom"))
            {
                float normalizedTime = stateInfo.normalizedTime;
                if (normalizedTime >= 1.0f)
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}
