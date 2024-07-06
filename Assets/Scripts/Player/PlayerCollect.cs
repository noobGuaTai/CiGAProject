using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollect : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Potion")
        {
            StartCoroutine(MovePotionToPlayer(other.gameObject));
        }
    }

    IEnumerator MovePotionToPlayer(GameObject potion)
    {
        Vector3 startPosition = potion.transform.position;
        Vector3 endPosition = transform.position;
        float duration = 0.3f;
        float elapsedTime = 0f;

        while (elapsedTime < duration && potion != null)
        {
            elapsedTime += Time.deltaTime;
            float ratio = elapsedTime / duration;
            potion.transform.position = Vector3.Lerp(startPosition, endPosition, ratio);
            yield return null;
        }
    }
}
