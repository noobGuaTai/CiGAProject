using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ui_statusbar : MonoBehaviour
{
    Color magic_color;
    Color magic_light_color;
    Color move_color;
    Color move_light_color;
    Image hp;
    Image mp;
    RectTransform hp_box;
    RectTransform mp_box;

    private GameObject player;
    bool is_move = false;

    private void Start() {
        hp = transform.Find("hp").GetComponent<Image>();
        mp = transform.Find("mp").GetComponent<Image>(); 
        hp_box = transform.Find("hp_box").GetComponent<RectTransform>();
        mp_box = transform.Find("mp_box").GetComponent<RectTransform>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void switch_mode() {
        if (is_move) {
            mp.material.SetColor("hp_base_color", magic_color);
            mp.material.SetColor("hp_light_color", magic_light_color);
        }
        else {
            mp.material.SetColor("hp_base_color", move_color);
            mp.material.SetColor("hp_light_color", move_light_color);
        }
        is_move = !is_move;
    }

    void update_ratio(float hp_ratio, float mp_ratio) {
        hp.material.SetFloat("hp_ratio", hp_ratio);
        mp.material.SetFloat("hp_ratio", mp_ratio);
    }

    void update_pos(Image hp, RectTransform hp_box) {
        float bx_min = hp_box.rect.xMin;
        float bx_max = hp_box.rect.xMax;
        float bx_min_world = (hp_box.localToWorldMatrix * new Vector4(bx_min, 0, 0, 1)).x;
        float bx_max_world = (hp_box.localToWorldMatrix * new Vector4(bx_max, 0, 0, 1)).x;


        hp.material.SetFloat("_hp_min", bx_min_world);
        hp.material.SetFloat("_hp_max", bx_max_world);
    }

    private void Update() {
        update_ratio(player.GetComponent<PlayerAttribute>().HP, player.GetComponent<PlayerAttribute>().MP);
        update_pos(hp, hp_box);
        update_pos(mp, mp_box);
    }
}
