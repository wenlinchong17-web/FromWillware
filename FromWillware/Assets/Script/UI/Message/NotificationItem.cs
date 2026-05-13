using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class NotificationItem : MonoBehaviour
{
    public TMP_Text messageText;
    public CanvasGroup canvasGroup;

    public float lifeTime = 2f;

    public void Init(string msg)
    {
        messageText.text = msg;

        StartCoroutine(Play());
    }

    IEnumerator Play()
    {
        // 淡入
        float t = 0;

        while (t < 0.3f)
        {
            t += Time.deltaTime;
            canvasGroup.alpha = t / 0.3f;
            yield return null;
        }

        canvasGroup.alpha = 1;

        // 停留
        yield return new WaitForSeconds(lifeTime);

        // 淡出
        t = 0;

        while (t < 0.3f)
        {
            t += Time.deltaTime;
            canvasGroup.alpha = 1 - t / 0.3f;
            yield return null;
        }

        Destroy(gameObject);
    }
}