using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ui_statusbar : MonoBehaviour
{
    public Color magic_color;
    public Color magic_light_color;
    public Color move_color;
    public Color move_light_color;
    Image hp;
    Image mp;
    RectTransform hp_box;
    RectTransform mp_box;

    private GameObject player;
    private PlayerMove playerMove;
    private PlayerAttribute playerAttribute;
    public bool is_move = true;
    float cnt_hp_ratio = 1.0f;
    float tar_hp_ratio = 1.0f;
    Image exp_image;
    Tween tween;
    Tween tween2;
    Tween tween_exp;

    void Start()
    {
        tween = GetComponent<Tween>();
        tween2 = gameObject.AddComponent<Tween>();
        tween_exp = gameObject.AddComponent<Tween>();
        hp = transform.Find("hp").GetComponent<Image>();
        mp = transform.Find("mp").GetComponent<Image>();
        player = GameObject.FindGameObjectWithTag("Player");
        playerMove = player.GetComponent<PlayerMove>();
        playerAttribute = player.GetComponent<PlayerAttribute>();
        exp_image = transform.Find("exp").GetComponent<Image>();
        var player_move = player.GetComponent<PlayerMove>();
        player_move.OnPlayerChangeState += on_switch_mode;
        hp.material.SetFloat("_hp_ratio", 1f);
        mp.material.SetFloat("_hp_ratio", 1f);
        hp.material.SetFloat("_white_ratio", 0f);
        mp.material.SetFloat("_white_ratio", 0f);
        mp.material.SetColor("_hp_base_color", magic_color);
        mp.material.SetColor("_hp_light_color", magic_light_color);

        playerAttribute.on_gain_exp += on_gain_exp;
    }

    void on_gain_exp(PlayerAttribute who)
    {
        var rr = 1.0f * who.EXP / who.ToNextLevelEXP;
        update_exp(rr);
    }
    void on_switch_mode(PlayerMove who)
    {
        var cnt_state = who.playerState;
        switch_mode();
    }

    void switch_mode()
    {
        if (is_move)
        {
            mp.material.SetColor("_hp_base_color", move_color);
            mp.material.SetColor("_hp_light_color", move_light_color);
        }
        else
        {
            mp.material.SetColor("_hp_base_color", magic_color);
            mp.material.SetColor("_hp_light_color", magic_light_color);
        }
        is_move = !is_move;
    }

    void update_ratio(float hp_ratio, float mp_ratio)
    {
        const float ttime = 0.25f;
        if (hp_ratio != tar_hp_ratio)
        {
            tar_hp_ratio = hp_ratio;
            tween.Clear();
            tween2.Clear();
            tween.AddTween<float>((float a) =>
            {
                hp.material.SetFloat("_hp_ratio", a);
                cnt_hp_ratio = a;
            }, cnt_hp_ratio, hp_ratio, ttime, Tween.TransitionType.QUAD, Tween.EaseType.OUT);
            tween2.AddTween<float>((float a) =>
            {
                hp.material.SetFloat("_white_ratio", a);
            }, 1, 0, ttime, Tween.TransitionType.QUAD, Tween.EaseType.OUT);
            tween.Play();
            tween2.Play();
        }
        mp.material.SetFloat("_hp_ratio", mp_ratio);
    }

    void update_pos(Image hp, RectTransform hp_box)
    {
        float bx_min = hp_box.rect.xMin;
        float bx_max = hp_box.rect.xMax;
        float bx_min_world = (hp_box.localToWorldMatrix * new Vector4(bx_min, 0, 0, 1)).x;
        float bx_max_world = (hp_box.localToWorldMatrix * new Vector4(bx_max, 0, 0, 1)).x;


        //hp.material.SetFloat("_hp_min", bx_min_world);
        //hp.material.SetFloat("_hp_max", bx_max_world);
    }


    void Update()
    {
        update_ratio(
            playerAttribute.HP / (float)playerAttribute.MAXHP,
            playerMove.playerState == PlayerState.infiniteAttack ?
            playerAttribute.endurance / (float)playerAttribute.enduranceMAX :
            playerAttribute.MP / (float)playerAttribute.MAXMP);

        //update_pos(hp, hp_box);
        //update_pos(mp, mp_box);
    }


    float tar_exp_ratio;


    void update_exp(float exp_ratio)
    {
        if (exp_ratio != tar_exp_ratio)
        {
            tween_exp.Clear();
            tween_exp.AddTween<float>((float a) =>
            {
                exp_image.fillAmount = a;
            }, tar_exp_ratio, exp_ratio, 0.1f);
            tar_exp_ratio = exp_ratio;
            tween_exp.Play();
        }
    }
}
