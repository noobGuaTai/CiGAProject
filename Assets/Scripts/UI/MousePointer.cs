using UnityEngine;

public class MousePointer : MonoBehaviour
{
    public Texture2D pointerTexture; // 自定义鼠标指针纹理
    public Vector2 hotSpot = Vector2.zero; // 热点，默认为纹理的左上角

    private void Start()
    {
        Cursor.SetCursor(pointerTexture, hotSpot, CursorMode.Auto);
    }

    public void OnDisable()
    {
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }
}
