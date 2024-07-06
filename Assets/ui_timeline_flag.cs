using System.Collections;
using UnityEngine;

public class ui_timeline_flag : MonoBehaviour
{
    public float end_x;
    public float start_x;
    public float duration = 20f;
    public GameObject globalManager;

    private RectTransform rectTransform;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        StartCoroutine(MoveOverTime());
    }

    void Update()
    {
        duration = globalManager.GetComponent<GlobalManager>().groundTime;
    }

    public IEnumerator MoveOverTime()
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float ratio = elapsedTime / duration;

            Vector3 position = rectTransform.anchoredPosition;
            position.x = Mathf.Lerp(start_x, end_x, ratio);
            rectTransform.anchoredPosition = position;

            yield return null;
        }

        Vector3 finalPosition = rectTransform.anchoredPosition;
        finalPosition.x = end_x;
        rectTransform.anchoredPosition = finalPosition;
    }
}
