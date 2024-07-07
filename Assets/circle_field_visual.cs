using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class circle_field_visual : MonoBehaviour
{
    Image image;
    // Start is called before the first frame update
    public GameObject circleField;
    void Start()
    {
        image = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        image.material.SetVector("_center_pos",
            new Vector2(circleField.transform.position.x,
            circleField.transform.position.y));
        var cc = circleField.GetComponent<CircleCollider2D>();
        image.material.SetFloat("_outter", 2 * cc.radius);
    }
}
