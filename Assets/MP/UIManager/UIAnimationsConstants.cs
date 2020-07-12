using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public static class UIAnimationsConstants
{
    public static void EnterVertical(RectTransform t, TweenCallback onEnterComplete = null)
    {
        var tween = t.DOAnchorPosY(0f, 0.2f);

        if(onEnterComplete != null)
        {
            tween.OnComplete(onEnterComplete);
        }
    }

    public static void ExitVertical(RectTransform t, TweenCallback onExitComplete = null)
    {
        var tween = t.DOAnchorPosY(1000f, 0.2f);

        if (onExitComplete != null)
        {
            tween.OnComplete(onExitComplete);
        }
    }
}
