using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class ui_mode : MonoBehaviour
{
    // default mode magic
    public Sprite move;
    public Sprite magic;
    public Image image;

    bool is_move = true;
    private GameObject player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        var player_move = player.GetComponent<PlayerMove>();
        player_move.OnPlayerChangeState += on_switch_mode;
    }

    void on_switch_mode(PlayerMove who) {
        switch_mode();
    }

    void Update()
    {
        if(player.GetComponent<PlayerMove>().playerState == PlayerState.infiniteMove)
        {
            image.sprite = move;
        }
        else
        {
            image.sprite = magic;
        }
    }

    void switch_mode() { 
        if(is_move) {
            image.sprite = magic;
        }
        else {
            image.sprite = move;
        }
        is_move = !is_move; 
    }
}
