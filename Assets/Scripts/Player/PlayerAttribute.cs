using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttribute : MonoBehaviour
{
    public int HP = 10;
    public int MAXHP = 10;
    public int MP = 10;
    public int MAXMP = 10;
    public int ATK = 10;
    public bool isInCircleField = true;
    public GameObject circleField;
    public float circleFieldAttackInterval = 1f;
    public float underAttackTime = 0f;
    void Start()
    {

    }


    void Update()
    {
        if (!isInCircleField)
        {
            if (Time.time - underAttackTime > circleFieldAttackInterval)
            {
                ChangeHP(-circleField.GetComponent<CircleField>().ATK);
                underAttackTime = Time.time;
            }

        }
    }

    public void ChangeHP(int value)
    {
        HP += value;
        if (HP < 0)
        {
            HP = 0;
        }
        if (HP > MAXHP)
        {
            HP = MAXHP;
        }

    }
}