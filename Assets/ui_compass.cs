using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class ui_compass : MonoBehaviour
{
    Image image;
    CircleField circleField;
    GameObject player;

    private void Start() {
        image = GetComponent<Image>();
        var gm = transform.Find("/Root/GlobalManager").GetComponent<GlobalManager>();
        player = GameObject.FindWithTag("Player");
        circleField = gm.circleField.GetComponent<CircleField>();
    }
    void set_direction(Vector2 direction) {
        image.material.SetVector("_cam_direction", direction);
    }

    void Update() {
        var dire = circleField.targetPoint - player.transform.position;
        set_direction(dire);
    }
}
