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
    public int EXP = 0;
    
    public float endurance = 1000f;
    public float enduranceMAX = 1000f;
    public bool isInCircleField = true;
    public GameObject circleField;
    public float circleFieldAttackInterval = 1f;
    public float underAttackTime = 0f;
    public GameObject globalManager;
    public float underAttackInterval = 1f;

    private float lastUnderAttackTime = 0f;

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
        if (Time.time - lastUnderAttackTime > underAttackInterval)
        {
            lastUnderAttackTime = Time.time;
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
}
