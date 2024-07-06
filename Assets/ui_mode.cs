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
    bool is_move = false;
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
