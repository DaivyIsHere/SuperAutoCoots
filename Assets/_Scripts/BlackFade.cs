using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class BlackFade : PersistentSingleton<BlackFade>
{
    [SerializeField] private float fadeDuration = 0.5f;
    [SerializeField] private float delayDuration = 0.5f;
    [SerializeField] private Image blackFadeImage;

    private void Start() 
    {
        blackFadeImage.raycastTarget = false;    
    }

    public void FadeTransition(TweenCallback action)
    {
        StartCoroutine(FadeInAndOut(action));
    }

    IEnumerator FadeInAndOut(TweenCallback action)
    {
        blackFadeImage.DOFade(1, fadeDuration).OnComplete(action);
        yield return new WaitForSecondsRealtime(delayDuration);
        blackFadeImage.DOFade(0, fadeDuration);
    }
}
