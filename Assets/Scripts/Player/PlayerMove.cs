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
        if (playerState == PlayerState.infiniteMove)
        {
            InfiniteMove();
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
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");
        moveInput.Normalize();
        rb.MovePosition(rb.position + moveInput * moveSpeed * Time.fixedDeltaTime);
        bool isMoving = moveInput.x != 0 || moveInput.y != 0;
        if (isMoving)
        {
            transform.localScale = new Vector3(Mathf.Sign(moveInput.x) * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
    }

    void FiniteMove()
    {
        moveInput.x = 0;
        moveInput.y = 0;
        if (Input.GetKey(KeyCode.W) && (selectedDirection1 == Vector2.up || selectedDirection2 == Vector2.up))
        {
            moveInput.y = 1;
        }
        if (Input.GetKey(KeyCode.S) && (selectedDirection1 == Vector2.down || selectedDirection2 == Vector2.down))
        {
            moveInput.y = -1;
        }
        if (Input.GetKey(KeyCode.A) && (selectedDirection1 == Vector2.left || selectedDirection2 == Vector2.left))
        {
            moveInput.x = -1;
        }
        if (Input.GetKey(KeyCode.D) && (selectedDirection1 == Vector2.right || selectedDirection2 == Vector2.right))
        {
            moveInput.x = 1;
        }
        moveInput.Normalize();
        rb.MovePosition(rb.position + moveInput * moveSpeed * Time.fixedDeltaTime);
        bool isMoving = moveInput.x != 0 || moveInput.y != 0;
        if (isMoving)
        {
            transform.localScale = new Vector3(Mathf.Sign(moveInput.x) * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
    }

    void SelectRandomDirections()
    {
        int index1 = Random.Range(0, possibleDirections.Length);
        int index2;
        do
        {
            index2 = Random.Range(0, possibleDirections.Length);
        } while (index2 == index1);

        selectedDirection1 = possibleDirections[index1];
        selectedDirection2 = possibleDirections[index2];
    }

    void Shoot()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;
        Vector3 shootDirection = (mousePosition - transform.position).normalized;
        GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.velocity = shootDirection * bulletSpeed;
    }

    public void ChangeState()
    {
        if (Input.GetButtonDown("ChangeState"))
        {
            if (playerState == PlayerState.infiniteMove)
            {
                SelectRandomDirections();
                playerState = PlayerState.infiniteAttack;
            }
            else
            {
                playerState = PlayerState.infiniteMove;
            }
                
        }
    }

}
