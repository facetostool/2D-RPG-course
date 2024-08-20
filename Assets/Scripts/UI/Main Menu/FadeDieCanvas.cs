using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeDieCanvas : FadeCanvas
{
    
    [Header("Died canvas")]
    [SerializeField] private CanvasGroup diedCanvas;
    [SerializeField] private float diedFadeDuration = 1;

    private void Start()
    {
        OnFadeOutComplete += () =>
        {
            diedCanvas.gameObject.SetActive(true);
            StartCoroutine(FadeCanvasGroupWithoutAction(diedCanvas, 1, 0, diedFadeDuration));
        };
        
        OnFadeInComplete += () =>
        {
            diedCanvas.gameObject.SetActive(true);
            StartCoroutine(FadeCanvasGroupWithoutAction(diedCanvas, 0, 1, diedFadeDuration));
        };
        
        diedCanvas.gameObject.SetActive(false);
    }
}
