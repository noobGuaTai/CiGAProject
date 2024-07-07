using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;

public class PlayerAttribute : MonoBehaviour
{
    public int HP = 10;
    public int MAXHP = 10;
    public int MP = 10;
    public int MAXMP = 10;
    public int initMP = 10;
    public int ATK = 4;
    public int initATK = 4;
    public int EXP = 0;
    public int Level = 0;
    public int ToNextLevelEXP = 0;

    public float endurance = 1200f;
    public float enduranceMAX = 1200f;
    public float initEndurance = 1200f;
    public bool isInCircleField = true;
    public GameObject circleField;
    public float circleFieldAttackInterval = 1f;
    public float underAttackTime = 0f;
    public GameObject globalManager;
    public float underAttackInterval = 1f;
    public GameObject levelUI;

    private float lastUnderAttackTime = 0f;
    private Animator levelUpAnimator;

    public delegate void OnGainExpType(PlayerAttribute who);
    public OnGainExpType on_gain_exp;

    private void Start()
    {
        ToNextLevelEXP = get_next_level_exp(Level);
        levelUpAnimator = transform.Find("LevelUp").GetComponent<Animator>();
    }

    int get_next_level_exp(int level)
    {
        return (int)math.pow(level, 1.5f) + level;
    }

    public void gain_exp()
    {
        EXP += 1;
        if (EXP >= ToNextLevelEXP)
        {
            EXP -= ToNextLevelEXP;
            Level += 1;
            ToNextLevelEXP = get_next_level_exp(Level);
            MAXMP += 1;
            enduranceMAX += 100;
            if (GetComponent<PlayerMove>().shootCoolDown > 0.2f)
                GetComponent<PlayerMove>().shootCoolDown -= 0.1f;
            else if (Level % 3 == 0)
                ATK += 1;
            GetComponent<PlayerMove>().moveSpeed += 3;
            levelUpAnimator.Play("LevelUp");
            globalManager.GetComponent<GlobalManager>().PlaySound(globalManager.GetComponent<GlobalManager>().audioSource5, "LevelUp");
        }
        on_gain_exp.Invoke(this);
    }

    void Update()
    {
        levelUI.GetComponent<TextMeshProUGUI>().text = Level.ToString();
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
