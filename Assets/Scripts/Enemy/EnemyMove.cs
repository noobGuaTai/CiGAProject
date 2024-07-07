using System.Collections;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    public GameObject player;
    public float speed = 100f;
    public int HP = 5;
    public GameObject potionPrefab;
    public GlobalManager globalManager;
    public float potionSpawnChance = 0.1f;
    public float attackInterval = 1f;
    public float disappearDistance = 300f;
    public float dashTime = 10f;// 如果存活超过10s，则加速，攻击到玩家后消失

    private float lastAttackTime = 0f;
    private float startTime = 0f;
    private bool isDashing = false;


    Tween tween;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        tween = gameObject.AddComponent<Tween>();
        startTime = Time.time;
    }

    void Update()
    {
        MoveToPlayer();

        if (HP <= 0)
        {
            if (Random.value <= potionSpawnChance)
            {
                GameObject potion = Instantiate(potionPrefab, transform.position, Quaternion.identity);
                potion.GetComponent<Potion>().Drop(transform.position);
            }
            // globalManager.on_enemy_dead(this);
            Destroy(gameObject);
            return;
        }

        var ofv = player.transform.position - transform.position;
        var dis = ofv.x * ofv.x + ofv.y * ofv.y + ofv.z * ofv.z;
        if (dis > disappearDistance * disappearDistance)
        {
            // globalManager.on_enemy_dead(this);
            Destroy(gameObject);
        }

        if(Time.time - startTime > dashTime && !isDashing)
        {
            isDashing = true;
            speed = speed * 1.5f;
            GetComponent<SpriteRenderer>().color = Color.HSVToRGB(0, 50, 100);
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

            float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
            if (distanceToPlayer > 1f)
            {
                if (direction.x > 0)
                {
                    transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
                }
                else
                {
                    transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
                }
            }
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player") && Time.time - lastAttackTime > attackInterval)
        {
            player.GetComponent<PlayerAttribute>().ChangeHP(-1);
            lastAttackTime = Time.time;
            if(isDashing)
                Destroy(gameObject);
        }
    }

    public void ChangeHP(int value)
    {
        HP += value;
        if (HP <= 0)
        {
            HP = 0;
        }
        else
        {
            tween.Clear();
            tween.AddTween((float a) =>
            {
                GetComponent<SpriteRenderer>().material.SetFloat("_white_ratio", a);
            }, 1, 0, 0.3f, Tween.TransitionType.QUAD, Tween.EaseType.OUT);
            tween.Play();
        }
    }
}
