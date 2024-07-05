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
    public float moveSpeed = 200f;
    public PlayerState playerState;
    public GameObject bulletPrefab;
    public float bulletSpeed = 20f;
    public float shootCoolDown = 0.2f;
    public bool canChangeState = false;
    public GameObject globalManager;
    public GameObject bulletSet;

    private Rigidbody2D rb;
    private Vector2 moveInput;
    private PlayerAttribute playerAttribute;
    private float shootTimer;
    private Vector2[] possibleDirections = { Vector2.up, Vector2.down, Vector2.left, Vector2.right };
    private Vector2 selectedDirection1;
    private Vector2 selectedDirection2;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerAttribute = GetComponent<PlayerAttribute>();
        playerState = PlayerState.infiniteMove;
    }


    void Update()
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
            playerAttribute.MP = 10;
        }

        if (Input.GetMouseButton(0) && shootTimer <= 0 && playerAttribute.MP >= 1)
        {
            Shoot();
            shootTimer = shootCoolDown;
            playerAttribute.MP -= 1;
        }
        if (shootTimer > 0)
        {
            shootTimer -= Time.deltaTime;
        }
        ChangeState();
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
        if (playerAttribute.endurance > 0)
        {
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
    }

    void Shoot()
    {
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
            if (playerState == PlayerState.infiniteMove)
            {
                playerState = PlayerState.infiniteAttack;
            }
            else
            {
                playerState = PlayerState.infiniteMove;
            }
            globalManager.GetComponent<GlobalManager>().groundStartTime = Time.time;
            globalManager.GetComponent<GlobalManager>().timeSlice += 1;
        }
    }

}
