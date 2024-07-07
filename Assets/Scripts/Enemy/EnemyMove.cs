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

    private float lastAttackTime = 0f;


    Tween tween;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        tween = gameObject.AddComponent<Tween>();

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
            globalManager.on_enemy_dead(this);
            Destroy(gameObject);
            return;
        }

        var ofv = player.transform.position - transform.position;
        var dis = ofv.x * ofv.x + ofv.y * ofv.y + ofv.z *  ofv.z;
        if (dis > disappearDistance * disappearDistance) {
            globalManager.on_enemy_dead(this);
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

            if ((player.transform.position - transform.position).x > 0)
            {
                transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
            else
            {
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player") && Time.time - lastAttackTime > attackInterval)
        {
            player.GetComponent<PlayerAttribute>().ChangeHP(-1);
            lastAttackTime = Time.time;
        }
    }

    public void ChangeHP(int value)
    {
        HP += value;
        if (HP <= 0) {
            HP = 0;
        }
        else { 
            tween.Clear();
            tween.AddTween((float a) => {
                GetComponent<SpriteRenderer>().material.SetFloat("_white_ratio", a);
            }, 1, 0, 0.3f, Tween.TransitionType.QUAD, Tween.EaseType.OUT);
            tween.Play();
        }
    }
}
