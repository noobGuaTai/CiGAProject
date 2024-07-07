using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tex_button : MonoBehaviour
{
    public Texture normalTexture;
    public Texture hoveredTexture;
 
    void OnMouseOver() {
        GetComponent<Renderer>().material.mainTexture = hoveredTexture;
    }

    void OnMouseExit() {
        GetComponent<Renderer>().material.mainTexture = normalTexture;
    }
}
