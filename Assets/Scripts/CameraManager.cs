using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{

    public Camera mainCamera;
    bool firstTween;
    bool secondTween;

    private static CameraManager _instance;

    public static CameraManager Instance { get { return _instance; } }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    private void Update()
    {
        int flossCount = Metronome.Instance.flossCount;

        if (flossCount >= 20 && flossCount <= 30)
            TweenCamera(3.5f);
        else if (flossCount > 30 && flossCount <= 40)
            TweenCamera(4.5f);
        else if (flossCount > 40 && flossCount <= 50)
            TweenCamera(5.5f);
        else if (flossCount > 50 && flossCount <= 60)
            TweenCamera(6.0f);
        else if (flossCount > 60 && flossCount <= 70)
            TweenCamera(7.0f);
        else if (flossCount > 70 && flossCount <= 80)
            TweenCamera(7.5f);
        else if (flossCount > 90)
            TweenCamera(8f);
    }

    public void TweenCamera(float orthoSize)
    {
        if (orthoSize == 3.5f && !firstTween)
            firstTween = true;

        if (orthoSize == 4f && !secondTween)
            secondTween = true;

        mainCamera.DOOrthoSize(orthoSize, 1.2f).SetEase(Ease.InCirc);
    }
}
