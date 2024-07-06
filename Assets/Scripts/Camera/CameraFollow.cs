using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

//[DefaultExecutionOrder(10)]
public class CameraFollow : MonoBehaviour
{
    public UnityEngine.Transform target;
    public float smoothing = 20f;
    public GameObject globalManager;
    public float zoomSpeed = 100f;
    public Vector3 offset;

    public bool isZoomIn = false;

    void Start()
    {
        // offset = transform.position - target.position;
    }

    void Update()
    {
        if (globalManager.GetComponent<GlobalManager>().isStart)
        {
            Vector3 targetCamPos = target.position + offset;
            //transform.position = Vector3.Lerp(transform.position, targetCamPos, smoothing * Time.deltaTime);
            transform.position = targetCamPos;
            ZoomOut();
        }
        else if (!isZoomIn)
        {
            ZoomIn();
            if (GetComponent<Camera>().orthographicSize == 90)
                isZoomIn = true;
        }
    }

    public void ZoomIn()
    {
        GetComponent<Camera>().orthographicSize = Mathf.Max(GetComponent<Camera>().orthographicSize - zoomSpeed * Time.deltaTime, 45);
    }

    public void ZoomOut()
    {
        GetComponent<Camera>().orthographicSize = Mathf.Min(GetComponent<Camera>().orthographicSize + zoomSpeed * Time.deltaTime, 90);
    }
}
