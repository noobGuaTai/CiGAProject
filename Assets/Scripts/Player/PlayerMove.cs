using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState
{
    infiniteMove,
    infiniteAttack,
}

public class PlayerMove : MonoBehaviour
{
    public float moveSpeed = 75f;
    public float initMoveSpeed = 75f;
    public PlayerState playerState;
    public GameObject bulletPrefab;
    public float bulletSpeed = 20f;
    public float shootCoolDown = 1f;
    public float initShootCoolDown = 1f;
    public bool canChangeState = false;
    public GameObject globalManager;
    public GameObject bulletSet;
    public GameObject potion;

    private Rigidbody2D rb;
    private Vector2 moveInput;
    private PlayerAttribute playerAttribute;
    private float shootTimer = 0f;
    public Animator animator;

    public delegate void OnPlayerChangeStateType(PlayerMove who);
    public OnPlayerChangeStateType OnPlayerChangeState;
    private Animator changeStateAnimator;
    private Animator smokeAnimator;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        playerAttribute = GetComponent<PlayerAttribute>();
        playerState = PlayerState.infiniteMove;
        changeStateAnimator = transform.Find("ChangeState").GetComponent<Animator>();
        smokeAnimator = transform.Find("SpawnEXP").GetComponent<Animator>();
    }


    void Update()
    {
        if (globalManager.GetComponent<GlobalManager>().isStart)
        {
            moveInput.x = Input.GetAxisRaw("Horizontal");
            moveInput.y = Input.GetAxisRaw("Vertical");
            moveInput.Normalize();
            if (playerState == PlayerState.infiniteMove)
            {
                InfiniteMove();
                playerAttribute.endurance = playerAttribute.enduranceMAX;
            }
            else
            {
                FiniteMove();
                playerAttribute.MP = playerAttribute.MAXMP;
            }

            if (Input.GetMouseButton(0) && shootTimer <= 0 && playerAttribute.MP >= 1)
            {
                Shoot();
                shootTimer = shootCoolDown;
                playerAttribute.MP -= 1;
            }

            if (Input.GetMouseButtonDown(0) && playerAttribute.MP == 0)
            {
                globalManager.GetComponent<GlobalManager>().PlaySound(globalManager.GetComponent<GlobalManager>().audioSource3, "BulletOverShoot");
            }
            if (shootTimer > 0)
            {
                shootTimer -= Time.deltaTime;
            }
            ChangeState();
            animator.speed = moveSpeed / initMoveSpeed;
        }

    }

    void InfiniteMove()
    {
        rb.MovePosition(rb.position + moveInput * moveSpeed * Time.fixedDeltaTime);
        bool isMoving = moveInput.x != 0 || moveInput.y != 0;
        if (isMoving)
        {
            transform.localScale = new Vector3(Mathf.Sign(moveInput.x) * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
    }

    void FiniteMove()
    {
        if (playerAttribute.endurance == 0)
        {
            moveSpeed = 20;
        }
        else
        {

        }
        Vector2 previousPosition = rb.position;
        Vector2 newPosition = previousPosition + moveInput * moveSpeed * Time.fixedDeltaTime;
        float distance = Vector2.Distance(previousPosition, newPosition);
        if (playerAttribute.endurance < distance)
        {
            rb.velocity = Vector2.zero;
            return;
        }
        rb.MovePosition(newPosition);
        playerAttribute.endurance -= distance;

        bool isMoving = moveInput.x != 0 || moveInput.y != 0;
        if (isMoving)
        {
            transform.localScale = new Vector3(Mathf.Sign(moveInput.x) * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }

    }

    void Shoot()
    {
        if (playerAttribute.MP > 3)
        {
            globalManager.GetComponent<GlobalManager>().PlaySound(globalManager.GetComponent<GlobalManager>().audioSource3, "BulletNormal");
        }
        else
        {
            globalManager.GetComponent<GlobalManager>().PlaySound(globalManager.GetComponent<GlobalManager>().audioSource3, "BulletLess");
        }
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;
        Vector3 shootDirection = (mousePosition - transform.position).normalized;
        GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        bullet.transform.SetParent(bulletSet.transform);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.velocity = shootDirection * bulletSpeed;
    }

    public void ChangeState()
    {
        if (Input.GetButtonDown("ChangeState") && canChangeState)
        {
            smokeAnimator.Play("smoke");
            if (playerState == PlayerState.infiniteMove)
            {
                shootCoolDown = shootCoolDown * 0.7f;
                moveSpeed = moveSpeed / 1.5f;
                playerState = PlayerState.infiniteAttack;
                changeStateAnimator.Play("MoveChange");
                animator.SetBool("attack", true);
            }
            else
            {
                shootCoolDown = shootCoolDown / 0.7f;
                moveSpeed = moveSpeed * 1.5f;
                playerState = PlayerState.infiniteMove;
                changeStateAnimator.Play("AttackChange");
                animator.SetBool("attack", false);
            }
            globalManager.GetComponent<GlobalManager>().ReStartNextTimeSlice();
            OnPlayerChangeState.Invoke(this);
            for (int i = 0; i < 3; i++)
            {
                Vector3 randomPosition = GetRandomPointInCircle(transform.position, 50f);
                Instantiate(potion, randomPosition, Quaternion.identity);
            }
        }
    }

    Vector3 GetRandomPointInCircle(Vector3 center, float radius)
    {
        float angle = Random.Range(0f, Mathf.PI * 2);
        float distance = Random.Range(40f, radius);
        Vector3 randomPosition = new Vector3(
            center.x + Mathf.Cos(angle) * distance,
            center.y,
            center.z + Mathf.Sin(angle) * distance
        );
        return randomPosition;
    }
}
