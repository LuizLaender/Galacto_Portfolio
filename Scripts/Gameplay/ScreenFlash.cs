using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScreenFlash : MonoBehaviour
{
    [SerializeField] private Image flashImage;
    [SerializeField] private float flashDuration;

    void Awake()
    {
        flashImage.color = new Color(1, 1, 1, 0);
        flashImage.enabled = true;
    }

    public void Flash()
    {
        StartCoroutine(FlashRoutine());
    }

    private IEnumerator FlashRoutine()
    {
        float halfDuration = flashDuration / 2f;

        for (float t = 0; t < halfDuration; t += Time.deltaTime)
        {
            float alpha = Mathf.Lerp(0, 1, t / halfDuration);
            flashImage.color = new Color(1, 1, 1, alpha);
            yield return null;
        }

        flashImage.color = new Color(1, 1, 1, 1);
        yield return null;

        for (float t = 0; t < halfDuration; t += Time.deltaTime)
        {
            float alpha = Mathf.Lerp(1, 0, t / halfDuration);
            flashImage.color = new Color(1, 1, 1, alpha);
            yield return null;
        }

        flashImage.color = new Color(1, 1, 1, 0);
    }
}
