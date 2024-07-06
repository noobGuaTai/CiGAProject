using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ui_timeline_flag : MonoBehaviour
{
    public float end_y;
    public float start_y;

    public void move() {
        // TODO: get real time
        float time_prograss = 0;
        float ratio = 0;

        Vector3 vector3 = transform.position;
        vector3.y = ratio * (end_y - start_y) + start_y;

        transform.position = vector3;
    }
}
