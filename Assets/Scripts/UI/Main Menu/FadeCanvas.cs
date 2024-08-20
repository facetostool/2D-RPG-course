using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeCanvas : MonoBehaviour
{
    [SerializeField] protected CanvasGroup canvasGroup;
    public Action OnFadeOutComplete;
    public Action OnFadeInComplete;
    public virtual void FadeIn(float fadeDuration)
    {
        canvasGroup.gameObject.SetActive(true);
        StartCoroutine(FadeCanvasGroup(canvasGroup, 0, 1, fadeDuration));
    }
    
    public virtual void FadeOut(float fadeDuration)
    {
        canvasGroup.gameObject.SetActive(true);
        StartCoroutine(FadeCanvasGroup(canvasGroup, 1, 0, fadeDuration));
    }
    
    protected IEnumerator FadeCanvasGroup(CanvasGroup canvasGroup, float start, float end, float duration)
    {
        float elapsedTime = 0;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(start, end, elapsedTime / duration);
            yield return null;
        }
        
        canvasGroup.alpha = end;
        
        if (end == 0)
            OnFadeOutComplete?.Invoke();
        else
            OnFadeInComplete?.Invoke();
    }
    
    protected IEnumerator FadeCanvasGroupWithoutAction(CanvasGroup canvasGroup, float start, float end, float duration)
    {
        float elapsedTime = 0;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(start, end, elapsedTime / duration);
            yield return null;
        }
        
        canvasGroup.alpha = end;
    }
    
}
