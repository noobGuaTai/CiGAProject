using System.Collections;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    public GameObject player;
    public float speed = 100f;
    public int HP = 5;
    public GameObject potionPrefab;
    public float potionSpawnChance = 0.1f;

    private bool canAttack = true;

    void Start()
    {

    }

    void Update()
    {
        MoveToPlayer();

        if(HP <= 0)
        {
            if(Random.value <= potionSpawnChance)
            {
                Instantiate(potionPrefab, transform.position, Quaternion.identity);
            }
            Destroy(gameObject);
        }
    }

    void MoveToPlayer()
    {
        if (player != null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
            Vector3 direction = (player.transform.position - transform.position).normalized;
            Vector3 moveAmount = direction * speed * Time.deltaTime;
            transform.position += moveAmount;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(Attack(other));
        }
    }

    private IEnumerator Attack(Collider2D player)
    {
        canAttack = false;
        player.GetComponent<PlayerAttribute>().ChangeHP(-1);
        yield return new WaitForSeconds(1f);
        canAttack = true;
    }
    public void ChangeHP(int value)
    {
        HP += value;
        if (HP <= 0)
        {
            HP = 0;
        }
    }
}
