using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class DraggableObject : MonoBehaviour
{
    private Rigidbody2D rb;
    private bool isDragging = false;
    private Vector2 lastMousePosition;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void OnMouseDown()
    {
        isDragging = true;
        // rb.isKinematic = true; // 暂时关闭物理引擎影响
        lastMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    void OnMouseUp()
    {
        isDragging = false;
        // rb.isKinematic = false; // 重新启用物理引擎影响
        Vector2 currentMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        // Vector2 releaseVelocity = (currentMousePosition - lastMousePosition) / Time.deltaTime;
        // rb.velocity = releaseVelocity; // 给予物体释放时的速度
    }

    void Update()
    {
        if (isDragging)
        {
            Vector2 currentMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 offset = currentMousePosition - lastMousePosition;
            Vector2 force = offset / Time.deltaTime; // 计算力
            rb.AddForce(force); // 施加力实现物体移动
            // transform.position = rb.position + offset;//改变位置实现物体移动
            lastMousePosition = currentMousePosition;
        }
    }
}
